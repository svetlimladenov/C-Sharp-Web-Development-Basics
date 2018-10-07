using System;
using System.Linq;
using System.Text;
using IRunesWebApp.Models;
using IRunesWebApp.Services;
using Microsoft.EntityFrameworkCore;
using SIS.Http.Enums;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Results;

namespace IRunesWebApp.Controller
{
    public class AlbumController : BaseController
    {
        private AlbumService albumService;
        private UserService userService;

        public AlbumController()
        {
            this.albumService = new AlbumService();
            this.userService = new UserService();
        }
        public IHttpResponse AllAlbums(IHttpRequest request)
        {
            if (request.Cookies.GetCookie(this.userCookieAuth) == null)
            {
                return new RedirectResult("/");
            }

            var currentUser = userService.GetUsername(request);
            var userId = this.Db.Users.FirstOrDefault(x => x.Username == currentUser).Id;


            var query = from album in this.Db.Albums
                        where album.AlbumUsers.Any(x => x.UserId == userId)
                        select album;


            var sb = new StringBuilder();
            foreach (var album in query)
            {
                sb.AppendLine($"<a href=\"/Albums/Details?id={album.Id}\">{album.Name}</a></br>");
            }

            var response = this.View("AllAlbums");
            var view = Encoding.UTF8.GetString(response.Content);
            view = view.Replace("{allAlbums}", sb.ToString());
            return new HtmlResult(view, HttpResponseStatusCode.OK);

        }

        public IHttpResponse CreateAlbum(IHttpRequest request)
        {
            if (request.Cookies.GetCookie(this.userCookieAuth) == null)
            {
                return new RedirectResult("/");
            }

            return this.View("CreateAlbum");
        }

        public IHttpResponse AddNewAlbum(IHttpRequest request)
        {
            var response = this.albumService.CreateAlbum(request);
            return response;
        }

        public IHttpResponse AlbumDetails(IHttpRequest request)
        {
            var id = request.QueryData["id"].ToString();
            var album = this.Db.Albums.FirstOrDefault(x => x.Id == id);

            var response = this.View("AlbumInfo");
            var view = Encoding.UTF8.GetString(response.Content);
            view = view.Replace("{albumCoverUrl}", album.Cover);
            view = view.Replace("{albumName}", album.Name);
            view = view.Replace("{albumPrice}", album.Price.ToString());
            view = view.Replace("{albumId}", album.Id.ToString());

            var albumId = album.Id;
            var query = from track in this.Db.Tracks
                        where track.TrackAlbums.Any(x => x.AlbumId == albumId)
                        select track;


            var sb = new StringBuilder();
            var counter = 0;
            foreach (var track in query)
            {
                counter++;

                sb.AppendLine($"<a href=\"/Albums/Tracks/Details?albumId={albumId}&trackId={track.Id}\">{counter}. {track.Name}</a></br>");
            }

            view = view.Replace("{tracks}", sb.ToString());
            return new HtmlResult(view, HttpResponseStatusCode.OK);
        }
    }
}
