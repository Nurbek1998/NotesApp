using AutoMapper;
using Microsoft.AspNetCore.Identity;
using NoteApp.Api.Domain.Dtos.UserDtos;
using NoteApp.Api.Domain.Entities;
using NoteApp.Api.Exceptions;
using NoteApp.Api.Interfaces;

namespace NoteApp.Api.Services;
public class UserService(
	IUserRepository userRepository,
	IMapper mapper,
	IUnitOfWork unitOfWork,
	IPasswordHasher<User> passwordHasher,
	ICurrentUserContext currentUserContext
	) : IUserService
{
	private readonly IUserRepository _userRepository = userRepository;
	private readonly IMapper _mapper = mapper;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
	private readonly ICurrentUserContext _currentUser = currentUserContext;

	public async Task<UserForResultDto> CreateUserAsync(UserForCreationDto userDto, CancellationToken cancellationToken)
	{
		var normalizedEmail = userDto.Email.Trim().ToLowerInvariant();

		if (await _userRepository.GetByEmailAsync(normalizedEmail, cancellationToken) is not null)
			throw new DuplicateEmailException(normalizedEmail);

		var user = _mapper.Map<User>(userDto);

		user.Password = _passwordHasher.HashPassword(user, userDto.Password);
		user.CreatedAt = DateTime.UtcNow;

		await _userRepository.CreateUserAsync(user, cancellationToken);

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return _mapper.Map<UserForResultDto>(user);
	}

	public async Task<bool> DeleteUserAsync(Guid id, CancellationToken cancellationToken)
	{
		var existingUser = await _userRepository
			.GetByIdAsync(id, cancellationToken)
			?? throw new UserNotFoundException(id);

		EnsureAccess(existingUser);

		_userRepository.Delete(existingUser);

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return true;
	}

	public async Task<List<UserForResultDto>> GetAllAsync(CancellationToken cancellationToken)
	{
		if (!_currentUser.IsAdmin)
			throw new ForbiddenException("Only admins can view all users.");

		var users = await _userRepository.GetAllAsync(cancellationToken);
		return _mapper.Map<List<UserForResultDto>>(users);
	}

	public async Task<UserForResultDto?> GetByEmailAsync(string email, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByEmailAsync(email, cancellationToken)
			?? throw new UserNotFoundException(email);

		EnsureAccess(user);
		return _mapper.Map<UserForResultDto>(user);
	}

	public async Task<UserForResultDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByIdAsync(id, cancellationToken)
			?? throw new UserNotFoundException(id);

		EnsureAccess(user);
		return _mapper.Map<UserForResultDto>(user);
	}

	public async Task<UserForResultDto?> GetByIdWithNotesAsync(Guid id, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByIdWithNotesAsync(id, cancellationToken)
			?? throw new UserNotFoundException(id);

		EnsureAccess(user);
		return _mapper.Map<UserForResultDto>(user);
	}

	public async Task<UserForResultDto> UpdateUserAsync(Guid targetUserId, UserForUpdateDto userDto, CancellationToken cancellationToken)
	{
		var existingUser = await _userRepository.GetByIdAsync(targetUserId, cancellationToken)
			?? throw new UserNotFoundException(targetUserId);

		EnsureAccess(existingUser);

		_mapper.Map(userDto, existingUser);

		if (!string.IsNullOrWhiteSpace(userDto.Password))
		{
			existingUser.Password = _passwordHasher.HashPassword(existingUser, userDto.Password);
		}

		existingUser.UpdatedAt = DateTime.UtcNow;

		_userRepository.Update(existingUser);
		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return _mapper.Map<UserForResultDto>(existingUser);
	}

	private void EnsureAccess(User targetUser)
	{
		if (_currentUser.IsAdmin)
			return;

		if (targetUser.Id != _currentUser.Id)
			throw new ForbiddenException();
	}
}
