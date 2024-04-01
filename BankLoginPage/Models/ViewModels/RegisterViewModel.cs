using System.ComponentModel.DataAnnotations;

namespace BankLoginPage.Models.ViewModels
{
	public class RegisterViewModel
	{
		public string Username { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		[Required]
		[StringLength(18, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
		[RegularExpression("^((?=.*[a-z])(?=.*[A-Z])(?=.*\\d)).+$", ErrorMessage = "Password must contain at least one lowercase letter, one UPPERCASE letter, and at least one non-alphanumeric character")]
		public string Password { get; set; }
	}
}
