using NoteApp.Api.Domain.Entities.Base_entity;

namespace NoteApp.Api.Domain.Entities;
public class User : BaseEntity
{
	public string Username { get; set; } = string.Empty;
	public string Password { get; set; } = string.Empty;
	public string Email { get; set; } = string.Empty;
	public string Role { get; set; } = "user";

	public List<Note> Notes { get; set; } = [];

}
