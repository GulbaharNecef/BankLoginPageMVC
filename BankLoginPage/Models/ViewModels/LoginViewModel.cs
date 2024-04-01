using System.ComponentModel.DataAnnotations;

namespace BankLoginPage.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage ="Username is required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Username is required")]
        public string Password { get; set; }
    }
}
