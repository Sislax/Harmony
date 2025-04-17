using System.Net;

namespace Harmony.Application.Common.Exceptions;

public interface IServiceException
{
    public HttpStatusCode StatusCode { get;}
    public string ErrorMessage { get;}
}
