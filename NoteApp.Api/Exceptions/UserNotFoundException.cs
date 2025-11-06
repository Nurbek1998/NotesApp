using System.Net;

namespace NoteApp.Api.Exceptions;
public class UserNotFoundException : Exception
{
	public UserNotFoundException(string message)
		: base($"User with '{message}' was not found")
	{

	}

	public UserNotFoundException(Guid id)
		: base($"User with id '{id}' was not found")
	{

	}

	public HttpStatusCode StatusCode { get; } = HttpStatusCode.NotFound;
}
