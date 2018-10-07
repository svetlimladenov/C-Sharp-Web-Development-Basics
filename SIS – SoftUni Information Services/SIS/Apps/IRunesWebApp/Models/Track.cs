using System;
using System.Collections.Generic;

namespace IRunesWebApp.Models
{
    public class Track
    {

        public Track()
        {
            this.TrackAlbums = new HashSet<TrackAlbum>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        //link for the track video
        public string Link { get; set; }

        public decimal Price { get; set; }

        public ICollection<TrackAlbum> TrackAlbums { get; set; }

    }
}
