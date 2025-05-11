using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Performer> Performer { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Release> Release { get; set; }
        public DbSet<Track> Track { get; set; }
        public DbSet<TrackPerformer> TrackPerformer { get; set; }
        public DbSet<ReleaseTrack> ReleaseTrack { get; set; }
        public DbSet<ReleasePerformer> ReleasePerformer { get; set; }
        public DbSet<PerformerType> PerformerType { get; set; }
        public DbSet<ReleaseType> ReleaseType { get; set; }
        public DbSet<Scale> Scale { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Contributor> Contributor { get; set; }
        public DbSet<TrackProducer> TrackProducer { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ReleaseTrack configuration
            builder.Entity<ReleaseTrack>()
                .HasKey(rt => new { rt.ReleaseID, rt.TrackID });

            builder.Entity<ReleaseTrack>()
                .HasOne(rt => rt.Release)
                .WithMany(r => r.ReleaseTracks)
                .HasForeignKey(rt => rt.ReleaseID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ReleaseTrack>()
                .HasOne(rt => rt.Track)
                .WithMany(t => t.ReleaseTracks)
                .HasForeignKey(rt => rt.TrackID)
                .OnDelete(DeleteBehavior.Cascade);

            // ReleasePerformer configuration
            builder.Entity<ReleasePerformer>()
                .HasKey(rp => new { rp.ReleaseID, rp.PerformerID });

            builder.Entity<ReleasePerformer>()
                .HasOne(rp => rp.Release)
                .WithMany(r => r.ReleasePerformers)
                .HasForeignKey(rp => rp.ReleaseID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ReleasePerformer>()
                .HasOne(rp => rp.Performer)
                .WithMany(p => p.ReleasePerformers)
                .HasForeignKey(rp => rp.PerformerID)
                .OnDelete(DeleteBehavior.Cascade);

            // TrackPerformer configuration
            builder.Entity<TrackPerformer>()
                .HasKey(tp => tp.TrackPerformerID);

            builder.Entity<TrackPerformer>()
                .HasOne(tp => tp.Track)
                .WithMany(t => t.TrackPerformers)
                .HasForeignKey(tp => tp.TrackID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TrackPerformer>()
                .HasOne(tp => tp.Performer)
                .WithMany(p => p.TrackPerformers)
                .HasForeignKey(tp => tp.PerformerID)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
