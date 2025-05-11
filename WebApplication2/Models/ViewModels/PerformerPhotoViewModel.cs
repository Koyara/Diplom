using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.ViewModels
{
    public class PerformerPhotoViewModel
    {
        public int PerformerID { get; set; }
        public string PerformerName { get; set; }
        public string? ExistingPhotoBase64 { get; set; }
    }
} 