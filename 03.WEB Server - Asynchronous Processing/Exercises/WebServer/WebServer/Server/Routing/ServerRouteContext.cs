using System;
using System.Collections.Generic;
using System.Linq;
using WebServer.Server.Enums;
using WebServer.Server.Routing.Contracts;

namespace WebServer.Server.Routing
{
    public class ServerRouteContext : IServerRouteContext
    {
        private readonly Dictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>> routes;
        

        public ServerRouteContext(IAppRouteConfig appRouteConfig)
        {
            this.routes = new Dictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>>();

            var avaivableMethods = Enum
                .GetValues(typeof(HttpRequestMethod))
                .Cast<HttpRequestMethod>();
            foreach (var method in avaivableMethods)
            {
                this.routes[method] = new Dictionary<string, IRoutingContext>();
            }

            this.InitializeRouteConfig(appRouteConfig);
        }

 
        public IDictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>> Routes => this.routes;

        private void InitializeRouteConfig(IAppRouteConfig appRouteConfig)
        {
            throw new NotImplementedException();
        }

    }
}
