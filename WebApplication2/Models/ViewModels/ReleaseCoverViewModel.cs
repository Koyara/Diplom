using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.ViewModels
{
    public class ReleaseCoverViewModel
    {
        public int ReleaseID { get; set; }
        public string ReleaseTitle { get; set; }
        public string ExistingCoverBase64 { get; set; }
    }
} 