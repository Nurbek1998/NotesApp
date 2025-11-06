using NoteApp.Api.Domain.Dtos.NoteDtos;

namespace NoteApp.Api.Interfaces;
public interface INoteService
{
	Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
	Task<List<NoteForResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
	Task<NoteForResultDto?> GetByIdAsync(Guid noteId, CancellationToken cancellationToken = default);
	Task<NoteForResultDto> CreateAsync(NoteForCreationDto noteDto, CancellationToken cancellationToken = default);
	Task<NoteForResultDto> UpdateAsync(Guid noteId, NoteForUpdateDto noteDto, CancellationToken cancellationToken = default);
	Task<NoteForResultDto?> GetByTitleAsync(string title, CancellationToken cancellationToken = default);
}
