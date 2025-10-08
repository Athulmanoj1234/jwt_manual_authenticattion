using jwtmanualauthentication.Data;
using jwtmanualauthentication.Models;
using jwtmanualauthentication.Models.Enities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace jwtmanualauthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public byte[] salt;
        public string? hashed;
        bool isLoggedIn;
        private ApplicationDbContext dbContext;
        private readonly IConfiguration _configuration;
        
        public UsersController(ApplicationDbContext dbContext, IConfiguration configuration) {
            this.dbContext = dbContext;
            this._configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult Register(UserDto userDto) {
            this.salt = RandomNumberGenerator.GetBytes(128 / 8);   //goes into GetBytes(128 / 8) GetBytes(n) n number of arrays
            //this.hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            //    password: userDto.Password!,
            //    salt: this.salt,
            //    iterationCount: 1000, 
            //    prf: KeyDerivationPrf.HMACSHA256,  //prf tells Pbkdf2 that which hashing function needs to be used here we used for genrate hash
            //    numBytesRequested: 256
            //    ));
            hashed = BCrypt.Net.BCrypt.HashPassword(userDto?.Password);
            var user = new User()
            {
                Username = userDto.Username,
                Password = hashed,
            };
            try
            {
                dbContext.Users.Add(user);
                dbContext.SaveChanges();
                return Ok(user);
            }
            catch (Exception e) {
                return StatusCode(500);
            }
        }

        [HttpPost("login")]
        public IActionResult Login(UserDto userDto) {
            try
            {
                var user = dbContext.Users.SingleOrDefault(doc => doc.Username == userDto.Username);
                if (user is null)
                {
                    return BadRequest();
                } else {
                    isLoggedIn = BCrypt.Net.BCrypt.Verify(userDto.Password, user.Password);
                    if (isLoggedIn)
                    {
                        var claims = new[]
                            {
                                new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),  //Subject (usually the user or principal)
                                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  //JWT ID (unique identifier for this token)
                                new Claim("userId", userDto.Username.ToString())  
                            };
                        
                        var key = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"])
                            );
                        //Symmetric means same key is used for both sides
                        //Symmetric sceurityKey means Represents a key used for signing/validating JWTs with the same secret
                        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                        //creating signin credentials: signin credentials tells the jwt middleware how to sign the token.
                        //key → the symmetric key we just created.SecurityAlgorithms.HmacSha256 → uses HMAC SHA - 256 algorithm for signing.

                        //creating the jwt token
                        //JwtSecurityToken represents the actual token
                        var token = new JwtSecurityToken(
                            _configuration["Jwt:Issuer"],  //iss claim, identifies who issued the token (Jwt:Issuer)
                            _configuration["Jwt:Audience"], //aud claim, identifies who the token is intended for (Jwt:Audience)
                            claims, //payload data about the user
                            expires: DateTime.UtcNow.AddMinutes(60), //DateTime.UtcNow (cordinated universal time ie gets the current date and time) and AddMinutes(minute) add extra minutes in this case extra 60 minutes added.
                            signingCredentials: signIn //sign in credentials which earlier used to sign token
                            );
                        return Ok();
                    }
                    else {
                        return BadRequest();
                    }
                }
                
            }
            catch (Exception e) {
                return StatusCode(500);
            }
        }

        //public 
    }
}
