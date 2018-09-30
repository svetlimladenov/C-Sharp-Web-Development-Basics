namespace WebServer.Server.Routing
{
    using Common;
    using System.Collections.Generic;
    using Handlers;
    using Contracts;

    public class RoutingContext : IRoutingContext
    {
        public RoutingContext(RequestHandler handler, IEnumerable<string> parameters)
        {
            CoreValidator.ThrowIfNull(handler,nameof(handler));
            CoreValidator.ThrowIfNull(parameters,nameof(parameters));
            this.Handler = handler;
            this.Parameters = parameters;
        }
        public IEnumerable<string> Parameters { get; private set; }
        public RequestHandler Handler { get; private set; }
    }
}
