using System.Net;

namespace NoteApp.Api.Exceptions;
public class DuplicateEmailException(string email) : Exception($"User with the email {email} is already exist")
{
}
