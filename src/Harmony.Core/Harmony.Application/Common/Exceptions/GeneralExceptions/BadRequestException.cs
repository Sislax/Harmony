using System.Net;

namespace Harmony.Application.Common.Exceptions.GeneralExceptions;

public class BadRequestException : Exception, IServiceException
{
    public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    public string ErrorMessage => Message;

    public BadRequestException(string message) : base(message)
    {
    }
}
