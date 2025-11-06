using NoteApp.Api.Domain.Dtos.NoteDtos;
using NoteApp.Api.Domain.Dtos.UserDtos;
using NoteApp.Api.Domain.Entities;

namespace NoteApp.Api.Interfaces;
public interface INoteRepository
{
	void Delete(Note note);
	void Update(Note note);
	Task<List<Note>> GetAllAsync(CancellationToken cancellationToken = default);
	Task<List<Note>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
	Task CreateAsync(Note note, CancellationToken cancellationToken = default);
	Task<Note?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
	Task<Note?> GetByTitleAsync(string title, CancellationToken cancellationToken = default);
}
