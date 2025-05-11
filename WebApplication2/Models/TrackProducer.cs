using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class TrackProducer
    {
        [Key]
        public int TrackProducerID { get; set; }

        [ForeignKey("Track")]
        public int TrackID { get; set; }
        public virtual Track Track { get; set; }

        [ForeignKey("Producer")]
        public int ProducerID { get; set; }
        public virtual Performer Producer { get; set; }
    }
} 