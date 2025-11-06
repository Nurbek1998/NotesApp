using System.Net;

namespace NoteApp.Api.Exceptions;
public class ForbiddenException : Exception
{
	public HttpStatusCode StatusCode { get; } = HttpStatusCode.Forbidden;

	public ForbiddenException(string message = "You do not have permission to perform this action.")
		: base(message)
	{

	}

}
