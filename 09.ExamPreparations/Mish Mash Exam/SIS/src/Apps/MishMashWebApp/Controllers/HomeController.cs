using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MishMashWebApp.Models;
using MishMashWebApp.ViewModels.Home;
using SIS.HTTP.Enums;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using SIS.WebServer.Results;

namespace MishMashWebApp.Controllers
{
    public class HomeController : BaseController
    {
        [HttpGet("/Home/Index")]
        public IHttpResponse Index()
        {
            if (this.User == null)
            {
                return this.View("Home/Index");
            }

            var user = this.Db.Users.FirstOrDefault(x => x.Username == this.User);
            var allChannels = this.Db.Channels.ToArray();

            var followedChannels = this.Db.UserInChannel.Where(x => x.UserId == user.Id).Select(x => x.Channel).Include(c => c.Followers).ToList();
            var otherChannels = this.Db.Channels.Where(x => x.Followers.Select(ch => ch.UserId).FirstOrDefault() != user.Id).ToArray();


            var followedChannelsTags = this.Db.Channels.Where(
                    x => x.Followers.Any(f => f.User.Username == this.User))
                .SelectMany(x => x.ChannelTags.Select(t => t.TagId)).ToList();

            var suggestedChannels = this.Db.Channels.Where(
                x => !x.Followers.Any(f => f.User.Username == this.User) &&
                     x.ChannelTags.Any(t => followedChannelsTags.Contains(t.TagId)));


            //TODO: Display corrent followers count;
            var viewModel = new LoggedInIndexViewModel()
            {
                UserRole = user.Role.ToString(),
                FollowedChannels = followedChannels,
                SuggestedChannels = suggestedChannels,
                OtherChannels = otherChannels,
            };
            if (user.Role == Role.Admin)
            {

                return this.View("Home/LoggedInIndex", viewModel ,"_Admin_Layout");
            }
            else
            {
                return this.View("Home/LoggedInIndex", viewModel);
            }

            
        }


        [HttpGet("/")]
        public IHttpResponse RootIndex()
        {
            return this.Index();
        }
    }
}
