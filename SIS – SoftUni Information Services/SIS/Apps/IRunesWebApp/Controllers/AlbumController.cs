using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using IRunesWebApp.GlobalConst;
using IRunesWebApp.Models;
using IRunesWebApp.ViewModels.Albums;
using Microsoft.EntityFrameworkCore;
using SIS.Http.Enums;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses.Contracts;
using SIS.MvcFramework;
using SIS.WebServer.Results;

namespace IRunesWebApp.Controller
{
    public class AlbumController : BaseController
    {
        [HttpGet("/Albums/All")]
        public IHttpResponse AllAlbums()
        {
            if (this.Request.Cookies.GetCookie(GlobalConstants.userCookieAuthentication) == null)
            {
                return Redirect("/");                
            }

            var currentUser = this.User;                
            var userId = this.Db.Users.FirstOrDefault(x => x.Username == currentUser)?.Id;

            var query = from album in this.Db.Albums
                        where album.AlbumUsers.Any(x => x.UserId == userId)
                        select album;

            var sb = new StringBuilder();
            foreach (var album in query)
            {
                sb.AppendLine($"<h5><a href=\"/Albums/Details?id={album.Id}\">{album.Name}</a></h5></br>");
            }

            var viewBag = new Dictionary<string, string>
            {
                {"Albums", sb.ToString()},
            };
            return this.View("AllAlbums", viewBag);
        }

        [HttpGet("/Albums/Create")]
        public IHttpResponse CreateAlbum()
        {
            if (this.Request.Cookies.GetCookie(GlobalConstants.userCookieAuthentication) == null)
            {
                return Redirect("/");

            }
            return this.View("CreateAlbum");
        }

        [HttpPost("/Albums/Create")]
        public IHttpResponse AddNewAlbum(AddNewAlbumInputModel model)
        {


            var album = new Album()
            {
                Name = model.AlbumName,
                Cover = model.CoverUrl
            };

            if (string.IsNullOrWhiteSpace(model.AlbumName) || string.IsNullOrWhiteSpace(model.AlbumName))
            {
                return this.BadRequestError("Fill in all the blanks.");
            }

            var albumExists = this.Db.Albums.Any(x => x.Name == album.Name);
            var albumId = "";
            if (!albumExists)
            {
                this.Db.Albums.Add(album);
                albumId = this.Db.Albums.Local.FirstOrDefault(x => x.Name == model.AlbumName)?.Id;
            }
            else
            {
                albumId = this.Db.Albums.FirstOrDefault(x => x.Name == model.AlbumName)?.Id;
            }




            var username = this.User;
            var userId = this.Db.Users.FirstOrDefault(x => x.Username == username)?.Id;
            var userAlbum = new UserAlbum()
            {
                AlbumId = albumId,
                UserId = userId,
            };

            this.Db.UsersAlbums.Add(userAlbum);
            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                return this.BadRequestError(e.Message);
            }

            return Redirect("/Albums/All");
        }

        [HttpGet("/Albums/Details")]
        public IHttpResponse AlbumDetails(AlbumsDetailsViewModel model)
        {
            //var id = this.Request.QueryData["id"].ToString();
            var id = model.Id;
            var album = this.Db.Albums.FirstOrDefault(x => x.Id == id);
            if (album == null)
            {
                return BadRequestError("Invalid Album.");
            }
            var albumId = album.Id;
            var query = from track in this.Db.Tracks
                        where track.TrackAlbums.Any(x => x.AlbumId == albumId)
                        select track;

            var tracks = new StringBuilder();
            var counter = 0;
            decimal allTracksSum = 0.0m;
            foreach (var track in query)
            {
                counter++;
                allTracksSum += track.Price;
                tracks.AppendLine($"<a href=\"/Albums/Tracks/Details?albumId={albumId}&trackId={track.Id}\">{counter}. {track.Name}</a></br>");
            }

            allTracksSum = Math.Round(allTracksSum - (allTracksSum * 0.13m),2);

            var viewBag = new Dictionary<string, string>
            {
                {"AlbumCoverUrl", album.Cover},
                {"AlbumName", album.Name},
                {"AlbumId", album.Id},
                {"AlbumPrice", allTracksSum.ToString(CultureInfo.InvariantCulture)},
                {"Tracks", tracks.ToString()}
            };


            return this.View("AlbumInfo", viewBag);
        }
    }
}
