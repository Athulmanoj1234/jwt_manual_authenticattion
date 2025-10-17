using jwtmanualauthentication.Data;
using jwtmanualauthentication.Migrations;
using jwtmanualauthentication.Models;
using jwtmanualauthentication.Models.Enities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace jwtmanualauthentication.services
{
    public class AuthService
    {
        private ApplicationDbContext _dbContext;
        public AuthService(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public string GenerateRefreshToken() {
            var randomNumber = new byte[32];  //creates 32 zeroes in total [0, 0, 0, .....0] upto 32;
            using var rng = RandomNumberGenerator.Create(); //“Hey OS, give me access to your secure random number generator, and I’ll close it after I’m done.”  //rng is a reference to that internal secure random generator. It implements IDisposable(hence using var is used). Creates an instance of the default implementation of a cryptographic random number generator that can be used to generate random data. — specifically for generating cryptographically secure random numbers (CSPRNG).
            rng.GetBytes(randomNumber);  //GetBytes(data) here the data is the array to fill;
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<string> GenerateAndSaveRefreshTokenAsync(User user) {
            var refreshToken = GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpirytime = DateTime.UtcNow.AddDays(7);
            await _dbContext.SaveChangesAsync();
            return refreshToken;
        }

        public async Task<User> ValidateRefreshTokenAsync(Guid UserId, string refreshToken) {
            var user = await _dbContext.FindAsync<User>(UserId);

            if (user is null || refreshToken != user.RefreshToken || user.RefreshTokenExpirytime <= DateTime.UtcNow) {
                //if refresh token expiry time is less than current time delete the refreshtoken from database
                return null;
            }
            return user;
        }

        public async Task<TokenResponseDto> GenerateAccessTokenByRefreshAsync(TokenRequestDto request) {
            var user = await ValidateRefreshTokenAsync(request.UserId, request.RefreshToken);

            if (user is null) {
                return null;
            }
            // logic to create accessToken and sent it by response
            return 
        }
    }
}
