using System.ComponentModel.DataAnnotations.Schema;
using WebApplication2.Models;

namespace WebApplication2.Models
{
    public class Performer
    {
        public int PerformerID { get; set; }

        [ForeignKey("PerformerType")]
        public int? PerformerTypeID {  get; set; }  
        public virtual PerformerType? PerformerType {  get; set; }
        public String Name { get; set; }
        public string Description { get; set; }

        [ForeignKey("Genre")]
        public int? MainGenreID { get; set; }
        public virtual Genre? MainGenre {  get; set; }
        [ForeignKey("Genre")]
        public int? SecondaryGenreID { get; set; }
        public virtual Genre? SecondaryGenre { get; set; }  

    }
}
