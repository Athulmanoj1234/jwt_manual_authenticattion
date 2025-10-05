using jwtmanualauthentication.Data;
using jwtmanualauthentication.Models;
using jwtmanualauthentication.Models.Enities;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

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
        
        public UsersController(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
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
