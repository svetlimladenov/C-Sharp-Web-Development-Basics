using System;
using System.Linq;
using IRunesWebApp.GlobalConst;
using IRunesWebApp.Models;
using IRunesWebApp.ViewModels.Albums;
using SIS.Http.Responses.Contracts;
using SIS.MvcFramework;

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

            var albums = from album in this.Db.Albums
                         where album.AlbumUsers.Any(x => x.UserId == userId)
                         select album;



            var viewModel = new AllAlbumsViewModel()
            {
                Albums = albums.ToArray(),
            };

            return this.View("AllAlbums", viewModel);
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
        public IHttpResponse AlbumDetails(AlbumDetailsViewModel model)
        {
            var id = model.Id;
            var album = this.Db.Albums.FirstOrDefault(x => x.Id == id);
            if (album == null)
            {
                return BadRequestError("Invalid Album.");
            }
            var albumId = album.Id;
            var tracks = from track in this.Db.Tracks
                         where track.TrackAlbums.Any(x => x.AlbumId == albumId)
                         select track;

            var viewModel = new AlbumDetailsViewModel()
            {
                Album = album,
                AlbumPrice = Math.Round(tracks.ToArray().Sum(x => x.Price) * 0.87m,2),
                Id = albumId,
                Tracks = tracks.ToArray(),
            };

            return this.View("AlbumInfo", viewModel);
        }
    }
}
