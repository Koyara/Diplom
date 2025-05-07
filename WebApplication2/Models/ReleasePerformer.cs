using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class ReleasePerformer
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey("Release")]
        public int ReleaseID { get; set; }
        public virtual Release Release { get; set; }

        [ForeignKey("Performer")]
        public int PerformerID { get; set; }
        public virtual Performer Performer { get; set; }
    }
} 