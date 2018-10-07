using IRunesWebApp.Controller;
using SIS.Http.Enums;
using SIS.WebServer;
using SIS.WebServer.Results;
using SIS.WebServer.Routing;

namespace IRunesWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var serverRoutingTable = new ServerRoutingTable();

            serverRoutingTable.Reoutes[HttpRequestMethod.Get]["/"] = request => new HomeController().Index(request);
            serverRoutingTable.Reoutes[HttpRequestMethod.Get]["/Home/Index"] = request => new RedirectResult("/");

            serverRoutingTable.Reoutes[HttpRequestMethod.Get]["/Users/Login"] = request => new AccountController().Login(request);
            serverRoutingTable.Reoutes[HttpRequestMethod.Post]["/Users/Login"] = request => new AccountController().DoLogin(request);
            serverRoutingTable.Reoutes[HttpRequestMethod.Get]["/Users/Logout"] = request => new AccountController().Logout(request);



            serverRoutingTable.Reoutes[HttpRequestMethod.Get]["/Users/Register"] = request => new AccountController().Register(request);
            serverRoutingTable.Reoutes[HttpRequestMethod.Post]["/Users/Register"] = request => new AccountController().DoRegister(request);


            serverRoutingTable.Reoutes[HttpRequestMethod.Get]["/Albums/All"] = request => new AlbumController().AllAlbums(request);
            serverRoutingTable.Reoutes[HttpRequestMethod.Get]["/Albums/Create"] = request => new AlbumController().CreateAlbum(request);
            serverRoutingTable.Reoutes[HttpRequestMethod.Post]["/Albums/Create"] = request => new AlbumController().AddNewAlbum(request);
            serverRoutingTable.Reoutes[HttpRequestMethod.Get]["/Albums/Details"] = request => new AlbumController().AlbumDetails(request);

            serverRoutingTable.Reoutes[HttpRequestMethod.Get]["/Albums/Tracks/Create"] = request => new TrackController().CreateTrack(request);
            serverRoutingTable.Reoutes[HttpRequestMethod.Post]["/Albums/Tracks/Create"] = request => new TrackController().AddTrack(request);
            serverRoutingTable.Reoutes[HttpRequestMethod.Get]["/Albums/Tracks/Details"] = request => new TrackController().TrackInfo(request);



            Server server = new Server(80, serverRoutingTable);

            server.Run();
        }
    }
}
