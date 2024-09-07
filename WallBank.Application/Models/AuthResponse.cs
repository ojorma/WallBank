
namespace WallBank.Application.Models
{
    public class AuthResponse
    {
        public string Id { get; set; }
        public string Email { get; set; }

        //  public bool IsVerified { get; set; }
        public string FullName { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string JWToken { get; set; }
        public string RefreshToken { get; set; }
         public bool IsVerified { get; set; }
        public bool Is2FA { get; set; }

    }
}
