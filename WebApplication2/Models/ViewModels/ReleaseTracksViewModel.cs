using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.ViewModels
{
    public class ReleaseTracksViewModel
    {
        public int ReleaseID { get; set; }
        public string ReleaseTitle { get; set; }

        [Display(Name = "Search Tracks")]
        public string SearchTerm { get; set; }

        public List<ReleaseTracksTrackViewModel> AvailableTracks { get; set; } = new List<ReleaseTracksTrackViewModel>();
        public List<int> SelectedTrackIds { get; set; } = new List<int>();
        public List<ReleaseTracksTrackViewModel> CurrentTracks { get; set; } = new List<ReleaseTracksTrackViewModel>();
    }

    public class ReleaseTracksTrackViewModel
    {
        public int TrackID { get; set; }
        public string Title { get; set; }
        public string PerformerName { get; set; }
        public TimeSpan? Length { get; set; }
        public bool IsSelected { get; set; }
    }
} 