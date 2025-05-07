using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Scale
    {
        [Key]
        public int ScaleId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
