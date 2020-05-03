using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Groups.Bases;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.Bases;
using Slask.Domain.Utilities;
using Slask.SpecFlow.IntegrationTests.ServiceTests;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests.RoundTests
{
    public class RoundStepDefinitions : TournamentStepDefinitions
    {
        [When(@"players per group count in round (.*) is set to (.*)")]
        public void WhenPlayersPerGroupCountInRoundIsSetTo(int roundIndex, int playersPerGroupCount)
        {
            ResizableRound round = createdRounds[roundIndex] as ResizableRound;

            round.SetPlayersPerGroupCount(playersPerGroupCount);
        }

        [When(@"created round (.*) fetches previous round")]
        public void WhenCreatedRoundFetchesPreviousRound(int roundIndex)
        {
            fetchedRounds.Add(createdRounds[roundIndex].GetPreviousRound());
        }

        [Then(@"created rounds in tournament should be valid with values:")]
        public void ThenCreatedRoundInTournamentShouldBeValidWithValues(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            for (int rowIndex = 0; rowIndex < table.Rows.Count; ++rowIndex)
            {
                TableRow row = table.Rows[rowIndex];
                ParseRoundTable(row, out string roundType, out string name, out int bestOf, out int advancingCount, out int playersPerGroupCount);

                RoundBase createdRound = createdRounds[rowIndex];
                CheckRoundValidity(createdRound, name, bestOf);

                if (createdRound is BracketRound bracketRound)
                {
                    roundType.Should().Be("Bracket");
                    bracketRound.AdvancingPerGroupCount.Should().Be(1);
                    bracketRound.PlayersPerGroupCount.Should().Be(playersPerGroupCount);
                }
                else if (createdRound is DualTournamentRound dualTournamentRound)
                {
                    roundType.Should().Be("Dual tournament");
                    dualTournamentRound.AdvancingPerGroupCount.Should().Be(2);
                    dualTournamentRound.PlayersPerGroupCount.Should().Be(4);
                }
                else if (createdRound is RoundRobinRound roundRobinRound)
                {
                    roundType.Should().Be("Round robin");
                    roundRobinRound.AdvancingPerGroupCount.Should().Be(advancingCount);
                    roundRobinRound.PlayersPerGroupCount.Should().Be(playersPerGroupCount);
                }
            }
        }

        [Then(@"created round (.*) in tournament should be invalid")]
        public void ThenCreatedRoundInTournamentShouldBeInvalid(int roundIndex)
        {
            RoundBase round = createdRounds[roundIndex];
            round.Should().BeNull();
        }

        [Then(@"fetched round (.*) in tournament should be valid with values:")]
        public void ThenFetchedRoundInTournamentShouldBeValidWithValues(int roundIndex, Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            ParseRoundTable(table.Rows[0], out string roundType, out string name, out int bestOf, out int advancingCount, out int playersPerGroupCount);

            RoundBase fetchedRound = fetchedRounds[roundIndex];
            CheckRoundValidity(fetchedRound, name, bestOf);

            if (fetchedRound is BracketRound bracketRound)
            {
                roundType.Should().Be("Bracket");
                bracketRound.AdvancingPerGroupCount.Should().Be(1);
                bracketRound.PlayersPerGroupCount.Should().Be(playersPerGroupCount);
            }
            else if (fetchedRound is DualTournamentRound dualTournamentRound)
            {
                roundType.Should().Be("Dual tournament");
                dualTournamentRound.AdvancingPerGroupCount.Should().Be(2);
                dualTournamentRound.PlayersPerGroupCount.Should().Be(4);
            }
            else if (fetchedRound is RoundRobinRound roundRobinRound)
            {
                roundType.Should().Be("Round robin");
                roundRobinRound.AdvancingPerGroupCount.Should().Be(advancingCount);
                roundRobinRound.PlayersPerGroupCount.Should().Be(playersPerGroupCount);
            }
        }

        [Then(@"fetched round (.*) in tournament should be invalid")]
        public void ThenFetchedRoundInTournamentShouldBeInvalid(int roundIndex)
        {
            fetchedRounds[roundIndex].Should().BeNull();
        }

        protected static void CheckRoundValidity(RoundBase round, string correctName, int bestOf)
        {
            if (round == null)
            {
                throw new ArgumentNullException(nameof(round));
            }

            round.Should().NotBeNull();
            round.Id.Should().NotBeEmpty();
            round.Name.Should().Be(correctName);
            round.BestOf.Should().Be(bestOf);
            round.Groups.Should().HaveCount(1);
            round.TournamentId.Should().NotBeEmpty();
            round.Tournament.Should().NotBeNull();
        }
    }
}
