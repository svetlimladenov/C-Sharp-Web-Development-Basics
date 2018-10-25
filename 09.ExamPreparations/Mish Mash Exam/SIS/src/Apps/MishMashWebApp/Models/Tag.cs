using System.Collections.Generic;

namespace MishMashWebApp.Models
{
    public class Tag
    {
        public Tag()
        {
            this.TagChannels = new HashSet<ChannelTag>();
        }
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<ChannelTag> TagChannels { get; set; }
    }
}