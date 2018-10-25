using System.Collections.Generic;

namespace MishMashWebApp.Models
{
    public class Channel
    {
        public Channel()
        {
            this.ChannelTags = new HashSet<ChannelTag>();
            this.Followers = new HashSet<UserInChannel>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public ChannelType Type { get; set; }

        public virtual ICollection<ChannelTag> ChannelTags { get; set; }

        public virtual ICollection<UserInChannel> Followers { get; set; }
    }
}
