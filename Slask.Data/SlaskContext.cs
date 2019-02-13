using Microsoft.EntityFrameworkCore;
using Slask.Domain;

namespace Slask.Data
{
    public class SlaskContext : DbContext
    {
        public SlaskContext(DbContextOptions<SlaskContext> options)
            : base(options)
        { }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
