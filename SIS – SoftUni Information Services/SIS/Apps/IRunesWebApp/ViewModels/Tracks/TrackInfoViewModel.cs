using IRunesWebApp.Models;

namespace IRunesWebApp.ViewModels.Tracks
{
    public class TrackInfoViewModel
    {
        public string AlbumId { get; set; }

        public string TrackId { get; set; }

        public string TrackUrl { get; set; }

        public string TrackName { get; set; }

        public decimal TrackPrice { get; set; }
    }
}
