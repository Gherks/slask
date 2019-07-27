using System;
using Slask.Persistence.Services;
using Slask.Domain;
using Slask.Persistence;
using Slask.Common;

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

        public Tournament WhenCreatedTournament()
        {
            Tournament tournament = TournamentService.CreateTournament("WCS 2019");
            SlaskContext.SaveChanges();

            return tournament;
        }

        public Round WhenCreatedRoundRobinRoundInTournament()
        {
            Tournament tournament = WhenCreatedTournament();
            Round round = tournament.AddRoundRobinRound("Round-Robin Group A", 5, 4);
            SlaskContext.SaveChanges();

            return round;
        }

        public Round WhenCreatedDualTournamentRoundInTournament()
        {
            Tournament tournament = WhenCreatedTournament();
            Round round = tournament.AddDualTournamentRound("Dual Tournament Group B", 5);
            SlaskContext.SaveChanges();

            return round;
        }

        public Round WhenCreatedBracketRoundInTournament()
        {
            Tournament tournament = WhenCreatedTournament();
            Round round = tournament.AddBracketRound("Bracket Round Group C", 5);
            SlaskContext.SaveChanges();

            return round;
        }

        public Group WhenCreatedGroupInRoundRobinRoundInTournament()
        {
            Round round = WhenCreatedRoundRobinRoundInTournament();
            Group group = round.AddGroup();
            SlaskContext.SaveChanges();

            return group;
        }

        public Group WhenCreatedGroupInDualTournamentRoundInTournament()
        {
            Round round = WhenCreatedDualTournamentRoundInTournament();
            Group group = round.AddGroup();
            SlaskContext.SaveChanges();

            return group;
        }

        public Group WhenCreatedGroupInBracketRoundInTournament()
        {
            Round round = WhenCreatedBracketRoundInTournament();
            Group group = round.AddGroup();
            SlaskContext.SaveChanges();

            return group;
        }

        public Tournament WhenCreatedMatchesInRoundRobinRoundInTournament()
        {
            Group group = WhenCreatedGroupInRoundRobinRoundInTournament();
            AddMatchesToGroup(group);
            SlaskContext.SaveChanges();

            return group.Round.Tournament;
        }

        public Tournament WhenCreatedMatchesInDualTournamentRoundInTournament()
        {
            Group group = WhenCreatedGroupInDualTournamentRoundInTournament();
            AddMatchesToGroup(group);
            SlaskContext.SaveChanges();

            return group.Round.Tournament;
        }

        public Tournament WhenCreatedMatchesInBracketRoundInTournament()
        {
            Group group = WhenCreatedGroupInBracketRoundInTournament();
            AddMatchesToGroup(group);
            SlaskContext.SaveChanges();

            return group.Round.Tournament;
        }

        public Match WhenCreatedMatchInRoundRobinRoundInTournamentAndMatchIsFinished()
        {
            Group group = WhenCreatedGroupInRoundRobinRoundInTournament();
            Match match = group.AddMatch("Maru", "Stork", DateTimeHelper.Now);

            int winningScore = (int)Math.Ceiling(group.Round.BestOf / 2.0);
            match.Player1.AddScore(winningScore);
            SlaskContext.SaveChanges();

            return match;
        }

        public Tournament WhenCreatedBetterInTournament()
        {
            User user = WhenCreatedUser();
            Tournament tournament = WhenCreatedTournament();
            tournament.AddBetter(user);
            SlaskContext.SaveChanges();

            return tournament;
        }

        private void AddMatchesToGroup(Group group)
        {
            group.AddMatch("Maru", "Stork", DateTimeHelper.Now.AddSeconds(1));
            group.AddMatch("Taeja", "Rain", DateTimeHelper.Now.AddSeconds(1));
            group.AddMatch("Bomber", "FanTaSy", DateTimeHelper.Now.AddSeconds(1));
            group.AddMatch("Stephano", "Thorzain", DateTimeHelper.Now.AddSeconds(1));
        }

        public static new TournamentServiceContext GivenServices(SlaskContextCreatorInterface slaskContextCreator)
        {
            return new TournamentServiceContext(slaskContextCreator.CreateContext());
        }
    }
}
