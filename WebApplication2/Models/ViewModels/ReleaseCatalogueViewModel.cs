namespace WebApplication2.Models.ViewModels
{
    public class ReleaseCatalogueViewModel
    {
        public int ReleaseID { get; set; }
        public string Title { get; set; }
        public DateOnly ReleaseDate { get; set; }
        public string? MainGenre { get; set; }
        public string? ReleaseType { get; set; }
        public string? CoverImageUrl { get; set; }
        public string? ArtistNames { get; set; } // Will be null in this version
    }
}