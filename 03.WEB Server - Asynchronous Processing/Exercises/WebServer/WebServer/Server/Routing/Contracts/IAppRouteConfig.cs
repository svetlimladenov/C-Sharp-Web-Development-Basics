namespace WebServer.Server.Routing.Contracts
{
    using Enums;
    using Handlers;
    using System.Collections.Generic;
    //GET /home - GetHandler
    //Get /about
    //Post /home - PostHandler
    //..
    public interface IAppRouteConfig
    {
        IReadOnlyDictionary<HttpRequestMethod, IDictionary<string, RequestHandler>> Routes { get; }

        void AddRoute(string route,RequestHandler handler);
    }
}
