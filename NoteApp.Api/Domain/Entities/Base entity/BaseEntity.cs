namespace NoteApp.Api.Domain.Entities.Base_entity;
public abstract class BaseEntity
{
	public Guid Id { get; set; }
	public DateTime CreatedAt { get; set; }
	public DateTime? UpdatedAt { get; set; }
}
