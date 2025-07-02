using Refit;
using System.Net;

namespace Confluence.Server.ApiClient.Net;

public class ConfluenceApiClientResult<T>
{
    public HttpStatusCode HttpStatusCode { get; set; }
    public ApiException? Error { get; set; } = null;
    public T? Result { get; set; }

    public ConfluenceApiClientResult()
    { }

    public ConfluenceApiClientResult(HttpStatusCode httpStatusCode, ApiException error, T? result)
    {
        HttpStatusCode = httpStatusCode;
        Error = error;
        Result = result;
    }
}