using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Contributor
    {
        public int ContributorID { get; set; }
        public string FullName { get; set; }


        public Contributor()
        {

        }
    }
}
