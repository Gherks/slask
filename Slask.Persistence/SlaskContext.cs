using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Slask.Domain;
using Slask.Domain.Bets;
using Slask.Domain.Bets.BetTypes;
using Slask.Domain.Groups;
using Slask.Domain.Groups.GroupTypes;
using Slask.Domain.ObjectState;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.RoundTypes;
using Slask.Domain.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Persistence
{
    public class SlaskContext : DbContext
    {
        public SlaskContext()
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public SlaskContext(DbContextOptions options)
            : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
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
            SetupTables(modelBuilder);
            SetupIgnoredProperties(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            UpdateSortOrders();
            UpdateEntityStates();

            return base.SaveChanges();
        }

        private void UpdateEntityStates()
        {
            List<EntityEntry<ObjectStateInterface>> trackedEntities = new List<EntityEntry<ObjectStateInterface>>();
            trackedEntities.AddRange(ChangeTracker.Entries<ObjectStateInterface>());

            foreach (EntityEntry<ObjectStateInterface> trackedEntity in trackedEntities)
            {
                trackedEntity.State = ConvertState(trackedEntity.Entity.ObjectState);
                trackedEntity.Entity.ResetObjectState();
            }
        }

        private void UpdateSortOrders()
        {
            foreach (var entry in ChangeTracker.Entries<SortableInterface>())
            {
                entry.Entity.UpdateSortOrder();
            }
        }

        private EntityState ConvertState(ObjectStateEnum objectState)
        {
            switch (objectState)
            {
                case ObjectStateEnum.Added:
                    return EntityState.Added;
                case ObjectStateEnum.Modified:
                    return EntityState.Modified;
                case ObjectStateEnum.Deleted:
                    return EntityState.Deleted;
                default:
                    return EntityState.Unchanged;
            }
        }

        private void SetupTables(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoundBase>().ToTable("Round")
                .HasDiscriminator<ContestTypeEnum>("ContestType")
                .HasValue<BracketRound>(ContestTypeEnum.Bracket)
                .HasValue<DualTournamentRound>(ContestTypeEnum.DualTournament)
                .HasValue<RoundRobinRound>(ContestTypeEnum.RoundRobin);

            modelBuilder.Entity<GroupBase>().ToTable("Group")
                .HasDiscriminator<ContestTypeEnum>("ContestType")
                .HasValue<BracketGroup>(ContestTypeEnum.Bracket)
                .HasValue<DualTournamentGroup>(ContestTypeEnum.DualTournament)
                .HasValue<RoundRobinGroup>(ContestTypeEnum.RoundRobin);

            modelBuilder.Entity<BetBase>().ToTable("Bet")
                .HasDiscriminator<BetTypeEnum>("BetType")
                .HasValue<MatchBet>(BetTypeEnum.MatchBet)
                .HasValue<MiscellaneousBet>(BetTypeEnum.MiscellaneousBet);
        }

        private void SetupIgnoredProperties(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tournament>().Ignore(tournament => tournament.ObjectState);
            modelBuilder.Entity<Tournament>().Ignore(tournament => tournament.TournamentIssueReporter);

            modelBuilder.Entity<Better>().Ignore(better => better.ObjectState);

            modelBuilder.Entity<PlayerReference>().Ignore(playerReference => playerReference.ObjectState);

            modelBuilder.Entity<RoundBase>().Ignore(round => round.ObjectState);

            modelBuilder.Entity<GroupBase>().Ignore(group => group.ChoosenTyingPlayerEntries);
            modelBuilder.Entity<GroupBase>().Ignore(group => group.ObjectState);

            modelBuilder.Entity<BracketGroup>().Ignore(bracketGroup => bracketGroup.BracketNodeSystem);

            modelBuilder.Entity<Match>().Ignore(match => match.ObjectState);

            modelBuilder.Entity<BetBase>().Ignore(bet => bet.ObjectState);
        }
    }
}
