using System;
using Slask.Persistance.Services;
using Slask.Domain;

namespace Slask.UnitTests.TestContexts
{
    public class TournamentServiceContext : UserServiceContext
    {
        public TournamentService TournamentService { get; }

        protected TournamentServiceContext()
        {
            TournamentService = new TournamentService(SlaskContext);
        }

        public Tournament WhenCreatedTournament()
        {
            Tournament tournament = TournamentService.CreateTournament("WCS 2019");
            SlaskContext.SaveChanges();

            return tournament;
        }

        public Round WhenAddedRoundToTournament()
        {
            Tournament tournament = WhenCreatedTournament();
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
            Player player1 = Player.Create("Maru");
            Player player2 = Player.Create("Stork");
            Match match = group.AddMatch(MatchPlayer.Create(player1), MatchPlayer.Create(player2));
            SlaskContext.SaveChanges();

            return match;
        }

        public Better WhenAddedBetterToTournament()
        {
            User user = WhenCreatedUser();
            Tournament tournament = WhenCreatedTournament();
            Better better = tournament.AddBetter(user);
            SlaskContext.SaveChanges();

            return better;
        }

        public Tournament WhenTournamentWithMatchesHasBeenCreated()
        {
            Group group = WhenAddedGroupToTournament();
            MatchPlayer matchPlayerMaru = MatchPlayer.Create(Player.Create("Maru"));
            MatchPlayer matchPlayerStork = MatchPlayer.Create(Player.Create("Stork"));
            MatchPlayer matchPlayerTaeja = MatchPlayer.Create(Player.Create("Taeja"));
            MatchPlayer matchPlayerRain = MatchPlayer.Create(Player.Create("Rain"));
            MatchPlayer matchPlayerBomber = MatchPlayer.Create(Player.Create("Bomber"));
            MatchPlayer matchPlayerFantaSy = MatchPlayer.Create(Player.Create("FanTaSy"));
            MatchPlayer matchPlayerStephano = MatchPlayer.Create(Player.Create("Stephano"));
            MatchPlayer matchPlayerThorzain = MatchPlayer.Create(Player.Create("Thorzain"));
            group.AddMatch(matchPlayerMaru, matchPlayerStork);
            group.AddMatch(matchPlayerTaeja, matchPlayerRain);
            group.AddMatch(matchPlayerBomber, matchPlayerFantaSy);
            group.AddMatch(matchPlayerStephano, matchPlayerThorzain);
            SlaskContext.SaveChanges();

            return group.Round.Tournament;
        }

        public Tournament WhenTournamentWithBettersHasBeenCreated()
        {
            throw new NotImplementedException();
        }

        public Tournament WhenACompleteTournamentHasBeenCreated()
        {
            throw new NotImplementedException();
        }

        public static new TournamentServiceContext GivenServices()
        {
            return new TournamentServiceContext();
        }
    }
}
