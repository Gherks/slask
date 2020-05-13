using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.Bases;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Slask.UnitTests.DomainTests.RoundTests
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
            for(int index = 0; index < 10; ++index)
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
        public void CannotRenameRoundToTheSameAsOtherRound()
        {
            RoundRobinRound firstRound = tournament.AddRoundRobinRound();
            RoundRobinRound secondRound = tournament.AddRoundRobinRound();

            string newName = "New Round Name";
            string initialRoundName = secondRound.Name;

            firstRound.RenameTo(newName);
            secondRound.RenameTo(newName);

            firstRound.Name.Should().Be(newName);
            secondRound.Name.Should().Be(initialRoundName);
        }

        [Fact]
        public void CannotSetRoundBestOfToZero()
        {
            RoundRobinRound round = tournament.AddRoundRobinRound();

            bool setResult = round.SetBestOf(0);

            setResult.Should().BeFalse();
            round.BestOf.Should().Be(3);
        }

        [Fact]
        public void CannotSetRoundBestOfToEvenValue()
        {
            RoundRobinRound round = tournament.AddRoundRobinRound();
            round.SetBestOf(1);

            for (int bestOf = 1; bestOf < 32; ++bestOf)
            {
                bool setResult = round.SetBestOf(bestOf);

                bool bestOfIsEven = bestOf % 2 == 0;
                if (bestOfIsEven)
                {
                    setResult.Should().BeFalse();
                    round.BestOf.Should().Be(bestOf - 1);
                }
                else
                {
                    setResult.Should().BeTrue();
                    round.BestOf.Should().Be(bestOf);
                }
            }
        }

        [Fact]
        public void FetchingPreviousRoundFromFirstRoundYieldsNull()
        {
            RoundBase round = tournament.AddRoundRobinRound();

            RoundBase previousRound = round.GetPreviousRound();

            previousRound.Should().BeNull();
        }

        [Fact]
        public void FetchingPreviousRoundFromSecondRoundYieldsFirstRound()
        {
            RoundBase firstRound = tournament.AddRoundRobinRound();
            RoundBase secondRound = tournament.AddRoundRobinRound();

            RoundBase previousRound = secondRound.GetPreviousRound();

            previousRound.Should().Be(firstRound);
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
