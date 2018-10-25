using System.Collections.Generic;
using MishMashWebApp.Models;

namespace MishMashWebApp.ViewModels.Channels
{
    public class FollowedChannelsViewModel
    {
        public IEnumerable<BaseChannelViewModel> FollowedChannels { get; set; }

    }
}
