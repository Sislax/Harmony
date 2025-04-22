using System.Net;

namespace Harmony.Application.Common.Exceptions.GeneralExceptions;

public class NotFoundException : Exception, IServiceException
{
    public HttpStatusCode StatusCode => HttpStatusCode.NotFound;
    public string ErrorMessage => Message;

    public NotFoundException(string message) : base(message)
    {
    }
}
