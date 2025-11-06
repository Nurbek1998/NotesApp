using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using NoteApp.Api.Domain.Dtos.NoteDtos;
using NoteApp.Api.Interfaces;

namespace NoteApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "AdminOrUserPolicy")]
public class NoteController(INoteService noteService) : BaseController
{
	private readonly INoteService _noteService = noteService;

	[HttpGet]
	public async Task<IActionResult> GetAllNotesAsync(CancellationToken cancellationToken)
		=> Ok(await _noteService.GetAllAsync(cancellationToken));


	[HttpGet("{id}", Name = "GetNoteById")]
	public async Task<IActionResult> GetNoteByIdAsync(Guid id, CancellationToken cancellationToken)
		=> Ok(await _noteService.GetByIdAsync(id, cancellationToken));


	[HttpPost]
	public async Task<IActionResult> CreateNoteAsync(NoteForCreationDto dto, CancellationToken cancellationToken)
	{
		var createdNote = await _noteService.CreateAsync(dto, cancellationToken);
		Console.WriteLine(nameof(GetNoteByIdAsync));
		return CreatedAtAction("GetNoteById", new { id = createdNote.Id }, createdNote);
	}


	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateNoteAsync(Guid id, NoteForUpdateDto dto, CancellationToken cancellationToken)
		=> Ok(await _noteService.UpdateAsync(id, dto, cancellationToken));


	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteNoteAsync(Guid id, CancellationToken cancellationToken)
	{
		await _noteService.DeleteAsync(id, cancellationToken);

		return NoContent();
	}

}
