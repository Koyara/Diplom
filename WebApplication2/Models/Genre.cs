using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Genre
    {
        [Key]
        public int GenreID { get; set; }

        public string? GenreName { get; set; }
    }
}
