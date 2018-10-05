using System;

namespace IRunesWebApp.Models
{
    public class TrackAlbum : BaseModel<int>
    {
        public Guid TrackId { get; set; }
        public virtual Track Track { get; set; }

        public Guid AlbumId { get; set; }
        public virtual Album Album { get; set; }
    }
}
