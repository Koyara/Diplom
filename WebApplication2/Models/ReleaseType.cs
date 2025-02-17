using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public class ReleaseType
    {
        [Key]
        public int ReleaseTypeID { get; set; }

        public string ReleaseTypeName { get; set; }

    }
}
