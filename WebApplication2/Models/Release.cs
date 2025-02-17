using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class Release
    {
        [Key]
        public int ReleaseID { get; set; }

        public string Title {  get; set; }

        public DateOnly ReleaseDate { get; set; }

        [ForeignKey("Genre")]
        public int? MainGenreCode{ get; set; }
        public virtual Genre? MainGenre{ get; set; }

        [ForeignKey("Genre")]
        public int? SecondGenreCode { get; set; }
        public virtual Genre? SecondGenre { get; set; }

        [ForeignKey("ReleaseType")]
        public int? ReleaseTypeID { get; set; }
        public virtual ReleaseType? ReleaseType { get; set; }

        public byte[]? ReleaseCover { get; set; }

    }
}
