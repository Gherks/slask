using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.Bases;
using Slask.Domain.Utilities;
using System;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests.RoundTests
{
    [Binding, Scope(Feature = "Round")]
    public class RoundSteps : RoundStepDefinitions
    {

    }

    public class RoundStepDefinitions : TournamentStepDefinitions
    {
        [When(@"players per group count in round (.*) is set to (.*)")]
        public void WhenPlayersPerGroupCountInRoundIsSetTo(int roundIndex, int playersPerGroupCount)
        {
            RoundBase round = createdRounds[roundIndex];

            round.SetPlayersPerGroupCount(playersPerGroupCount);
        }

        [When(@"best of in round (.*) is set to (.*)")]
        public void WhenAdvancingPlayersPerGroupCountInRoundIsSetTo(int roundIndex, int bestOf)
        {
            RoundBase round = createdRounds[roundIndex];

            round.SetBestOf(bestOf);
        }

        [When(@"advancing per group count in round (.*) is set to (.*)")]
        public void WhenAdvancingPerGroupCountInRoundIsSetTo(int roundIndex, int playersPerGroupCount)
        {
            RoundBase round = createdRounds[roundIndex];

            round.SetAdvancingPerGroupCount(playersPerGroupCount);
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

            RoundBase round = fetchedRounds[roundIndex];

            for (int index = 0; index < table.Rows.Count; ++index)
            {
                ParseRoundTable(table.Rows[index], out string roundType, out string name, out int bestOf, out int advancingCount, out int playersPerGroupCount);

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

        [Then(@"players per group count in round (.*) should be (.*)")]
        public void PlayersPerGroupCountInRoundIsSetTo(int roundIndex, int playersPerGroupCount)
        {
            RoundBase round = createdRounds[roundIndex];

            round.PlayersPerGroupCount.Should().Be(playersPerGroupCount);
        }

        [Then(@"best of in round (.*) should be (.*)")]
        public void BestOfInRoundIsSetTo(int roundIndex, int bestOf)
        {
            RoundBase round = createdRounds[roundIndex];

            round.BestOf.Should().Be(bestOf);
        }

        [Then(@"advancing per group count in round (.*) should be (.*)")]
        public void AdvancingPerGroupCountInRoundIsSetTo(int roundIndex, int advancingPerGrouCount)
        {
            RoundBase round = createdRounds[roundIndex];

            round.AdvancingPerGroupCount.Should().Be(advancingPerGrouCount);
        }

        [Then(@"play state of round (.*) is set to ""(.*)""")]
        public void ThenPlayStateOfRoundIsSetTo(int roundIndex, string playStateString)
        {
            RoundBase round = createdRounds[roundIndex];

            PlayState playState = ParsePlayStateString(playStateString);

            round.GetPlayState().Should().Be(playState);
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
