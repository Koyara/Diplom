using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Contributor
    {
        [Key]
        public int ContributorID { get; set; }
        public string FullName { get; set; }
        public DateOnly? BirthDate { get; set; }

        [ForeignKey("Country")]
        public string? CountryCode { get; set; }
        public virtual Country Country{ get; set; }
        public bool? IsMale { get; set; }
       



        public Contributor()
        {

        }
    }
}
