using Microsoft.EntityFrameworkCore;
using Slask.Data.Models;

namespace Slask.Data
{
    public class SlaskContext : DbContext
    {
        public SlaskContext(DbContextOptions<SlaskContext> options)
            : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Player>().HasData(
                new Player { Id = 1, Name = "Taeja" },
                new Player { Id = 2, Name = "Rain" },
                new Player { Id = 3, Name = "Bomber" },
                new Player { Id = 4, Name = "FanTaSy" },
                new Player { Id = 5, Name = "Stephano" },
                new Player { Id = 6, Name = "Thorzain" },
                new Player { Id = 7, Name = "Crank" },
                new Player { Id = 8, Name = "DeMuslim" },
                new Player { Id = 9, Name = "Stats" },
                new Player { Id = 10, Name = "ZerO" },
                new Player { Id = 11, Name = "Stork" }
                );
        }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Player> Players { get; set; }
    }
}
