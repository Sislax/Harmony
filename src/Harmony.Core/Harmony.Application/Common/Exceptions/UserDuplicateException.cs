using System.Net;

namespace Harmony.Application.Common.Exceptions;

public class UserDuplicateException : Exception, IServiceException
{
    public HttpStatusCode StatusCode => HttpStatusCode.Conflict;

    public string ErrorMessage => Message;

    public UserDuplicateException(string message) : base(message)
    {
    }
}
