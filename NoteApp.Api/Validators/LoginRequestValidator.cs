using FluentValidation;
using NoteApp.Api.Domain.Dtos.AuthDtos;

namespace NoteApp.Api.Validators;
public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
	public LoginRequestValidator()
	{
		RuleFor(x => x.Email)
		   .NotEmpty().WithMessage("Email is required")
		   .EmailAddress().WithMessage("Invalid email format.")
		   .Must(x => x.Trim() == x)
		   .WithMessage("Remove the spaces");

		RuleFor(x => x.Password)
			.NotEmpty().WithMessage("Password is required")
			.Must(x => x.Trim() == x)
			.WithMessage("Password shouldn't contain space");
	}
}
