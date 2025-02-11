using Microsoft.EntityFrameworkCore.Metadata;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class Scale
    {
        [Key]
        public int ScaleId { get; set; }
        public int? Name{ get; set; }
    }
}
