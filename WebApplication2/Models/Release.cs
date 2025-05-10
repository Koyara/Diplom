using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Release
    {
        [Key]
        public int ReleaseID { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public DateOnly ReleaseDate { get; set; }

        public string? Description { get; set; }


        [ForeignKey("Genre")]
        public int? MainGenreCode { get; set; }
        public virtual Genre? MainGenre { get; set; }

        [ForeignKey("Genre")]
        public int? SecondGenreCode { get; set; }
        public virtual Genre? SecondGenre { get; set; }

        public byte[]? ReleaseCover { get; set; }

        [ForeignKey("ReleaseType")]
        public int? ReleaseTypeID { get; set; }
        public virtual ReleaseType? ReleaseType { get; set; }

        // Navigation properties
        public virtual ICollection<ReleasePerformer> ReleasePerformers { get; set; } = new List<ReleasePerformer>();
        public virtual ICollection<ReleaseTrack> ReleaseTracks { get; set; } = new List<ReleaseTrack>();

        // Helper property to get all performers
        [NotMapped]
        public IEnumerable<Performer> Performers => ReleasePerformers.Select(rp => rp.Performer);

        // Helper property to get all tracks
        [NotMapped]
        public IEnumerable<Track> Tracks => ReleaseTracks.Select(rt => rt.Track);
    }
}
