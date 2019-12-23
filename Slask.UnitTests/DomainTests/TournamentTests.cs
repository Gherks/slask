using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests
{
    public class TournamentTests
    {
        private readonly Tournament tournament;
        private readonly BracketRound bracketRound;
        private readonly BracketGroup bracketGroup;
        private readonly User user;
        private readonly Better better;

        public TournamentTests()
        {
            tournament = Tournament.Create("GSL 2019");
            bracketRound = tournament.AddBracketRound("Bracket round", 3) as BracketRound;
            bracketGroup = bracketRound.AddGroup() as BracketGroup;

            user = User.Create("Stålberto");
            better = tournament.AddBetter(user);
        }

        [Fact]
        public void CanCreateTournament()
        {
            string tournamentName = "ASUS ROG 2012";

            Tournament tournament = Tournament.Create(tournamentName);

            tournament.Should().NotBeNull();
            tournament.Id.Should().NotBeEmpty();
            tournament.Name.Should().Be(tournamentName);
            tournament.Rounds.Should().BeEmpty();
            tournament.Betters.Should().BeEmpty();
            tournament.Settings.Should().BeEmpty();
            tournament.MiscBetCatalogue.Should().BeEmpty();
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
            PlayerReference playerReference = bracketGroup.AddPlayerReference("Maru");

            PlayerReference fetchedPlayerReference = tournament.GetPlayerReferenceByPlayerId(playerReference.Id);

            fetchedPlayerReference.Should().NotBeNull();
            fetchedPlayerReference.Id.Should().Be(playerReference.Id);
            fetchedPlayerReference.Name.Should().Be(playerReference.Name);
        }

        [Fact]
        public void CanGetPlayerInTournamentByPlayerNameNoMatterLetterCasing()
        {
            PlayerReference playerReference = bracketGroup.AddPlayerReference("Maru");

            PlayerReference fetchedPlayerReference = tournament.GetPlayerReferenceByPlayerName(playerReference.Name.ToLower());

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
        public void TournamentDoesNotKeepRoundsThatFailedToBeCreated()
        {
            Tournament tournament = Tournament.Create("ASUS ROG 2012");

            for (int bestOf = 0; bestOf <= 64; bestOf += 2)
            {
                RoundBase bracketRound = tournament.AddRoundRobinRound("", bestOf, 8);
                RoundBase dualTournamentRound = tournament.AddDualTournamentRound("", bestOf);
                RoundBase roundRobinRound = tournament.AddRoundRobinRound("", bestOf, 8);

                bracketRound.Should().BeNull();
                dualTournamentRound.Should().BeNull();
                roundRobinRound.Should().BeNull();
                tournament.Rounds.Should().HaveCount(0);
            }
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

            foreach(string playerName in playerNames)
            {
                bracketGroup.AddPlayerReference(playerName);
            }

            List<PlayerReference> playerReferences = tournament.GetPlayerReferencesInTournament();

            playerReferences.Should().HaveCount(playerNames.Count);
            foreach (string playerName in playerNames)
            {
                playerReferences.Single(playerReference => playerReference.Name == playerName).Should().NotBeNull();
            }
        }

        public void FetchingAllPlayerReferencesShouldNotYieldTwoPlayerReferencesWithSameName()
        {
            string playerName = "Maru";
            bracketGroup.AddPlayerReference(playerName);
            bracketGroup.AddPlayerReference(playerName);

            List<PlayerReference> playerReferences = tournament.GetPlayerReferencesInTournament();

            playerReferences.Should().HaveCount(1);
            playerReferences.First().Name.Should().Be(playerName);
        }

        [Fact]
        public void FetchingAllPlayerRefencesShouldNotYieldRemovedPlayerReferences()
        {
            string playerName = "Maru";
            bracketGroup.AddPlayerReference(playerName);

            List<PlayerReference> playerReferences = tournament.GetPlayerReferencesInTournament();
            playerReferences.Should().HaveCount(1);
            playerReferences.First().Name.Should().Be(playerName);

            bracketGroup.RemovePlayerReference(playerName);

            playerReferences = tournament.GetPlayerReferencesInTournament();
            playerReferences.Should().HaveCount(0);
        }
    }
}
