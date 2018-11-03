using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MishMashWebApp.Models;
using MishMashWebApp.ViewModels.Channels;
using SIS.HTTP.Responses;
using SIS.MvcFramework;

namespace MishMashWebApp.Controllers
{
    public class ChannelsController : BaseController
    {
        [HttpGet("/Channels/Create")]
        public IHttpResponse Create()
        {
            var user = this.Db.Users.FirstOrDefault(x => x.Username == this.User);
            if (user == null || user.Role != Role.Admin)
            {
                return Redirect("/");
            }
            return this.View("Channels/Create", "_Admin_Layout");
        }

        [HttpPost("/Channels/Create")]
        public IHttpResponse CreateChannel(CreateChannelsInputModel model)
        {
            var user = this.Db.Users.FirstOrDefault(x => x.Username == this.User);
            if (user == null || user.Role != Role.Admin)
            {
                return Redirect("/");
            }

            if (!Enum.TryParse(model.Type, true, out ChannelType type))
            {
                return this.BadRequestError("Invalid channel type");
            }

            if (string.IsNullOrWhiteSpace(model.Name) || string.IsNullOrWhiteSpace(model.Description))
            {
                return this.BadRequestError("Please provide a valid Channel Name and Description");
            }


            var channel = new Channel()
            {
                Name = model.Name,
                Description = model.Description,
                Type = type,
            };

            if (!string.IsNullOrWhiteSpace(model.Tags))
            {
                var tags = model.Tags.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var tag in tags)
                {
                    var dbTag = this.Db.Tags.FirstOrDefault(x => x.Name == tag.Trim());
                    if (dbTag == null)
                    {
                        dbTag = new Tag() { Name = tag.Trim() };
                        this.Db.Tags.Add(dbTag);
                        this.Db.SaveChanges();
                    }

                    channel.ChannelTags.Add(new ChannelTag()
                    {
                        TagId = dbTag.Id
                    });
                }

            }


            this.Db.Channels.Add(channel);
            this.Db.SaveChanges();

            return this.Redirect("/Channels/Details?id=" + channel.Id);
        }

        [HttpGet("/Channels/Details")]
        public IHttpResponse Details(int id)
        {
            var user = this.Db.Users.FirstOrDefault(x => x.Username == this.User);
            if (user == null)
            {
                return this.Redirect("/");
            }

            var channel = this.Db.Channels.Where(x => x.Id == id)
                .Include(x => x.Followers)
                .Include(x => x.ChannelTags)
                .ThenInclude(x => x.Tag)
                .FirstOrDefault();
            if (channel == null)
            {
                return BadRequestError("Invalid Channel.");
            }


            var viewModel = new DetailsViewModel()
            {
                ChannelName = channel.Name,
                ChannelDescription = channel.Description,
                ChannelType = channel.Type.ToString(),
                FollowersCount = channel.Followers.Count,
                Tags = channel.ChannelTags,
            };

            if (user.Role == Role.Admin)
            {
                return this.View("Channels/Details", viewModel ,"_Admin_Layout");
            }
            return this.View("Channels/Details", viewModel);
        }

        [HttpGet("/Channels/Follow")]
        public IHttpResponse Follow(int id)
        {
            var user = this.Db.Users.FirstOrDefault(x => x.Username == this.User);
            if (user == null)
            {
                 return this.Redirect("/Users/Login");
            }

            if (!this.Db.UserInChannel.Any(x => x.UserId == user.Id && x.ChannelId == id))
            {
                this.Db.UserInChannel.Add(new UserInChannel()
                {
                    UserId = user.Id,
                    ChannelId = id,
                });

                this.Db.SaveChanges();
            }


            return this.Redirect("/Channels/Followed");
        }

        [HttpGet("/Channels/Followed")]
        public IHttpResponse Followed()
        {
            var user = this.Db.Users.FirstOrDefault(x => x.Username == this.User);
            if (user == null)
            {
                return this.Redirect("/Users/Login");
            }

            var followedChannels = this.Db.Channels
                .Where(x => x.Followers.Any(f => f.UserId == user.Id))
                .Select(x => new BaseChannelViewModel()
                {
                    Name = x.Name,
                    Type = x.Type,
                    FollowersCount = x.Followers.Count,
                    Id = x.Id
                }).ToList();
            
            var viewModel = new FollowedChannelsViewModel {FollowedChannels = followedChannels}; 

            if (user.Role == Role.Admin)
            {
                return this.View("Channels/Followed", viewModel,"_Admin_Layout");
            }

            return this.View("/Channels/Followed", viewModel);
        }


        [HttpGet("/Channels/Unfollow")]
        public IHttpResponse Unfollow(int id)
        {
            if (this.User == null)
            {
                return this.Redirect("/Users/Login");
            }

            var userInChannel = this.Db.UserInChannel.FirstOrDefault(x => x.User.Username == this.User && x.ChannelId == id);
            if (userInChannel != null)
            {
                this.Db.UserInChannel.Remove(userInChannel);
                this.Db.SaveChanges();
            }

            return this.Redirect("/Channels/Followed");
        }
    }
}
