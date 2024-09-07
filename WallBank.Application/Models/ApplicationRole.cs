using Microsoft.AspNetCore.Identity;

namespace WallBank.Application.Models
{
    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base()
        {
        }
        public ApplicationRole(string roleName) : base(roleName)
        {
        }
        
        //  public string RoleName { get; set; }
        public string Description { get; set; } = "";
        public bool IsDeleted { get; set; } = false;
        
    }

}
