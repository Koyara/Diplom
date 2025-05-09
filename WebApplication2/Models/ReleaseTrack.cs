﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models
{
    public class ReleaseTrack
    {
       // [Key]
        public int ID { get; set; }

        [ForeignKey("Release")]
        public int ReleaseID { get; set; }
        public virtual Release Release { get; set; }

        [ForeignKey("Track")]
        public int TrackID { get; set; }
        public virtual Track Track { get; set; }

        public int? TrackNumber { get; set; }
    }
}
