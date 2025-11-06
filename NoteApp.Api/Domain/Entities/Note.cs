using NoteApp.Api.Domain.Entities.Base_entity;

namespace NoteApp.Api.Domain.Entities;
public class Note : BaseEntity
{
	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;

	public Guid UserId { get; set; }
	public User User { get; set; } = null!;
}