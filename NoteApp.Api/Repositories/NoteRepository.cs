using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NoteApp.Api.Data;
using NoteApp.Api.Domain.Entities;
using NoteApp.Api.Interfaces;
using System.Runtime.InteropServices;

namespace NoteApp.Api.Repositories;
public class NoteRepository(AppDbContext dbContext) : INoteRepository
{
	private readonly AppDbContext _context = dbContext;

	public async Task CreateAsync(Note note, CancellationToken cancellationToken = default)
		=> await _context.Notes.AddAsync(note, cancellationToken);

	public void Delete(Note note)
		=> _context.Notes.Remove(note);

	public async Task<List<Note>> GetAllAsync(CancellationToken cancellationToken = default)
		=> await _context.Notes
			.AsNoTracking()
			.ToListAsync(cancellationToken);

	public async Task<List<Note>> GetAllByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
		=> await _context.Notes
			.AsNoTracking()
			.Where(n => n.UserId == userId)
			.ToListAsync(cancellationToken);

	public async Task<Note?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
		=> await _context.Notes
			.AsNoTracking()
			.FirstOrDefaultAsync(n => n.Id == id, cancellationToken);

	public async Task<Note?> GetByTitleAsync(string title, CancellationToken cancellationToken = default)
		=> await _context.Notes
			.AsNoTracking()
			.FirstOrDefaultAsync(n => n.Title == title, cancellationToken);


	public void Update(Note note)
		=> _context.Notes.Update(note);
}
