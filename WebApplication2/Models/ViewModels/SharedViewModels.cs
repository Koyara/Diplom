using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.ViewModels
{
    public class PerformerViewModel
    {
        public int PerformerID { get; set; }
        public string Name { get; set; }
    }

    public class ReleaseViewModel
    {
        public int ReleaseID { get; set; }
        public string Title { get; set; }
    }
} 