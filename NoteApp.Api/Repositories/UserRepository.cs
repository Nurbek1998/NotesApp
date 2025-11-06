using Microsoft.EntityFrameworkCore;
using NoteApp.Api.Data;
using NoteApp.Api.Domain.Dtos;
using NoteApp.Api.Domain.Entities;
using NoteApp.Api.Interfaces;

namespace NoteApp.Api.Repositories;
public class UserRepository(AppDbContext appDbContext) : IUserRepository
{
	private readonly AppDbContext _context = appDbContext;

	public async Task CreateUserAsync(User user, CancellationToken cancellationToken)
		=> await _context.Users.AddAsync(user, cancellationToken);

	public void Delete(User user)
		=> _context.Users.Remove(user);

	public async Task<List<User>> GetAllAsync(CancellationToken cancellationToken)
		=> await _context.Users
		.AsNoTracking()
		.ToListAsync(cancellationToken);

	public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
	{
		return await _context.Users
			.AsNoTracking()
			.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
	}

	public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
		=> await _context.Users
		.AsNoTracking()
		.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

	public async Task<User?> GetByIdWithNotesAsync(Guid id, CancellationToken cancellationToken)
		=> await _context.Users.
		Include(u => u.Notes)
		.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

	public void Update(User user)
		=> _context.Users.Update(user);

}
