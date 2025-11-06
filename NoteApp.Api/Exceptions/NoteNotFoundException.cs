using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Net;

namespace NoteApp.Api.Exceptions;
public class NoteNotFoundException : Exception
{
	public HttpStatusCode StatusCode { get; } = HttpStatusCode.NotFound;

	public NoteNotFoundException(string title)
		: base($"Note with the title {title} is not found ")
	{

	}

	public NoteNotFoundException(Guid id)
		: base($"Note the givent id {id} is not found")
	{

	}
}

