using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.SpecFlow.IntegrationTests.ServiceTests;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests
{
    [Binding, Scope(Feature = "Round")]
    public class RoundSteps : RoundStepDefinitions
    {

    }

    public class RoundStepDefinitions : TournamentServiceStepDefinitions
    {
        protected readonly List<Round> createdRounds;
        protected readonly List<Round> fetchedRounds;

        public RoundStepDefinitions()
        {
            createdRounds = new List<Round>();
            fetchedRounds = new List<Round>();
        }

        [Given(@"a tournament creates rounds")]
        [When(@"a tournament creates rounds")]
        public void GivenATournamentCreatesARound(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            Tournament tournament = GivenATournament();

            foreach(TableRow row in table.Rows)
            {
                if (ParseRoundTable(row, out RoundType type, out string name, out int bestOf, out int advancingAmount))
                {
                    createdRounds.Add(tournament.AddRound(type, name, bestOf, advancingAmount));
                }
            }
        }

        [When(@"created round (.*) fetches previous round")]
        public void WhenCreatedRoundFetchesPreviousRound(int roundIndex)
        {
            fetchedRounds.Add(createdRounds[roundIndex].GetPreviousRound());
        }

        [Then(@"created round (.*) in tournament should be valid with values:")]
        public void ThenCreatedRoundInTournamentShouldBeValidWithValues(int roundIndex, Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));

            }
            if (ParseRoundTable(table.Rows[0], out RoundType type, out string name, out int bestOf, out int advancingAmount))
            {
                CheckRoundValidity(createdRounds[roundIndex], type, name, bestOf, advancingAmount);
            }
        }

        [Then(@"created round (.*) in tournament should be invalid")]
        public void ThenCreatedRoundInTournamentShouldBeInvalid(int roundIndex)
        {
            Round round = createdRounds[roundIndex];
            round.Should().BeNull();
        }

        [Then(@"created rounds (.*) to (.*) in tournament should be invalid")]
        public void ThenCreatedRoundsToInTournamentShouldBeInvalid(int startIndex, int endIndex)
        {
            for (int roundIndex = startIndex; roundIndex < endIndex; ++roundIndex)
            {
                Round round = createdRounds[roundIndex];
                round.Should().BeNull();
            }
        }

        [Then(@"fetched round (.*) in tournament should be valid with values:")]
        public void ThenFetchedRoundInTournamentShouldBeValidWithValues(int roundIndex, Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (ParseRoundTable(table.Rows[0], out RoundType type, out string name, out int bestOf, out int advancingAmount))
            {
                CheckRoundValidity(fetchedRounds[roundIndex], type, name, bestOf, advancingAmount);
            }
        }

        [Then(@"fetched round (.*) in tournament should be invalid")]
        public void ThenFetchedRoundInTournamentShouldBeInvalid(int roundIndex)
        {
            fetchedRounds[roundIndex].Should().BeNull();
        }

        protected static void CheckRoundValidity(Round round, RoundType type, string correctName, int bestOf, int advancingAmount)
        {
            if (round == null)
            {
                throw new ArgumentNullException(nameof(round));
            }

            round.Should().NotBeNull();
            round.Id.Should().NotBeEmpty();
            round.Type.Should().Be(type);
            round.Name.Should().Be(correctName);
            round.BestOf.Should().Be(bestOf);
            round.AdvancingPerGroupAmount.Should().Be(advancingAmount);
            round.Groups.Should().BeEmpty();
            round.TournamentId.Should().NotBeEmpty();
            round.Tournament.Should().NotBeNull();
        }

        protected static bool ParseRoundTable(TableRow row, out RoundType type, out string name, out int bestOf, out int advancingAmount)
        {
            if (ParseRoundType(row["Round type"], out type) &&
                int.TryParse(row["Best of"], out bestOf) &&
                int.TryParse(row["Advancing amount"], out advancingAmount))
            {
                name = row["Round name"];
                return true;
            }
            string argumentString = string.Format("Round type: {0}, Round name: {1},  Best of {2}, Advancing amount {3}", row["Round type"], row["Round name"], row["Best of"], row["Advancing amount"]);
            throw new Exception("Uknown Round Specflow table row given - " + argumentString);
        }

        protected static bool ParseRoundType(string type, out RoundType outType)
        {
            type = StringUtility.ToUpperNoSpaces(type);

            if (type.Contains("BRACKET", StringComparison.CurrentCulture))
            {
                outType = RoundType.Bracket;
                return true;
            }
            else if (type.Contains("DUALTOURNAMENT", StringComparison.CurrentCulture))
            {
                outType = RoundType.DualTournament;
                return true;
            }
            else if (type.Contains("ROUNDROBIN", StringComparison.CurrentCulture))
            {
                outType = RoundType.Bracket;
                return true;
            }
            else
            {
                throw new Exception("Uknown round type given: " + type);
            }
        }

        private Tournament GivenATournament()
        {
            return GivenATournamentNamedHasBeenCreated("GSL 2019");
        }
    }
}
