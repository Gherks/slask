using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.Bases;
using System;
using System.Collections.Generic;
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
            RoundRobinRound firstRound = RoundRobinRound.Create(tournament);
            RoundRobinRound secondRound = RoundRobinRound.Create(tournament);
            RoundRobinRound thirdRound = RoundRobinRound.Create(tournament);
            RoundRobinRound fourthRound = RoundRobinRound.Create(tournament);
            RoundRobinRound fifthRound = RoundRobinRound.Create(tournament);

            firstRound.Name.Should().Be("Round A");
            secondRound.Name.Should().Be("Round B");
            thirdRound.Name.Should().Be("Round C");
            fourthRound.Name.Should().Be("Round D");
            fifthRound.Name.Should().Be("Round E");
        }

        [Fact]
        public void CanRenameRound()
        {
            string newName = "New Round Name";

            RoundRobinRound round = RoundRobinRound.Create(tournament);
            round.RenameTo(newName);

            round.Name.Should().Be(newName);
        }

        [Fact]
        public void CannotRenameRoundToTheSameAsOtherRound()
        {
            RoundRobinRound firstRound = RoundRobinRound.Create(tournament);
            RoundRobinRound secondRound = RoundRobinRound.Create(tournament);

            string newName = "New Round Name";
            string initialRoundName = secondRound.Name;

            firstRound.RenameTo(newName);
            secondRound.RenameTo(newName);

            firstRound.Name.Should().Be(newName);
            secondRound.Name.Should().Be(initialRoundName);
        }

        [Fact]
        public void CannotCreateRoundWithEvenOrZeroBestOfs()
        {
            for (int bestOf = 0; bestOf < 32; ++bestOf)
            {
                RoundRobinRound round = RoundRobinRound.Create(tournament, bestOf: bestOf);
                bool bestOfIsEven = bestOf % 2 == 0;

                if (bestOfIsEven)
                {
                    round.Should().BeNull();
                }
                else
                {
                    round.Should().NotBeNull();
                }
            }
        }

        [Fact]
        public void CannotCreateRoundWithZeroOrLessAdvancingPlayers()
        {
            RoundRobinRound firstRound = RoundRobinRound.Create(tournament, advancingPerGroupCount: 0);
            RoundRobinRound secondRound = RoundRobinRound.Create(tournament, advancingPerGroupCount: -1);

            firstRound.Should().BeNull();
            secondRound.Should().BeNull();
        }

        [Fact]
        public void FetchingPreviousRoundFromFirstRoundYieldsNull()
        {
            RoundRobinRound round = RoundRobinRound.Create(tournament);

            RoundBase previousRound = round.GetPreviousRound();

            previousRound.Should().BeNull();
        }

        [Fact]
        public void FetchingPreviousRoundFromSecondRoundYieldsFirstRound()
        {
            RoundRobinRound firstRound = RoundRobinRound.Create(tournament, name: "First round");
            RoundRobinRound secondRound = RoundRobinRound.Create(tournament, name: "Second round");

            RoundBase previousRound = secondRound.GetPreviousRound();

            previousRound.Should().Be(firstRound);
        }

        [Fact]
        public void CanRegisterPlayerReferencesToFirstRound()
        {
            string playerName = "Maru";

            RoundRobinRound round = RoundRobinRound.Create(tournament);

            PlayerReference playerReference = round.RegisterPlayerReference(playerName);

            playerReference.Id.Should().NotBeEmpty();
            playerReference.Name.Should().Be(playerName);
            playerReference.TournamentId.Should().Be(round.TournamentId);
            playerReference.Tournament.Should().Be(round.Tournament);
        }

        [Fact]
        public void CannotRegisterPlayerReferencesToRoundsThatIsNotTheFirstOne()
        {
            string playerName = "Maru";
            int roundCount = 5;

            RoundRobinRound firstRound = RoundRobinRound.Create(tournament);

            for (int index = 1; index < roundCount; ++index)
            {
                RoundRobinRound round = RoundRobinRound.Create(tournament);
                PlayerReference playerReference = round.RegisterPlayerReference(playerName + index.ToString());

                playerReference.Should().BeNull();
                round.PlayerReferences.Should().BeEmpty();
            }
        }
    }
}
