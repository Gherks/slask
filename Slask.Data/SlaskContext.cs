using Microsoft.EntityFrameworkCore;
using Slask.Domain;

namespace Slask.Data
{
    public class SlaskContext : DbContext
    {
        public SlaskContext()
        {
        }

        public SlaskContext(DbContextOptions<SlaskContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB; Database = SlaskDB; Trusted_Connection = True;");
            }
        }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Player> Players { get; set; }
    }
}
