using BankLoginPage.Extensions;
using BankLoginPage.Models.IdentityEntities;
using BankLoginPage.Models.ViewModels;
using FluentValidation;

namespace BankLoginPage.Validators
{
	public class RegisterValidator : AbstractValidator<RegisterViewModel>
	{
		public RegisterValidator()
		{
			RuleFor(x => x.Username).NotEmpty().NotNull().WithMessage("Username is required");
			RuleFor(x => x.FirstName).NotEmpty().WithMessage("Firstname is required");
			RuleFor(x => x.LastName).NotEmpty().WithMessage("Lastname is required");
			RuleFor(x => x.Email).NotEmpty()
				.WithMessage("Email is required")
				.EmailAddress().WithMessage("A valid email address is required.");
			RuleFor(x => x.Password).Password();
	            
		}
	}
}
