
using Microsoft.AspNetCore.Identity;

namespace WallBank.Application.Models
{
    public class ApplicationUser : IdentityUser
    {

        public string? Photo { get; set; }
       
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Country { get; set; } = "";
        public string City { get; set; } = "";
        public string Phone { get; set; } = "";
        public string ContactEmail { get; set; } = "";       
      //  public List<RefreshToken> RefreshTokens { get; set; }
        public bool OwnsToken(string token)
        {
            return true;
         //   return RefreshTokens?.Find(x => x.Token == token) != null;
        }
        
    }

  
}
