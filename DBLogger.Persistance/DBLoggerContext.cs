using DBLogger.Domain;
using Microsoft.EntityFrameworkCore;

namespace DBLogger.Persistance
{
    public class DBLoggerContext : DbContext
    {
        public DBLoggerContext()
        {
        }

        public DBLoggerContext(DbContextOptions<DBLoggerContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB; Database = SlaskDB; Trusted_Connection = True;");
        }

        public DbSet<Log> Logs { get; set; }
    }
}