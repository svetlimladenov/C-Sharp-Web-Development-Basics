using SIS.Http.Requests.Contracts;
using SIS.Http.Responses.Contracts;

namespace SIS.WebServer.Routing
{
    using System;
    using System.Collections.Generic;
    using Http.Enums;
    using Http.Requests;
    using Http.Responses;

    public class ServerRoutingTable
    {
        public ServerRoutingTable()
        {
            this.Reoutes = new Dictionary<HttpRequestMethod, IDictionary<string, Func<IHttpRequest, IHttpResponse>>>
            {
                [HttpRequestMethod.Get] = new Dictionary<string, Func<IHttpRequest, IHttpResponse>>(),
                [HttpRequestMethod.Post] = new Dictionary<string, Func<IHttpRequest, IHttpResponse>>(),
                [HttpRequestMethod.Put] = new Dictionary<string, Func<IHttpRequest, IHttpResponse>>(),
                [HttpRequestMethod.Delete] = new Dictionary<string, Func<IHttpRequest, IHttpResponse>>()
            };
        }
        public IDictionary<HttpRequestMethod, IDictionary<string, Func<IHttpRequest, IHttpResponse>>> Reoutes { get; }
    }
}
