using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Country
    {
        [Key]
        public string CountryCode { get; set; }
        public string CountryName { get; set; }

        public Country()
        {

        }
    }
}
