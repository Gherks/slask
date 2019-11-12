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
            if (slaskContextCreator == null)
            {
                throw new ArgumentNullException(nameof(slaskContextCreator));
            }

            return new TournamentServiceContext(slaskContextCreator.CreateContext());
        }

        public Tournament WhenCreatedTournament(string name)
        {
            return TournamentService.CreateTournament(name);
        }

        public void WhenAddedBettersToTournament(Tournament tournament)
        {
            if (tournament == null)
            {
                throw new ArgumentNullException(nameof(tournament));
            }

            tournament.AddBetter(UserService.GetUserByName("Stålberto"));
            tournament.AddBetter(UserService.GetUserByName("Bönis"));
            tournament.AddBetter(UserService.GetUserByName("Guggelito"));
        }

        public static Round WhenAddedBracketRoundToTournament(Tournament tournament, string name, int bestOf)
        {
            if (tournament == null)
            {
                throw new ArgumentNullException(nameof(tournament));
            }

            return tournament.AddRound(RoundType.Bracket, name, bestOf);
        }

        public static Round WhenAddedDualTournamentRoundToTournament(Tournament tournament, string name, int bestOf)
        {
            if (tournament == null)
            {
                throw new ArgumentNullException(nameof(tournament));
            }

            return tournament.AddRound(RoundType.DualTournament, name, bestOf);
        }

        public static Round WhenAddedRoundRobinRoundToTournament(Tournament tournament, string name, int bestOf, int advanceAmount)
        {
            if (tournament == null)
            {
                throw new ArgumentNullException(nameof(tournament));
            }

            return tournament.AddRound(RoundType.RoundRobin, name, bestOf, advanceAmount);
        }

        public static GroupBase WhenAddedGroupToRound(Round round)
        {
            if (round == null)
            {
                throw new ArgumentNullException(nameof(round));
            }

            return round.AddGroup();
        }

        public static void WhenAddedPlayerReferenceToGroup(GroupBase group, string name)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

            group.AddPlayerReference(name);
        }

        public static void WhenSetStartDateTimeOnMatch(Match match, DateTime dateTime)
        {
            if (match == null)
            {
                throw new ArgumentNullException(nameof(match));
            }

            match.SetStartDateTime(dateTime);
        }

        public static void WhenBetterPlacesBet(Better better, Match match, Player player)
        {
            if (better == null)
            {
                throw new ArgumentNullException(nameof(better));
            }

            better.PlaceMatchBet(match, player);
        }

        public static void WhenBettersPlacesBetsOnAllMatchesInGroups(GroupBase group)
        {
            if (group == null)
            {
                throw new ArgumentNullException(nameof(group));
            }

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

        public static void WhenPlayerScoreIncreased(Player player, int score)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            player.IncreaseScore(score);
        }
    }
}
