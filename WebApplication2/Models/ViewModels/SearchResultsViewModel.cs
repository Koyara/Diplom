using System.Collections.Generic;

namespace WebApplication2.Models.ViewModels
{
    public class SearchResultsViewModel
    {
        public List<ReleaseSearchResult> Releases { get; set; } = new List<ReleaseSearchResult>();
        public List<PerformerSearchResult> Artists { get; set; } = new List<PerformerSearchResult>();
        public List<TrackSearchResult> Tracks { get; set; } = new List<TrackSearchResult>();
    }

    public class ReleaseSearchResult
    {
        public int ReleaseID { get; set; }
        public string Title { get; set; }
        public string ReleaseType { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public string CoverImageUrl { get; set; }
    }

    public class PerformerSearchResult
    {
        public int PerformerID { get; set; }
        public string Name { get; set; }
        public string MainGenre { get; set; }
    }

    public class TrackSearchResult
    {
        public int TrackID { get; set; }
        public string Title { get; set; }
        public string ArtistName { get; set; }
    }
} 