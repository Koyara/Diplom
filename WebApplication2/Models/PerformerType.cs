using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class PerformerType
    {
        [Key]
        public int PerformerTypeID{ get; set; }
        public String? TypeName { get; set; }
    }
}
