using System.Collections.Generic;
using MishMashWebApp.Models;

namespace MishMashWebApp.ViewModels.Home
{
    public class LoggedInIndexViewModel
    {
        public string UserRole { get; set; }

        public IEnumerable<Channel> FollowedChannels { get; set; }

        public IEnumerable<Channel> SuggestedChannels { get; set; }

        public IEnumerable<Channel> OtherChannels { get; set; }

    }
}
