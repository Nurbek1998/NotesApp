using NoteApp.Api.Domain.Dtos.AuthDtos;
using NoteApp.Api.Exceptions;
using NoteApp.Api.Interfaces;
using BCrypt.Net;
using Microsoft.AspNetCore.Identity;
using NoteApp.Api.Domain.Entities;
using AutoMapper;

namespace NoteApp.Api.Services;
public class AuthService(
	IUserRepository userRepository,
	IUnitOfWork unitOfWork,
	IJwtService jwtService,
	IPasswordHasher<User> passwordHasher) : IAuthService
{
	private readonly IUserRepository _userRepository = userRepository;
	private readonly IJwtService _jwtService = jwtService;
	private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
	private readonly IUnitOfWork _unitOfWork = unitOfWork;
	public async Task<string> LoginAsync(LoginRequest dto, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(dto);

		var email = dto.Email.Trim();

		var user = await _userRepository.GetByEmailAsync(email, cancellationToken)
			?? throw new UserNotFoundException(email);

		var result = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
		if (result == PasswordVerificationResult.Failed)
			throw new InvalidCredentialsException("Invalid credentials");

		return _jwtService.GenerateToken(user);
	}

	public async Task<string> RegisterAsync(RegisterRequest dto, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(dto);

		var existingUser = await _userRepository.GetByEmailAsync(dto.Email, cancellationToken);

		if (existingUser is not null)
			throw new UserAlreadyExistsException(dto.Email);

		var user = new User
		{
			Username = dto.Username.Trim(),
			Email = dto.Email.Trim(),
			Role = "user"
		};

		user.Password = _passwordHasher.HashPassword(user, dto.Password.Trim());
		user.CreatedAt = DateTime.UtcNow;

		await _userRepository.CreateUserAsync(user, cancellationToken);

		await _unitOfWork.SaveChangesAsync(cancellationToken);

		return _jwtService.GenerateToken(user);
	}
}
