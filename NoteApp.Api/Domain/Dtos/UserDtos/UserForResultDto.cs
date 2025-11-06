namespace NoteApp.Api.Domain.Dtos.UserDtos;
public class UserForResultDto
{
	public Guid Id { get; set; }
	public string Username { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; }
}

