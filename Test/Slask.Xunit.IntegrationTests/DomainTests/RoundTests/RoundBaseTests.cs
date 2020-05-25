using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.RoundTypes;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.Xunit.IntegrationTests.DomainTests.RoundTests
{
    public class RoundBaseTests
    {
        private readonly Tournament tournament;

        public RoundBaseTests()
        {
            tournament = Tournament.Create("GSL 2019");
        }

        [Fact]
        public void AddingSeveralRoundsYieldsRoundsWithExpectedNames()
        {
            RoundRobinRound firstRound = tournament.AddRoundRobinRound();
            RoundRobinRound secondRound = tournament.AddRoundRobinRound();
            RoundRobinRound thirdRound = tournament.AddRoundRobinRound();
            RoundRobinRound fourthRound = tournament.AddRoundRobinRound();
            RoundRobinRound fifthRound = tournament.AddRoundRobinRound();

            firstRound.Name.Should().Be("Round A");
            secondRound.Name.Should().Be("Round B");
            thirdRound.Name.Should().Be("Round C");
            fourthRound.Name.Should().Be("Round D");
            fifthRound.Name.Should().Be("Round E");
        }

        [Fact]
        public void AddingEnoughRoundsToUseAllSingleLetterRestartsLetteringWithTwoLetters()
        {
            for (int index = 0; index < 10; ++index)
            {
                tournament.AddBracketRound();
                tournament.AddDualTournamentRound();
                tournament.AddRoundRobinRound();
            }

            tournament.Rounds[26].Name.Should().Be("Round AA");
            tournament.Rounds[27].Name.Should().Be("Round AB");
            tournament.Rounds[28].Name.Should().Be("Round AC");
            tournament.Rounds[29].Name.Should().Be("Round AD");
        }

        [Fact]
        public void CanRenameRound()
        {
            string newName = "New Round Name";

            RoundRobinRound round = tournament.AddRoundRobinRound();
            round.RenameTo(newName);

            round.Name.Should().Be(newName);
        }

        [Fact]
        public void CannotRenameRoundToEmptyname()
        {
            RoundRobinRound round = tournament.AddRoundRobinRound();
            round.RenameTo("");

            round.Name.Should().Be("Round A");
        }

        [Fact]
        public void CannotRenameRoundToTheSameAsOtherRoundNoMatterLetterCasing()
        {
            RoundRobinRound firstRound = tournament.AddRoundRobinRound();
            RoundRobinRound secondRound = tournament.AddRoundRobinRound();

            string newName = "New Round Name";
            string initialRoundName = secondRound.Name;

            firstRound.RenameTo(newName);
            secondRound.RenameTo(newName.ToLower());

            firstRound.Name.Should().Be(newName);
            secondRound.Name.Should().Be(initialRoundName);
        }

        [Fact]
        public void CanDetectWhenRoundIsTheFirstOne()
        {
            RoundBase firstRound = tournament.AddRoundRobinRound();
            RoundBase secondRound = tournament.AddRoundRobinRound();

            bool isFirstRound = firstRound.IsFirstRound();

            isFirstRound.Should().BeTrue();
        }

        [Fact]
        public void CanDetectWhenRoundIsNotTheFirstOne()
        {
            RoundBase firstRound = tournament.AddRoundRobinRound();
            RoundBase secondRound = tournament.AddRoundRobinRound();

            bool isFirstRound = secondRound.IsFirstRound();

            isFirstRound.Should().BeFalse();
        }

        [Fact]
        public void CanDetectWhenRoundIsTheLastOne()
        {
            RoundBase firstRound = tournament.AddRoundRobinRound();
            RoundBase secondRound = tournament.AddRoundRobinRound();

            bool isLastRound = secondRound.IsLastRound();

            isLastRound.Should().BeTrue();
        }

        [Fact]
        public void CanDetectWhenRoundIsNotTheLastOne()
        {
            RoundBase firstRound = tournament.AddRoundRobinRound();
            RoundBase secondRound = tournament.AddRoundRobinRound();

            bool isFirstRound = firstRound.IsLastRound();

            isFirstRound.Should().BeFalse();
        }

        [Fact]
        public void CanFetchRoundAfterThisRound()
        {
            RoundBase firstRound = tournament.AddRoundRobinRound();
            RoundBase secondRound = tournament.AddRoundRobinRound();

            RoundBase nextRound = firstRound.GetNextRound();

            nextRound.Should().Be(secondRound);
        }

        [Fact]
        public void FetchingNextRoundWithLastRoundYieldsNull()
        {
            RoundBase firstRound = tournament.AddRoundRobinRound();
            RoundBase secondRound = tournament.AddRoundRobinRound();

            RoundBase nextRound = secondRound.GetNextRound();

            nextRound.Should().BeNull();
        }

        [Fact]
        public void CanFetchRoundBeforeThisRound()
        {
            RoundBase beforeRound = tournament.AddRoundRobinRound();
            RoundBase thisRound = tournament.AddRoundRobinRound();

            RoundBase previousRound = thisRound.GetPreviousRound();

            previousRound.Should().Be(beforeRound);
        }

        [Fact]
        public void FetchingPreviousRoundWithFirstRoundYieldsNull()
        {
            RoundBase firstRound = tournament.AddRoundRobinRound();

            RoundBase previousRound = firstRound.GetPreviousRound();

            previousRound.Should().BeNull();
        }

        [Fact]
        public void CanFetchFirstMatch()
        {
            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain" };

            RoundBase round = tournament.AddBracketRound();
            round.SetPlayersPerGroupCount(2);

            foreach(string playerName in playerNames)
            {
                round.RegisterPlayerReference(playerName);
            }

            Match match = round.GetFirstMatch();

            match.Group.Should().Be(round.Groups[0]);
            match.Player1.Name.Should().Be(playerNames[0]);
            match.Player2.Name.Should().Be(playerNames[1]);
        }

        [Fact]
        public void CanFetchLastMatch()
        {
            List<string> playerNames = new List<string>() { "Maru", "Stork", "Taeja", "Rain" };

            RoundBase round = tournament.AddBracketRound();
            round.SetPlayersPerGroupCount(2);

            foreach (string playerName in playerNames)
            {
                round.RegisterPlayerReference(playerName);
            }

            Match match = round.GetLastMatch();

            match.Group.Should().Be(round.Groups[1]);
            match.Player1.Name.Should().Be(playerNames[2]);
            match.Player2.Name.Should().Be(playerNames[3]);
        }

        [Fact]
        public void CanRegisterPlayerReferencesToFirstRound()
        {
            string playerName = "Maru";

            RoundRobinRound round = tournament.AddRoundRobinRound();

            PlayerReference playerReference = round.RegisterPlayerReference(playerName);

            playerReference.Id.Should().NotBeEmpty();
            playerReference.Name.Should().Be(playerName);
            playerReference.TournamentId.Should().Be(round.TournamentId);
            playerReference.Tournament.Should().Be(round.Tournament);

            round.PlayerReferences.First().Should().Be(playerReference);
        }

        [Fact]
        public void CannotRegisterPlayerReferencesToRoundsThatIsNotTheFirstOne()
        {
            string playerName = "Maru";
            int roundCount = 5;

            RoundRobinRound firstRound = tournament.AddRoundRobinRound();

            for (int index = 1; index < roundCount; ++index)
            {
                RoundRobinRound round = tournament.AddRoundRobinRound();
                PlayerReference playerReference = round.RegisterPlayerReference(playerName + index.ToString());

                playerReference.Should().BeNull();
                round.PlayerReferences.Should().BeEmpty();
            }
        }

        [Fact]
        public void CanExcludePlayerReferencesFromGroupsWithinFirstRound()
        {
            string playerName = "Maru";

            RoundRobinRound round = tournament.AddRoundRobinRound();

            round.RegisterPlayerReference(playerName);
            bool exclusionResult = round.ExcludePlayerReference(playerName);

            exclusionResult.Should().BeTrue();
            round.PlayerReferences.Should().BeEmpty();
        }
    }
}
