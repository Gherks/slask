using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.SpecFlow.IntegrationTests.ServiceTests;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests.RoundTests
{
    [Binding, Scope(Feature = "Round")]
    public class RoundSteps : RoundStepDefinitions
    {

    }

    public class RoundStepDefinitions : TournamentServiceStepDefinitions
    {
        protected readonly List<RoundBase> createdRounds;
        protected readonly List<RoundBase> fetchedRounds;

        public RoundStepDefinitions()
        {
            createdRounds = new List<RoundBase>();
            fetchedRounds = new List<RoundBase>();
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

            foreach (TableRow row in table.Rows)
            {
                ParseRoundTable(row, out string type, out string name, out int bestOf, out int advancingAmount);

                RoundBase round = null;
                if (type.Length > 0)
                {
                    type = GetRoundType(type);

                    if (type == "BRACKET")
                    {
                        round = tournament.AddBracketRound(name, bestOf);
                    }
                    else if (type == "DUALTOURNAMENT")
                    {
                        round = tournament.AddDualTournamentRound(name, bestOf);
                    }
                    else if (type == "ROUNDROBIN")
                    {
                        round = tournament.AddRoundRobinRound(name, bestOf, advancingAmount);
                    }
                }

                createdRounds.Add(round);
            }
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
                ParseRoundTable(row, out _, out string name, out int bestOf, out int advancingAmount);

                RoundBase createdRound = createdRounds[rowIndex];
                CheckRoundValidity(createdRound, name, bestOf);

                if (createdRound is BracketRound bracketRound)
                {
                    bracketRound.AdvancingPerGroupAmount.Should().Be(1);
                }
                else if (createdRound is DualTournamentRound dualTournamentRound)
                {
                    dualTournamentRound.AdvancingPerGroupAmount.Should().Be(2);
                }
                else if (createdRound is RoundRobinRound roundRobinRound)
                {
                    roundRobinRound.AdvancingPerGroupAmount.Should().Be(advancingAmount);
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

            ParseRoundTable(table.Rows[0], out _, out string name, out int bestOf, out int advancingAmount);

            RoundBase fetchedRound = fetchedRounds[roundIndex];
            CheckRoundValidity(fetchedRound, name, bestOf);

            if (fetchedRound is BracketRound bracketRound)
            {
                bracketRound.AdvancingPerGroupAmount.Should().Be(1);
            }
            else if (fetchedRound is DualTournamentRound dualTournamentRound)
            {
                dualTournamentRound.AdvancingPerGroupAmount.Should().Be(2);
            }
            else if (fetchedRound is RoundRobinRound roundRobinRound)
            {
                roundRobinRound.AdvancingPerGroupAmount.Should().Be(advancingAmount);
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
            round.Groups.Should().BeEmpty();
            round.TournamentId.Should().NotBeEmpty();
            round.Tournament.Should().NotBeNull();
        }

        protected static void ParseRoundTable(TableRow row, out string typeName, out string name, out int bestOf, out int advancingAmount)
        {
            typeName = "";
            name = "";
            bestOf = 1;
            advancingAmount = 1;

            if (row.ContainsKey("Round type"))
            {
                typeName = row["Round type"];
            }

            if (row.ContainsKey("Round name"))
            {
                name = row["Round name"];
            }

            if (row.ContainsKey("Best of"))
            {
                int.TryParse(row["Best of"], out bestOf);
            }

            if (row.ContainsKey("Advancing amount"))
            {
                int.TryParse(row["Advancing amount"], out advancingAmount);
            }
        }

        protected static string GetRoundType(string type)
        {
            type = StringUtility.ToUpperNoSpaces(type);

            if (type.Contains("BRACKET", StringComparison.CurrentCulture))
            {
                return "BRACKET";
            }
            else if (type.Contains("DUALTOURNAMENT", StringComparison.CurrentCulture))
            {
                return "DUALTOURNAMENT";
            }
            else if (type.Contains("ROUNDROBIN", StringComparison.CurrentCulture))
            {
                return "ROUNDROBIN";
            }

            return "";
        }

        private Tournament GivenATournament()
        {
            return GivenATournamentNamedHasBeenCreated("GSL 2019");
        }
    }
}
