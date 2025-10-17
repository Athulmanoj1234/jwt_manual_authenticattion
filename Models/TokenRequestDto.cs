namespace jwtmanualauthentication.Models
{
    public class TokenRequestDto
    {
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }
    }
}
