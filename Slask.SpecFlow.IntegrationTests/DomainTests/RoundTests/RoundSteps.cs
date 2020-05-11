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
        [When(@"advancing players per group count in round (.*) is set to (.*)")]
        public void WhenAdvancingPlayersPerGroupCountInRoundIsSetTo(int roundIndex, int playersPerGroupCount)
        {
            RoundBase round = createdRounds[roundIndex] as RoundBase;

            round.SetAdvancingPerGroupCount(playersPerGroupCount);
        }

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


        [Then(@"created round (.*) in tournament should be invalid")]
        public void ThenCreatedRoundInTournamentShouldBeInvalid(int roundIndex)
        {
            RoundBase round = createdRounds[roundIndex];
            round.Should().BeNull();
        }

        [Then(@"created rounds in tournament (.*) should be valid with values")]
        public void CreatedRoundsInTournamentShouldBeValidWithValues(int tournamentIndex, Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            Tournament tournament = createdTournaments[tournamentIndex];

            for (int index = 0; index < table.Rows.Count; ++index)
            {
                ParseRoundTable(table.Rows[index], out string roundType, out string name, out int bestOf, out int advancingCount, out int playersPerGroupCount);

                RoundBase round = tournament.Rounds[index];

                if (round is BracketRound bracketRound)
                {
                    roundType.Should().Be("Bracket");
                }
                else if (round is DualTournamentRound dualTournamentRound)
                {
                    roundType.Should().Be("Dual tournament");
                }
                else if (round is RoundRobinRound roundRobinRound)
                {
                    roundType.Should().Be("Round robin");
                }

                CheckRoundValidity(round, name, bestOf, advancingCount, playersPerGroupCount);
            }
        }

        [Then(@"fetched round (.*) in tournament should be valid with values")]
        public void ThenFetchedRoundInTournamentShouldBeValidWithValues(int roundIndex, Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            ParseRoundTable(table.Rows[0], out string roundType, out string name, out int bestOf, out int advancingCount, out int playersPerGroupCount);

            RoundBase fetchedRound = fetchedRounds[roundIndex];

            if (fetchedRound is BracketRound bracketRound)
            {
                roundType.Should().Be("Bracket");
            }
            else if (fetchedRound is DualTournamentRound dualTournamentRound)
            {
                roundType.Should().Be("Dual tournament");
            }
            else if (fetchedRound is RoundRobinRound roundRobinRound)
            {
                roundType.Should().Be("Round robin");
            }

            CheckRoundValidity(fetchedRound, name, bestOf, advancingCount, playersPerGroupCount);
        }

        [Then(@"fetched round (.*) in tournament should be invalid")]
        public void ThenFetchedRoundInTournamentShouldBeInvalid(int roundIndex)
        {
            fetchedRounds[roundIndex].Should().BeNull();
        }

        protected static void CheckRoundValidity(RoundBase round, string correctName, int bestOf, int advancingCount, int playersPerGroupCount)
        {
            if (round == null)
            {
                throw new ArgumentNullException(nameof(round));
            }

            round.Should().NotBeNull();
            round.Id.Should().NotBeEmpty();
            round.Name.Should().Be(correctName);
            round.PlayersPerGroupCount.Should().Be(playersPerGroupCount);
            round.BestOf.Should().Be(bestOf);
            round.AdvancingPerGroupCount.Should().Be(advancingCount);
            round.Groups.Should().HaveCount(1);
            round.TournamentId.Should().NotBeEmpty();
            round.Tournament.Should().NotBeNull();
        }
    }
}
