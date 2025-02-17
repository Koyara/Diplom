using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Track
    {
        [Key]
        public int TrackID { get; set; }
        public TimeSpan? Length { get; set; }
        public bool? IsSong { get; set; }
        public string Title { get; set; }

        [ForeignKey("Performer")]
        public int? GuestID { get; set; }
        public virtual Performer? MainGuest { get; set; }

        [ForeignKey("Performer")]
        public int? SecondGuestID { get; set; }
        public virtual Performer? SecondGuest { get; set; }

        [ForeignKey("Language")]
        public string? LanguageCode{ get; set; }
        public virtual Language? Language { get; set; }

        [ForeignKey("Scale")]
        public int? ScaleID { get; set; }
        public virtual Scale? Scale { get; set; }

        public int BPM;
    }
}
