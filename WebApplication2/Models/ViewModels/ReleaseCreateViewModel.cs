using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApplication2.Models.ViewModels
{
    public class ReleaseCreateViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public DateOnly ReleaseDate { get; set; }

        public int? MainGenreCode { get; set; }

        public string? Description { get; set; }

        public int? SecondGenreCode { get; set; }
        public int? ReleaseTypeID { get; set; }
        public byte[]? ReleaseCover { get; set; }

        // Track management
        public List<ReleaseCreateTrackViewModel> AvailableTracks { get; set; } = new List<ReleaseCreateTrackViewModel>();
        //public List<int> SelectedTrackIds { get; set; } = new List<int>();
        public string SelectedTrackIds { get; set; } = string.Empty;
        public List<ReleaseCreateTrackViewModel> CurrentTracks { get; set; } = new List<ReleaseCreateTrackViewModel>();
        public string SearchTerm { get; set; } = string.Empty;

        // Performer management
        public List<PerformerViewModel> AvailablePerformers { get; set; } = new List<PerformerViewModel>();
        public List<PerformerViewModel> CurrentPerformers { get; set; } = new List<PerformerViewModel>();
        public string SelectedPerformerIds { get; set; } = string.Empty;
    }

    public class ReleaseCreateTrackViewModel
    {
        public int TrackID { get; set; }
        public string Title { get; set; }
        public string PerformerName { get; set; }
        public TimeSpan? Length { get; set; }
        public bool IsSelected { get; set; }
    }
} 