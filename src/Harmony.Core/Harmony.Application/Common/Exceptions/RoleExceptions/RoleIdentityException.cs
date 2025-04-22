using System.Net;

namespace Harmony.Application.Common.Exceptions.RoleExceptions;

public class RoleIdentityException : Exception, IServiceException
{
    public HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;

    public string ErrorMessage => Message;

    public RoleIdentityException(string message) : base(message)
    {
    }
}
