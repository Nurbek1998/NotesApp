
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using NoteApp.Api.Domain.Dtos.AuthDtos;
using NoteApp.Api.Interfaces;

namespace NoteApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
	private readonly IAuthService _authService = authService;

	[HttpPost("register")]
	public async Task<IActionResult> RegisterAsync(RegisterRequest dto)
	{
		var token = await _authService.RegisterAsync(dto);
		return Ok(new { Token = token });
	}

	[HttpPost("login")]
	public async Task<IActionResult> LoginAsync(LoginRequest dto)
	{
		var token = await _authService.LoginAsync(dto);
		return Ok(new { Token = token });
	}
}
