using NoteApp.Api.Domain.Dtos.AuthDtos;

namespace NoteApp.Api.Interfaces;
public interface IAuthService
{
	Task<string> RegisterAsync(RegisterRequest dto, CancellationToken cancellationToken = default);
	Task<string> LoginAsync(LoginRequest dto, CancellationToken cancellationToken = default);
}
