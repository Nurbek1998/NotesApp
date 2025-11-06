using NoteApp.Api.Domain.Dtos.UserDtos;
using NoteApp.Api.Domain.Entities;

namespace NoteApp.Api.Interfaces;
public interface IUserService
{
	Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken = default);
	Task<List<UserForResultDto>> GetAllAsync(CancellationToken cancellationToken = default);
	Task<UserForResultDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
	Task<UserForResultDto?> GetByEmailAsync(string email, CancellationToken cancellationToken);
	Task<UserForResultDto> CreateUserAsync(UserForCreationDto userDto, CancellationToken cancellationToken = default);
	Task<UserForResultDto> UpdateUserAsync(Guid targetUserId, UserForUpdateDto userDto, CancellationToken cancellationToken);
}
