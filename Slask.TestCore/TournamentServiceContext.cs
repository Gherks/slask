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

        //public void SetMockedTime(DateTime dateTime)
        //{
        //    mockedDateTime = DateTimeMocker.CreateDateTime(dateTime);
        //}

        public static new TournamentServiceContext GivenServices(SlaskContextCreatorInterface slaskContextCreator)
        {
            return new TournamentServiceContext(slaskContextCreator.CreateContext());
        }

        public Tournament WhenCreatedTournament(string name)
        {
            return TournamentService.CreateTournament(name);
        }

        public void WhenAddedBettersToTournament(Tournament tournament)
        {
            tournament.AddBetter(UserService.GetUserByName("Stålberto"));
            tournament.AddBetter(UserService.GetUserByName("Bönis"));
            tournament.AddBetter(UserService.GetUserByName("Guggelito"));
        }

        public Round WhenAddedBracketRoundToTournament(Tournament tournament, string name, int bestOf)
        {
            return tournament.AddBracketRound(name, bestOf);
        }

        public Round WhenAddedDualTournamentRoundToTournament(Tournament tournament, string name, int bestOf)
        {
            return tournament.AddDualTournamentRound(name, bestOf);
        }

        public Round WhenAddedRoundRobinRoundToTournament(Tournament tournament, string name, int bestOf, int advanceAmount)
        {
            return tournament.AddRoundRobinRound(name, bestOf, advanceAmount);
        }

        public GroupBase WhenAddedGroupToRound(Round round)
        {
            return round.AddGroup();
        }

        public void WhenAddedPlayerReferenceToGroup(GroupBase group, string name)
        {
            group.AddPlayerReference(name);
        }

        public void WhenSetStartDateTimeOnMatch(Match match, DateTime dateTime)
        {
            match.SetStartDateTime(dateTime);
        }

        public void WhenBetterPlacesBet(Better better, Match match, Player player)
        {
            better.PlaceMatchBet(match, player);
        }

        public void WhenBettersPlacesBetsOnAllMatchesInGroups(GroupBase group)
        {
            Tournament tournament = group.Round.Tournament;
            Better firstBetter = tournament.Betters[0];
            Better secondBetter = tournament.Betters[1];
            Better thirdBetter = tournament.Betters[2];

            Random random = new Random(133742069);
            int matchCounter = 0;
            foreach (Domain.Match match in group.Matches)
            {
                matchCounter++;

                if (matchCounter == 2)
                {
                    continue;
                }

                WhenBetterPlacesBet(firstBetter, match, random.Next(2) == 0 ? match.Player1 : match.Player2);
                WhenBetterPlacesBet(secondBetter, match, random.Next(2) == 0 ? match.Player1 : match.Player2);

                if (matchCounter == 3)
                {
                    continue;
                }

                WhenBetterPlacesBet(thirdBetter, match, random.Next(2) == 0 ? match.Player1 : match.Player2);
            }
        }

        public void WhenPlayerScoreIncreased(Player player, int score)
        {
            player.IncreaseScore(score);
        }
    }
}
