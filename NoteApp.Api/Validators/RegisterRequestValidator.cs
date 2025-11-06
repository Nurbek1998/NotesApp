using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NoteApp.Api.Domain.Dtos.AuthDtos;

namespace NoteApp.Api.Validators;
public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
	public RegisterRequestValidator()
	{
		RuleFor(u => u.Username)
			.NotEmpty().WithMessage("Username is required")
			.MinimumLength(3).WithMessage("Username must be at least 3 characters long.")
			.Must(x => x.Trim() == x)
			.WithMessage("Remove the spaces");

		RuleFor(u => u.Password)
			.NotEmpty().WithMessage("Password is required")
			.MinimumLength(6).WithMessage("Password must be at least 6 charachter")
			.MaximumLength(50).WithMessage("Password cannot exceed 50 characters")
			.Must(x => x.Trim() == x)
			.WithMessage("Password shouldn't contain spaces"); ;

		RuleFor(u => u.Email)
			.NotEmpty().WithMessage("Email is required")
			.EmailAddress().WithMessage("Invalid email format.")
			.MinimumLength(6).WithMessage("Email must be at least 6 characters long.")
			.Must(x => x.Trim() == x)
			.WithMessage("Email shouldn't contain spaces");
	}
}
