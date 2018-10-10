namespace SIS.Demo
{
    using Http.Enums;
    using WebServer;
    using WebServer.Routing;
    public class Launcher
    {
        public static void Main(string[] args)
        {
            var serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Routes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index();
            serverRoutingTable.Routes[HttpRequestMethod.Get]["/home"] = request => new HomeController().Home();
            Server server = new Server(8000, serverRoutingTable);

            server.Run();
        }
    }
}
