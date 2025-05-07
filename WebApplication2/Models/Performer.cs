using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication2.Models;

namespace WebApplication2.Models
{
    public class Performer
    {
        [Key]
        public int PerformerID { get; set; }

        [ForeignKey("PerformerType")]
        public int? PerformerTypeID { get; set; }
        public virtual PerformerType? PerformerType { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [ForeignKey("Genre")]
        public int? MainGenreID { get; set; }
        public virtual Genre? MainGenre { get; set; }

        [ForeignKey("Genre")]
        public int? SecondaryGenreID { get; set; }
        public virtual Genre? SecondaryGenre { get; set; }

        [ForeignKey("Country")]
        public string? CountryCode { get; set; }
        public virtual Country? Country { get; set; }

        public virtual ICollection<ReleasePerformer> ReleasePerformers { get; set; } = new List<ReleasePerformer>();
        public virtual ICollection<TrackPerformer> TrackPerformers { get; set; } = new List<TrackPerformer>();
/*        public virtual ICollection<Track> MainGuestTracks { get; set; } = new List<Track>();*/

        // Helper property to get all releases
        [NotMapped]
        public IEnumerable<Release> Releases => ReleasePerformers.Select(rp => rp.Release);
    }
}
