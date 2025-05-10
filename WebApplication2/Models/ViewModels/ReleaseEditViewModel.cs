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

        // Track management
        public List<ReleaseCreateTrackViewModel> AvailableTracks { get; set; } = new List<ReleaseCreateTrackViewModel>();
        public string SelectedTrackIds { get; set; } = string.Empty;
        public List<ReleaseCreateTrackViewModel> CurrentTracks { get; set; } = new List<ReleaseCreateTrackViewModel>();
        public string SearchTerm { get; set; } = string.Empty;
    }
} 