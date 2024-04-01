using BankLoginPage.Extensions;
using BankLoginPage.Models.ViewModels;
using FluentValidation;

namespace BankLoginPage.Validators
{
    public class LoginValidator:AbstractValidator<LoginViewModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Username).NotEmpty().NotNull().WithMessage("Username is required");
            RuleFor(x => x.Password).NotEmpty().NotNull().WithMessage("Password is required");
        }
    }
}
