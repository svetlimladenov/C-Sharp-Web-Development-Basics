using System.Collections.Generic;
using SIS.Http.Enums;
using SIS.Http.Headers.Contracts;

namespace SIS.Http.Requests.Contracts
{
    public interface IHttpRequest
    {
        string Path { get; }

        string Url { get; }

        Dictionary<string,object> FormData { get; }

        Dictionary<string, object> QueryData { get; }

        IHttpHeaderCollection Headers { get; }

        HttpRequestMethod RequestMethod { get; }


    }
}
