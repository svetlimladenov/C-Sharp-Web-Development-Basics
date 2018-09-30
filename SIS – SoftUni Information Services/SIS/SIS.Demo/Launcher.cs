namespace SIS.Demo
{
    using SIS.Http.Enums;
    using SIS.WebServer;
    using SIS.WebServer.Routing;
    public class Launcher
    {
        public static void Main(string[] args)
        {
            var serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Reoutes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index();

            Server server = new Server(8000, serverRoutingTable);

            server.Run();
        }
    }
}
