using NoteApp.Api.Domain.Dtos;
using NoteApp.Api.Domain.Entities;

namespace NoteApp.Api.Interfaces;
public interface IUserRepository
{
	void Delete(User user);
	void Update(User user);
	Task<List<User>> GetAllAsync(CancellationToken cancellationToken = default);
	Task CreateUserAsync(User user, CancellationToken cancellationToken = default);
	Task<User?> GetByIdWithNotesAsync(Guid id, CancellationToken cancellationToken);
	Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
	Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
}
