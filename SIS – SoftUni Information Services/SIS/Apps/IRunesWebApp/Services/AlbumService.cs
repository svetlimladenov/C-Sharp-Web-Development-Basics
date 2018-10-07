using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using IRunesWebApp.Data;
using IRunesWebApp.Models;
using Microsoft.EntityFrameworkCore;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Results;

namespace IRunesWebApp.Services
{
    public class AlbumService
    {
        private readonly UserService userService;
        public AlbumService()
        {
            this.Context = new IRunesDbContext();
            this.BadRequestService = new BadRequestService();
            this.userService = new UserService();
        }

        protected IRunesDbContext Context { get; set; }
        protected BadRequestService BadRequestService { get; set; }



        public IHttpResponse CreateAlbum(IHttpRequest request)
        {
            var albumName = request.FormData["albumName"].ToString().Trim();
            var coverUrl = WebUtility.UrlDecode(request.FormData["coverUrl"].ToString().Trim());

            var album = new Album()
            {
                Name = albumName,
                Cover = coverUrl
            };

            var albumExists = this.Context.Albums.Any(x => x.Name == album.Name);
            if (!albumExists)
            {
               this.Context.Albums.Add(album);
            }
            

            
            var albumId = this.Context.Albums.Local.FirstOrDefault(x => x.Name == albumName)?.Id;
            var username = userService.GetUsername(request);
            var userId = this.Context.Users.FirstOrDefault(x => x.Username == username)?.Id;
            var userAlbum = new UserAlbum()
            {
                AlbumId = albumId,
                UserId = userId,
            };
           
            this.Context.UsersAlbums.Add(userAlbum);
            try
            {
                this.Context.SaveChanges();
            }
            catch (Exception e)
            {
                return this.BadRequestService.ServerError(e.Message);
            }

            return new RedirectResult("/Albums/All");

        }
    }
}
