namespace NoteApp.Api.Interfaces;

public interface ICurrentUserContext
{
	/// <summary>
	/// The ID of the currently authenticated user.
	/// </summary>
	Guid Id { get; }


	/// <summary>
	/// Indicates whether the current HTTP request is made by an authenticated user.
	/// </summary>
	bool IsAuthenticated { get; }


	/// <summary>
	/// Indicates whether the currently authenticated user has the "admin" role.
	/// </summary>
	bool IsAdmin { get; }


	/// <summary>
	/// Gets the role of the currently authenticated user (for example, "admin" or "user").
	/// Returns <c>null</c> if the user is not authenticated.
	/// </summary>
	string? UserRole { get; }


	/// <summary>
	/// Gets the email address of the currently authenticated user.
	/// Returns <c>null</c> if the user is not authenticated or the claim is missing.
	/// </summary>
	string? Email { get; }


	/// <summary>
	/// Gets the username (display name) of the currently authenticated user.
	/// Returns <c>null</c> if the user is not authenticated or the claim is missing.
	/// </summary>
	string? UserName { get; }

}
