using System.Net;

namespace Harmony.Application.Common.Exceptions.UserExceptions;

public class UserIdentityException : Exception, IServiceException
{
    public HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;

    public string ErrorMessage => Message;

    public UserIdentityException(string message) : base(message)
    {
    }
}
