using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class TrackPerformer
    {
        [Key]
        public int TrackPerformerID { get; set; }
        public int TrackID { get; set; }
        public int PerformerID { get; set; }

        public virtual Track Track { get; set; }
        public virtual Performer Performer { get; set; }
    }
}