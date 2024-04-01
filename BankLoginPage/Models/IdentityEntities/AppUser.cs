using Microsoft.AspNetCore.Identity;

namespace BankLoginPage.Models.IdentityEntities
{
    public class AppUser : IdentityUser<string>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
