using System;
using Slask.Persistence.Services;
using Slask.Domain;
using Slask.Persistence;

namespace Slask.TestCore
{
    public class TournamentServiceContext : UserServiceContext
    {
        public TournamentService TournamentService { get; }

        protected TournamentServiceContext(SlaskContext slaskContext)
            : base(slaskContext)
        {
            TournamentService = new TournamentService(SlaskContext);
        }

        public Tournament WhenTournamentCreated()
        {
            Tournament tournament = TournamentService.CreateTournament("WCS 2019");
            SlaskContext.SaveChanges();

            return tournament;
        }

        public Round WhenAddedRoundToTournament()
        {
            Tournament tournament = WhenTournamentCreated();
            Round round = tournament.AddRound("Group A", 1, 5, 8);
            SlaskContext.SaveChanges();

            return round;
        }

        public Group WhenAddedGroupToTournament()
        {
            Round round = WhenAddedRoundToTournament();
            Group group = round.AddGroup();
            SlaskContext.SaveChanges();

            return group;
        }

        public Match WhenAddedMatchToTournament()
        {
            Group group = WhenAddedGroupToTournament();
            Match match = group.AddMatch("Maru", "Stork", DateTime.Now.AddSeconds(1));
            SlaskContext.SaveChanges();

            return match;
        }

        public Tournament WhenAddedMatchesToTournament()
        {
            Group group = WhenAddedGroupToTournament();
            group.AddMatch("Taeja", "Rain", DateTime.Now.AddSeconds(1));
            group.AddMatch("Bomber", "FanTaSy", DateTime.Now.AddSeconds(1));
            group.AddMatch("Stephano", "Thorzain", DateTime.Now.AddSeconds(1));
            SlaskContext.SaveChanges();

            return group.Round.Tournament;
        }

        public Match WhenAddedMatchToTournamentAndMatchIsFinished()
        {
            Group group = WhenAddedGroupToTournament();
            Match match = group.AddMatch("Maru", "Stork", DateTime.Now);

            int winningScore = (int)Math.Ceiling(group.Round.BestOf / 2.0);
            match.Player1.AddScore(winningScore);

            SlaskContext.SaveChanges();

            return match;
        }

        public Tournament WhenAddedBetterToTournament()
        {
            User user = WhenUserCreated();
            Tournament tournament = WhenTournamentCreated();
            tournament.AddBetter(user);
            SlaskContext.SaveChanges();

            return tournament;
        }

        public Tournament WhenCreatedACompleteTournament()
        {
            Tournament tournament = WhenAddedMatchesToTournament();
            User user = WhenUserCreated();

            return tournament;
        }

        public static new TournamentServiceContext GivenServices(SlaskContextCreatorInterface slaskContextCreator)
        {
            return new TournamentServiceContext(slaskContextCreator.CreateContext());
        }
    }
}
