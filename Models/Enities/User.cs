namespace jwtmanualauthentication.Models.Enities
{
    public class User
        {
            public Guid Id { get; set; }
            public string? Username { get; set; }
            public string? Password { get; set; }

            //adding refresh tokens to create new access token when the old access token expires
            public string? RefreshToken { get; set; } //intially refresh token will be null
            public DateTime? RefreshTokenExpirytime { get; set; }  //setting the expiry of refresh token
        }
 }
