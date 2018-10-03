using System;
using SIS.Http.Enums;
using SIS.WebServer;
using SIS.WebServer.Routing;

namespace CakesWebApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Reoutes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index();
            serverRoutingTable.Reoutes[HttpRequestMethod.Get]["/home"] = request => new HomeController().Home();
            Server server = new Server(8000, serverRoutingTable);

            server.Run();
        }
    }
}
