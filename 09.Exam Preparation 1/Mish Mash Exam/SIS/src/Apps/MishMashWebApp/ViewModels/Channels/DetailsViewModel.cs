using System.Collections.Generic;
using System.Linq;
using MishMashWebApp.Models;

namespace MishMashWebApp.ViewModels.Channels
{
    public class DetailsViewModel
    {
        public string ChannelName { get; set; }

        public string ChannelType { get; set; }

        public int FollowersCount { get; set; }

        public string ChannelDescription { get; set; }

        public ICollection<ChannelTag> Tags { get; set; }

        public string TagsAsString => string.Join(", ", this.Tags.Select(x => x.Tag.Name));
    }
}
