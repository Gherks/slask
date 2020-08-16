using FluentAssertions;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.RoundTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.Domain.Xunit.UnitTests
{
    public class TournamentTests
    {
        private readonly Tournament tournament;
        private readonly BracketRound bracketRound;
        private readonly User user;
        private readonly Better better;

        public TournamentTests()
        {
            tournament = Tournament.Create("GSL 2019");
            bracketRound = tournament.AddBracketRound() as BracketRound;

            user = User.Create("Stålberto");
            better = tournament.AddBetter(user);
        }

        [Fact]
        public void CanCreateTournament()
        {
            string tournamentName = "ASUS ROG 2012";
            int acceptableInaccuracy = 2000;

            Tournament tournament = Tournament.Create(tournamentName);

            tournament.Should().NotBeNull();
            tournament.Id.Should().NotBeEmpty();
            tournament.Name.Should().Be(tournamentName);
            tournament.Created.Should().BeCloseTo(DateTime.Now, acceptableInaccuracy);
            tournament.Rounds.Should().BeEmpty();
            tournament.Betters.Should().BeEmpty();
        }

        [Fact]
        public void CanGetRoundInTournamentByRoundId()
        {
            RoundBase fetchedRound = tournament.GetRoundByRoundId(bracketRound.Id);

            fetchedRound.Should().NotBeNull();
            fetchedRound.Id.Should().Be(bracketRound.Id);
            fetchedRound.Name.Should().Be(bracketRound.Name);
        }

        [Fact]
        public void CanGetRoundInTournamentByRoundName()
        {
            RoundBase fetchedRound = tournament.GetRoundByRoundName(bracketRound.Name);

            fetchedRound.Should().NotBeNull();
            fetchedRound.Id.Should().Be(bracketRound.Id);
            fetchedRound.Name.Should().Be(bracketRound.Name);
        }

        [Fact]
        public void CanGetPlayerReferenceInTournamentByPlayerId()
        {
            PlayerReference playerReference = bracketRound.RegisterPlayerReference("Maru");

            PlayerReference fetchedPlayerReference = tournament.GetPlayerReferenceByPlayerId(playerReference.Id);

            fetchedPlayerReference.Should().NotBeNull();
            fetchedPlayerReference.Id.Should().Be(playerReference.Id);
            fetchedPlayerReference.Name.Should().Be(playerReference.Name);
        }

        [Fact]
        public void CanGetPlayerInTournamentByPlayerName()
        {
            PlayerReference playerReference = bracketRound.RegisterPlayerReference("Maru");

            PlayerReference fetchedPlayerReference = tournament.GetPlayerReferenceByPlayerName(playerReference.Name);

            fetchedPlayerReference.Should().NotBeNull();
            fetchedPlayerReference.Id.Should().Be(playerReference.Id);
            fetchedPlayerReference.Name.Should().Be(playerReference.Name);
        }

        [Fact]
        public void CanGetBetterInTournamentByBetterId()
        {
            Better fetchedBetter = tournament.GetBetterById(better.Id);

            fetchedBetter.Should().NotBeNull();
            fetchedBetter.User.Should().NotBeNull();
            fetchedBetter.Id.Should().Be(better.Id);
            fetchedBetter.User.Name.Should().Be(better.User.Name);
        }

        [Fact]
        public void CanGetBetterInTournamentByBetterName()
        {
            Better fetchedBetter = tournament.GetBetterByName(better.User.Name);

            fetchedBetter.Should().NotBeNull();
            fetchedBetter.User.Should().NotBeNull();
            fetchedBetter.Id.Should().Be(better.Id);
            fetchedBetter.User.Name.Should().Be(better.User.Name);
        }

        [Fact]
        public void CanGetBetterInTournamentByBetterNameNoMatterLetterCasing()
        {
            Better fetchedBetter = tournament.GetBetterByName(better.User.Name.ToLower());

            fetchedBetter.Should().NotBeNull();
            fetchedBetter.User.Should().NotBeNull();
            fetchedBetter.Id.Should().Be(better.Id);
            fetchedBetter.User.Name.Should().Be(better.User.Name);
        }

        [Fact]
        public void CanOnlyAddUserAsBetterOncePerTournament()
        {
            Better duplicateBetter = tournament.AddBetter(user);

            duplicateBetter.Should().BeNull();
        }

        [Fact]
        public void CanRemoveBetterFromTournament()
        {
            bool removalResult = tournament.RemoveBetter(better);

            removalResult.Should().BeTrue();
            tournament.Betters.Should().HaveCount(0);
        }

        [Fact]
        public void TournamentKeepsRoundsThatWasSuccessfullyCreated()
        {
            tournament.Rounds.Should().HaveCount(1);
            tournament.Rounds.First().Should().Be(bracketRound);
        }

        [Fact]
        public void TournamentKeepsBettersThatWasSuccessfullyCreated()
        {
            tournament.Betters.Should().HaveCount(1);
            tournament.Betters.First().Should().Be(better);
        }

        [Fact]
        public void TournamentDoesNotKeepBettersThatFailedToBeCreated()
        {
            Tournament tournament = Tournament.Create("ASUS ROG 2012");
            User user = User.Create("Guggelito");

            tournament.AddBetter(user);
            Better better = tournament.AddBetter(user);

            better.Should().BeNull();
            tournament.Betters.Should().HaveCount(1);
        }

        [Fact]
        public void TournamentCanFetchPlayerReferences()
        {
            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain" };

            foreach (string playerName in playerNames)
            {
                bracketRound.RegisterPlayerReference(playerName);
            }

            List<PlayerReference> playerReferences = tournament.GetPlayerReferences();

            playerReferences.Should().HaveCount(playerNames.Count);
            foreach (string playerName in playerNames)
            {
                playerReferences.Single(playerReference => playerReference.Name == playerName).Should().NotBeNull();
            }
        }

        public void FetchingAllPlayerReferencesShouldNotYieldTwoPlayerReferencesWithSameName()
        {
            string playerName = "Maru";
            bracketRound.RegisterPlayerReference(playerName);
            bracketRound.RegisterPlayerReference(playerName);

            List<PlayerReference> playerReferences = tournament.GetPlayerReferences();

            playerReferences.Should().HaveCount(1);
            playerReferences.First().Name.Should().Be(playerName);
        }

        [Fact]
        public void FetchingAllPlayerRefencesShouldNotYieldRemovedPlayerReferences()
        {
            string playerName = "Maru";
            bracketRound.RegisterPlayerReference(playerName);

            List<PlayerReference> playerReferences = tournament.GetPlayerReferences();
            playerReferences.Should().HaveCount(1);
            playerReferences.First().Name.Should().Be(playerName);

            bracketRound.ExcludePlayerReference(playerName);

            playerReferences = tournament.GetPlayerReferences();
            playerReferences.Should().BeEmpty();
        }

        [Fact]
        public void CanFetchFirstRound()
        {
            tournament.AddBracketRound();
            tournament.AddDualTournamentRound();
            tournament.AddRoundRobinRound();

            tournament.GetFirstRound().Should().Be(bracketRound);
        }

        [Fact]
        public void CanFetchLastRound()
        {
            tournament.AddBracketRound();
            tournament.AddDualTournamentRound();
            RoundRobinRound lastRound = tournament.AddRoundRobinRound();

            tournament.GetLastRound().Should().Be(lastRound);
        }
    }
}
