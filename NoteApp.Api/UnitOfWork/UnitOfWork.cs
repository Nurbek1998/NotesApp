using NoteApp.Api.Data;
using NoteApp.Api.Interfaces;

namespace NoteApp.Api.UnitOfWork;
public class UnitOfWork(AppDbContext dbContext, IUserRepository users, INoteRepository notes) : IUnitOfWork
{
	private readonly AppDbContext _context = dbContext;
	public IUserRepository Users { get; } = users;
	public INoteRepository Notes { get; } = notes;

	public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		=> await _context.SaveChangesAsync(cancellationToken);

	public void Dispose()
	{
		_context.Dispose();
		GC.SuppressFinalize(this);
	}
}
