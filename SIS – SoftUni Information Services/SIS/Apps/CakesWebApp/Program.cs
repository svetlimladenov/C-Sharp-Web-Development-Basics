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
            serverRoutingTable.Reoutes[HttpRequestMethod.Post]["/login"] = request => new AccountController().DoLogin(request);
            serverRoutingTable.Reoutes[HttpRequestMethod.Get]["/logout"] = request => new AccountController().Logout(request);
            serverRoutingTable.Reoutes[HttpRequestMethod.Get]["/profile"] = request => new AccountController().Profile(request);

            serverRoutingTable.Reoutes[HttpRequestMethod.Get]["/register"] = request => new AccountController().Register(request);
            serverRoutingTable.Reoutes[HttpRequestMethod.Post]["/register"] =  request => new AccountController().DoRegister(request);

            serverRoutingTable.Reoutes[HttpRequestMethod.Get]["/hello"] = request => new HomeController().HelloUser(request);

            serverRoutingTable.Reoutes[HttpRequestMethod.Get]["/add"] = request => new CakeController().CakePage(request);
            serverRoutingTable.Reoutes[HttpRequestMethod.Post]["/add"] = request => new CakeController().AddCake(request);


            Server server = new Server(80, serverRoutingTable);

            server.Run();
        }
    }
}
