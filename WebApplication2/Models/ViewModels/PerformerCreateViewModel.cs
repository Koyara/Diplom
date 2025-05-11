using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.ViewModels
{
    public class PerformerCreateViewModel
    {
        public int? PerformerTypeID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public int? MainGenreID { get; set; }
        public int? SecondaryGenreID { get; set; }
        public string? CountryCode { get; set; }
        public byte[]? Photo { get; set; }
    }
} 