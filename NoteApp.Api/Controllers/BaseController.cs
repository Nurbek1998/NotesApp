using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace NoteApp.Api.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
	protected Guid CurrentUserId
	{
		get
		{
			var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
				throw new UnauthorizedAccessException("Invalid user ID");

			return userId;
		}
	}

	protected bool IsCurrentUserRoleAdmin => User.IsInRole("admin");

}
