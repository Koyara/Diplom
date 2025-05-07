using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Country
    {
        [Key]
        public string CountryCode { get; set; }

        [Required]
        public string CountryName { get; set; }

        [ForeignKey("Rondo")]
        public int? RondoId { get; set; }
        public virtual Rondo? Rondo { get; set; }

        public Country()
        {

        }
    }
}
