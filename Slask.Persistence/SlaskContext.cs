using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Slask.Domain;
using Slask.Domain.Bets;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;

namespace Slask.Persistence
{
    public class SlaskContext : DbContext
    {
        public SlaskContext()
        {
        }

        public SlaskContext(DbContextOptions options)
            : base(options)
        {
        }

        public SlaskContext(DbContextOptions<SlaskContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; private set; }
        public DbSet<Tournament> Tournaments { get; private set; }

        public static readonly ILoggerFactory DebugLoggerFactory 
            = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter((category, level) 
                    => category == DbLoggerCategory.ChangeTracking.Name
                    && level == LogLevel.Debug)
                .AddDebug();
        });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server = (localdb)\\MSSQLLocalDB; Database = SlaskDB; Trusted_Connection = True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RoundBase>().ToTable("Round");
            modelBuilder.Entity<GroupBase>().ToTable("Group");
            modelBuilder.Entity<BetBase>().ToTable("Bet");

            modelBuilder.Entity<Tournament>().Ignore(tournament => tournament.TournamentIssueReporter);

            modelBuilder.Entity<GroupBase>().Ignore(group => group.PlayerReferences);
            modelBuilder.Entity<GroupBase>().Ignore(group => group.ChoosenTyingPlayerEntries);

            //modelBuilder.Entity<BracketGroup>().Ignore(bracketGroup => bracketGroup.BracketNodeSystem); 

            modelBuilder.Entity<Match>().Ignore(match => match.Player1);
            modelBuilder.Entity<Match>().Ignore(match => match.Player2);

            modelBuilder.Entity<Player>().Ignore(player => player.Name);
        }
    }
}
