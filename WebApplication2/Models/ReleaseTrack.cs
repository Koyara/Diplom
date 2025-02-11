using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class ReleaseTrack
    {
        [Key]
        public int ID { get; set; }
        public int TrackID { get; set; }
        public int ReleaseID {  get; set; }
    }
}
