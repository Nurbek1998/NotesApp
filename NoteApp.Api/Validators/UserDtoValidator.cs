using FluentValidation;
using NoteApp.Api.Domain.Dtos.UserDtos;

namespace NoteApp.Api.Validators;

public class UserDtoValidator : AbstractValidator<UserForCreationDto>
{
	public UserDtoValidator()
	{
		RuleFor(u => u.Username)
		   .NotEmpty().WithMessage("Username is required")
		   .MinimumLength(3).WithMessage("Username must be at least 3 characters long.");

		RuleFor(u => u.Password)
			.NotEmpty().WithMessage("Password is required")
			.MinimumLength(6).WithMessage("Password must be at least 6 charachter");

		RuleFor(u => u.Email)
			.NotEmpty().WithMessage("Email is required")
			.EmailAddress().WithMessage("Invalid email format.")
			.MinimumLength(6).WithMessage("Email must be at least 6 characters long.");

	}
}
