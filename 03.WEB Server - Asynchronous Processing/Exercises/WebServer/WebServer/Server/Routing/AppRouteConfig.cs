using System;
using System.Linq;

namespace WebServer.Server.Routing
{
    using System.Collections.Generic;
    using Enums;
    using Handlers;
    using Contracts;
    public class AppRouteConfig : IAppRouteConfig
    {
        private Dictionary<HttpRequestMethod, IDictionary<string, RequestHandler>> routes;

        public AppRouteConfig()
        {
            this.routes = new Dictionary<HttpRequestMethod, IDictionary<string, RequestHandler>>();
            var avaivableMethod = Enum
                .GetValues(typeof(HttpRequestMethod))
                .Cast<HttpRequestMethod>();

            foreach (var method in avaivableMethod)
            {
                this.routes[method] = new Dictionary<string, RequestHandler>();
            }
        }

        public IReadOnlyDictionary<HttpRequestMethod, IDictionary<string, RequestHandler>> Routes => this.routes;


        //.AddRoute("/home, new GetHandler(...)")
        public void AddRoute(string route,RequestHandler handler)
        {
            var hanlerName = handler.GetType().Name.ToLower();
            if (hanlerName.Contains("get"))
            {
                this.routes[HttpRequestMethod.Get].Add(route, handler);
            }
            else if (hanlerName.Contains("post"))
            {
                this.routes[HttpRequestMethod.Post].Add(route, handler);

            }
            else
            {
                throw new InvalidOperationException("Invalid handle.");
            }
        }

    }
}
