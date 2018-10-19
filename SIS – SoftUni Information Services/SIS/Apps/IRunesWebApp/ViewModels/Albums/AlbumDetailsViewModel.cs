using System.Collections.Generic;
using System.Linq;
using IRunesWebApp.Models;

namespace IRunesWebApp.ViewModels.Albums
{
    public class AlbumDetailsViewModel
    {
        public string Id { get; set; }

        public Album Album { get; set; }

        public decimal AlbumPrice { get; set; }

        public IEnumerable<Track> Tracks { get; set; }

    }
}
