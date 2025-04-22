using System.Net;

namespace Harmony.Application.Common.Exceptions.RoleExceptions;

public class DuplicateRoleException : Exception, IServiceException
{
    public HttpStatusCode StatusCode => HttpStatusCode.Conflict;

    public string ErrorMessage => Message;

    public DuplicateRoleException(string message) : base(message)
    {
    }
}
