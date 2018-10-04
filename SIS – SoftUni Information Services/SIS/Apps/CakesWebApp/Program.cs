namespace CakesWebApp
{
    using SIS.Http.Enums;
    using SIS.WebServer;
    using SIS.WebServer.Routing;
    using Controllers;
    class Program
    {
        static void Main(string[] args)
        {
            var serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Reoutes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index(request);
            serverRoutingTable.Reoutes[HttpRequestMethod.Get]["/login"] = request => new AccountController().Login(request);
            serverRoutingTable.Reoutes[HttpRequestMethod.Get]["/register"] = request => new AccountController().Register(request);
            serverRoutingTable.Reoutes[HttpRequestMethod.Post]["/register"] =  request => new AccountController().DoRegister(request);
            Server server = new Server(80, serverRoutingTable);

            server.Run();
        }
    }
}
