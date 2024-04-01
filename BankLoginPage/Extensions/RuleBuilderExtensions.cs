using FluentValidation;

namespace BankLoginPage.Extensions
{
	public static class RuleBuilderExtensions
	{
		public static void Password<T>(this IRuleBuilder<T, string> ruleBuilder)
		{
			ruleBuilder
				.NotEmpty().WithMessage("Your password cannot be empty")
				.MinimumLength(8).WithMessage("Your password length must be at least 8.")
				.Matches("[A-Z]").WithMessage("Your password must contain at least one uppercase letter.")
				.Matches("[a-z]").WithMessage("Your password must contain at least one lowercase letter.")
				.Matches("[0-9]").WithMessage("Your password must contain at least one number.")
				.Matches("[^a-zA-Z0-9]").WithMessage("Your password must contain at least one alphanumeric character ");
		}
	}
}
