using NoteApp.Api.Interfaces;
using System.Security.Claims;

namespace NoteApp.Api.Services;
public class CurrentUserContext(IHttpContextAccessor httpContextAccessor) : ICurrentUserContext
{
	private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
	private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;
	public Guid Id
	{
		get
		{
			var idClaim = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			if (idClaim is  null || !Guid.TryParse(idClaim, out var id))
				throw new UnauthorizedAccessException("Invalid or missing user ID in token.");
			return id;
		}
	}

	public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

	public bool IsAdmin => User?.FindFirst(ClaimTypes.Role)?.Value == "admin";

	public string? UserRole => User?.FindFirst(ClaimTypes.Role)?.Value;

	public string? Email => User?.FindFirst(ClaimTypes.Email)?.Value;

	public string? UserName => User?.FindFirst(ClaimTypes.Name)?.Value;
}
