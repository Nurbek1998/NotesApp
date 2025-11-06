using FluentValidation;
using NoteApp.Api.Domain.Dtos.NoteDtos;

namespace NoteApp.Api.Validators;
public class NoteDtoValidator: AbstractValidator<NoteForCreationDto>
{
	public NoteDtoValidator()
	{
		RuleFor(n => n.Title)
			.NotEmpty().WithMessage("Title is required")
			.MaximumLength(100);
	}
}
