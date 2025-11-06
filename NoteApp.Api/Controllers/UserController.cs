using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteApp.Api.Domain.Dtos.UserDtos;
using NoteApp.Api.Interfaces;
using System.Security.Claims;

namespace NoteApp.Api.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UserController(IUserService userService) : BaseController
{
	private readonly IUserService _userService = userService;

	[Authorize(Policy = "AdminPolicy")]
	[HttpGet]
	public async Task<IActionResult> GetAllUsersAsync(CancellationToken cancellationToken)
		=> Ok(await _userService.GetAllAsync(cancellationToken));


	[Authorize(Policy = "AdminOrUserPolicy")]
	[HttpGet("id/{id}", Name = "GetUserById")]
	public async Task<IActionResult> GetUserByIdAsync(Guid id, CancellationToken cancellationToken)
		=> Ok(await _userService.GetByIdAsync(id, cancellationToken));


	[Authorize(Policy = "AdminOrUserPolicy")]
	[HttpGet("email/{email}")]
	public async Task<IActionResult> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
		=> Ok(await _userService.GetByEmailAsync(email, cancellationToken));

	
	[Authorize(Policy = "AdminPolicy")]
	[HttpPost]
	public async Task<IActionResult> CreateUserAsync(UserForCreationDto dto, CancellationToken cancellationToken)
	{
		var createdUser = await _userService.CreateUserAsync(dto, cancellationToken);

		return CreatedAtAction("GetUserById", new { createdUser.Id }, createdUser);
	}


	[Authorize(Policy = "AdminOrUserPolicy")]
	[HttpPut("{id}")]
	public async Task<IActionResult> UpdateUserAsync(Guid id, UserForUpdateDto dto, CancellationToken cancellationToken)
	{
		await _userService.UpdateUserAsync(id, dto, cancellationToken);

		return NoContent();
	}

	[Authorize(Policy = "AdminPolicy")]
	[HttpDelete("{id}")]
	public async Task<IActionResult> DeleteUserAsync(Guid id, CancellationToken cancellationToken)
	{
		await _userService.DeleteUserAsync(id, cancellationToken);
		return NoContent();
	}

}
