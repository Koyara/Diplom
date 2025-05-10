using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.ViewModels
{
    public class TrackViewModel
    {
        public int TrackID { get; set; }

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Display(Name = "Length")]
        public TimeSpan? Length { get; set; }

        [Display(Name = "BPM")]
        [Range(1, 240)]
        public int? BPM { get; set; }

        [Display(Name = "Release")]
        public int? ReleaseID { get; set; }

        public List<PerformerViewModel> AvailablePerformers { get; set; } = new List<PerformerViewModel>();
        public List<ReleaseViewModel> AvailableReleases { get; set; } = new List<ReleaseViewModel>();
    }
} 