using System;
using System.Linq;
using System.Net;
using System.Text;
using IRunesWebApp.Models;
using SIS.Http.Enums;
using SIS.Http.Requests.Contracts;
using SIS.Http.Responses.Contracts;
using SIS.WebServer.Results;

namespace IRunesWebApp.Controller
{
    public class TrackController : BaseController
    {
        public IHttpResponse CreateTrack(IHttpRequest request)
        {
            var albumId = request.QueryData["albumId"].ToString();
            var response = this.View("CreateTrack");
            var view = Encoding.UTF8.GetString(response.Content);

            view = view.Replace("{albumId}",albumId);

            return new HtmlResult(view,HttpResponseStatusCode.OK);
        }

        public IHttpResponse AddTrack(IHttpRequest request)
        {
            var trackName = request.FormData["trackName"].ToString().Trim();
            var trackLink = request.FormData["trackLink"].ToString().Trim();
            var trackPrice = decimal.Parse(request.FormData["trackPrice"].ToString().Trim());
            var albumId = request.QueryData["albumId"].ToString();

            var track = new Track
            {
                Name = trackName,
                Link = trackLink,
                Price = trackPrice,           
            };

            bool trackExists = this.Db.Tracks.Any(x => x.Name == track.Name);

            string trackId; 
            if (!trackExists)
            {
                this.Db.Tracks.Add(track);   
                trackId = this.Db.Tracks.Local.First(x => x.Name == track.Name).Id;
            }
            else
            {
                trackId = this.Db.Tracks.First(x => x.Name == track.Name).Id;
            }

            var trackAlbum = new TrackAlbum()
            {
                AlbumId = this.Db.Albums.Find(albumId).Id,
                
                TrackId = trackId        
            };

            bool trackAlbumExist =
                this.Db.TracksAlbums.Any(x => x.AlbumId == trackAlbum.AlbumId && x.TrackId == trackAlbum.TrackId);

            if (!trackAlbumExist)
            {
                this.Db.Add(trackAlbum);
                this.Db.SaveChanges();
            }

            return new RedirectResult($"/Albums/Details?id={albumId}");
        }

        public IHttpResponse TrackInfo(IHttpRequest request)
        {
            var response = this.View("TrackInfo");
            var view = Encoding.UTF8.GetString(response.Content);
            var albumId = request.QueryData["albumId"].ToString();
            var trackId = request.QueryData["trackId"].ToString();
            var track = this.Db.Tracks.FirstOrDefault(x => x.Id == trackId);
            var album = this.Db.Albums.FirstOrDefault(x => x.Id == albumId);

            var youtubeLink = WebUtility.UrlDecode(track.Link);
            youtubeLink = youtubeLink.Replace(".com/", ".com/embed/");
            var trackName = track.Name;
            var price = track.Price;

            view = view.Replace("{trackUrl}", youtubeLink);
            view = view.Replace("{trackUrlName}", trackName);
            view = view.Replace("{name}", trackName);
            view = view.Replace("{price}", price.ToString());
            view = view.Replace("{albumid}", albumId);
            return new HtmlResult(view,HttpResponseStatusCode.OK);
        }
    }
}
