using AutoMapper;
using NoteApp.Api.Domain.Dtos.NoteDtos;
using NoteApp.Api.Domain.Entities;
using NoteApp.Api.Exceptions;
using NoteApp.Api.Interfaces;

namespace NoteApp.Api.Services;
public class NoteService(
	INoteRepository noteRepository,
	IMapper mapper,
	IUnitOfWork unitOfWork,
	ICurrentUserContext currentUserContext
	) : INoteService
{
	private readonly IMapper _mapper = mapper;
	private readonly INoteRepository _repository = noteRepository;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly ICurrentUserContext _currentUserContext = currentUserContext;

	public async Task<NoteForResultDto> CreateAsync(NoteForCreationDto noteDto, CancellationToken cancellationToken = default)
	{
		var note = _mapper.Map<Note>(noteDto);

		note.UserId = _currentUserContext.Id;
		note.CreatedAt = DateTime.UtcNow;

		await _repository.CreateAsync(note, cancellationToken);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return _mapper.Map<NoteForResultDto>(note);
	}

	public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
	{
		var note = await _repository.GetByIdAsync(id, cancellationToken)
			?? throw new NoteNotFoundException(id);

		EnsureAccess(note);

		_repository.Delete(note);

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return true;
	}

	public async Task<List<NoteForResultDto>> GetAllAsync(CancellationToken cancellationToken = default)
	{
		List<Note> notes;

		if (_currentUserContext.IsAdmin)
			notes = await _repository.GetAllAsync(cancellationToken);

		else
			notes = await _repository.GetAllByUserIdAsync(_currentUserContext.Id, cancellationToken);

		return _mapper.Map<List<NoteForResultDto>>(notes);
	}

	public async Task<NoteForResultDto?> GetByIdAsync(Guid noteId, CancellationToken cancellationToken = default)
	{
		var note = await _repository.GetByIdAsync(noteId, cancellationToken)
			?? throw new NoteNotFoundException(noteId);

		EnsureAccess(note);

		return _mapper.Map<NoteForResultDto>(note);
	}

	public async Task<NoteForResultDto?> GetByTitleAsync(string title, CancellationToken cancellationToken = default)
	{
		var normalizedTitle = title.Trim().ToLowerInvariant();
		var note = await _repository.GetByTitleAsync(normalizedTitle, cancellationToken)
			?? throw new NoteNotFoundException(normalizedTitle);

		EnsureAccess(note);

		return _mapper.Map<NoteForResultDto>(note);
	}

	public async Task<NoteForResultDto> UpdateAsync(Guid noteId, NoteForUpdateDto noteDto, CancellationToken cancellationToken = default)
	{
		var note = await _repository.GetByIdAsync(noteId, cancellationToken)
			?? throw new NoteNotFoundException(noteId);

		EnsureAccess(note);

		_mapper.Map(noteDto, note);
		note.UpdatedAt = DateTime.UtcNow;

		_repository.Update(note);

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return _mapper.Map<NoteForResultDto>(note);

	}
	private void EnsureAccess(Note note)
	{
		if (_currentUserContext.IsAdmin)
			return;

		if (note.UserId != _currentUserContext.Id)
			throw new ForbiddenException("You do not have permission to perform this action.");
	}
}
