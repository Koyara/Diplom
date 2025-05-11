using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Track
    {
        [Key]
        public int TrackID { get; set; }

        [Required]
        public string Title { get; set; }

        public TimeSpan? Length { get; set; }
        public bool? IsSong { get; set; }
        public string? Lyrics { get; set; }

        [ForeignKey("MainGuest")]
        public int? GuestID { get; set; }
        public virtual Performer? MainGuest { get; set; }

        [ForeignKey("SecondGuest")]
        public int? SecondGuestID { get; set; }
        public virtual Performer? SecondGuest { get; set; }

        [ForeignKey("Language")]
        public string? LanguageCode { get; set; }
        public virtual Language? Language { get; set; }

        [ForeignKey("Scale")]
        public int? ScaleID { get; set; }
        public virtual Scale? Scale { get; set; }

        public int? BPM{ get; set; }

        public virtual ICollection<ReleaseTrack> ReleaseTracks { get; set; } = new List<ReleaseTrack>();
        public virtual ICollection<TrackPerformer> TrackPerformers { get; set; } = new List<TrackPerformer>();
        public virtual ICollection<TrackProducer> TrackProducers { get; set; } = new List<TrackProducer>();
    }
}
