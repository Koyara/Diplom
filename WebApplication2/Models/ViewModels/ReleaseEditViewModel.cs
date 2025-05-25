using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApplication2.Models.ViewModels
{
    public class ReleaseEditViewModel
    {
        public int ReleaseID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateOnly ReleaseDate { get; set; }

        public int? MainGenreCode { get; set; }

        public string? Description { get; set; }

        public int? SecondGenreCode { get; set; }
        public int? ReleaseTypeID { get; set; }
        public byte[]? ReleaseCover { get; set; }

        //
        public string? ExistingCoverBase64 { get; set; }

        // Track management
        public List<ReleaseCreateTrackViewModel> AvailableTracks { get; set; } = new List<ReleaseCreateTrackViewModel>();
        public string SelectedTrackIds { get; set; } = string.Empty;

        public List<int> TrackIds { get; set; } = new();
        public List<int> TrackNumbers { get; set; } = new();
        public List<ReleaseCreateTrackViewModel> CurrentTracks { get; set; } = new List<ReleaseCreateTrackViewModel>();
        public string SearchTerm { get; set; } = string.Empty;

        // Performer management
        public List<PerformerViewModel> AvailablePerformers { get; set; } = new List<PerformerViewModel>();
        public List<PerformerViewModel> CurrentPerformers { get; set; } = new List<PerformerViewModel>();
        public string SelectedPerformerIds { get; set; } = string.Empty;
    }
} 