using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;

namespace WebApplication2.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<WebApplication2.Models.Contributor> Contributor { get; set; } = default!;
        public DbSet<WebApplication2.Models.Country> Country{ get; set; } = default!;
        public DbSet<WebApplication2.Models.Scale> Scale { get; set; } = default!;
        public DbSet<WebApplication2.Models.Language> Language { get; set; } = default!;
        public DbSet<WebApplication2.Models.PerformerType> PerformerType { get; set; } = default!;
        public DbSet<WebApplication2.Models.Genre> Genre { get; set; } = default!;

        public DbSet<WebApplication2.Models.ReleaseTrack> ReleaseTrack { get; set; } = default!;
        public DbSet<WebApplication2.Models.Performer> Performer { get; set; } = default!;

        public DbSet<WebApplication2.Models.Rondo> Rondo { get; set; } = default!;
    }
}
