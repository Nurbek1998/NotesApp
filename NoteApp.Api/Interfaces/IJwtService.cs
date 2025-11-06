using NoteApp.Api.Domain.Entities;

namespace NoteApp.Api.Interfaces;
public interface IJwtService
{
	string GenerateToken(User user);
}
