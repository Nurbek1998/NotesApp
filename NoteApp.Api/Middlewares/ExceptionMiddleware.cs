using NoteApp.Api.Exceptions;
using System.Net;
using System.Text.Json;

namespace NoteApp.Api.Middlewares;
public class ExceptionMiddleware(RequestDelegate next)
{
	private readonly RequestDelegate _next = next;

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{

			await HandleExceptionAsync(context, ex);
		}
	}

	private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		context.Response.ContentType = "application/json";

		var statusCode = exception switch
		{
			UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
			ArgumentNullException => (int)HttpStatusCode.BadRequest,
			KeyNotFoundException => (int)HttpStatusCode.NotFound,
			UserNotFoundException => (int)StatusCodes.Status409Conflict,
			DuplicateEmailException => (int)StatusCodes.Status409Conflict,
			ForbiddenException => (int)HttpStatusCode.Forbidden,
			InvalidCredentialsException => (int)StatusCodes.Status401Unauthorized,
			NoteNotFoundException => (int)HttpStatusCode.NotFound,
			UserAlreadyExistsException => (int)HttpStatusCode.BadRequest,
			_ => (int)HttpStatusCode.InternalServerError
		};

		context.Response.StatusCode = statusCode;

		var response = new
		{
			StatusCode = statusCode,
			Error = exception.Message,
			errorType = exception.GetType().Name,
			Timestamp = DateTime.UtcNow,
			context.TraceIdentifier,
			context.Request.Path
		};

		var json = JsonSerializer.Serialize(response);
		await context.Response.WriteAsync(json);
	}
}
