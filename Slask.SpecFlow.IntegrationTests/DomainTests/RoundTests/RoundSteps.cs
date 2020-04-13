using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using Slask.SpecFlow.IntegrationTests.ServiceTests;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests.RoundTests
{
    [Binding, Scope(Feature = "Round")]
    public class RoundSteps : RoundStepDefinitions
    {

    }

    public class RoundStepDefinitions : TournamentServiceStepDefinitions
    {
        private Random randomizer;

        [Given(@"created tournament (.*) adds rounds")]
        [When(@"created tournament (.*) adds rounds")]
        public void GivenCreatedTournamentAddsRounds(int tournamentIndex, Table table)
        {
            if (createdTournaments.Count <= tournamentIndex)
            {
                throw new IndexOutOfRangeException("Given created tournament index is out of bounds");
            }

            Tournament tournament = createdTournaments[tournamentIndex];

            foreach (TableRow row in table.Rows)
            {
                ParseRoundTable(row, out string type, out string name, out int bestOf, out int advancingAmount);

                if (type.Length > 0)
                {
                    type = GetRoundType(type);

                    if (type == "BRACKET")
                    {
                        createdRounds.Add(tournament.AddBracketRound(name, bestOf));
                    }
                    else if (type == "DUALTOURNAMENT")
                    {
                        createdRounds.Add(tournament.AddDualTournamentRound(name, bestOf));
                    }
                    else if (type == "ROUNDROBIN")
                    {
                        createdRounds.Add(tournament.AddRoundRobinRound(name, bestOf, advancingAmount));
                    }
                }
            }
        }

        [Given(@"players ""(.*)"" is registered to round (.*)")]
        [When(@"players ""(.*)"" is registered to round (.*)")]
        public void GivenPlayersIsRegisteredToTournament(string commaSeparatedPlayerNames, int roundIndex)
        {
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");
            RoundBase round = createdRounds[roundIndex];

            foreach (string playerName in playerNames)
            {
                round.RegisterPlayerReference(playerName);
            }
        }

        [When(@"created round (.*) fetches previous round")]
        public void WhenCreatedRoundFetchesPreviousRound(int roundIndex)
        {
            fetchedRounds.Add(createdRounds[roundIndex].GetPreviousRound());
        }

        [Given(@"created round (.*) adds (.*) groups")]
        [When(@"created round (.*) adds (.*) groups")]
        public void GivenCreatedRoundAddsGroups(int roundIndex, int groupCount)
        {
            //if (createdRounds.Count <= roundIndex)
            //{
            //    throw new IndexOutOfRangeException("Given created round index is out of bounds");
            //}

            //for (int counter = 0; counter < groupCount; ++counter)
            //{
            //    createdGroups.Add(createdRounds[roundIndex].AddGroup());
            //}
        }

        [Given(@"created groups within created tournament is played out and betted on")]
        [When(@"created groups within created tournament is played out and betted on")]
        public void GivenCreatedGroupsWithinCreatedTournamentIsPlayedOutAndBettedOn(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            foreach (TableRow row in table.Rows)
            {
                ParseTargetGroupToPlaceBets(row, out int tournamentIndex, out int roundIndex, out int groupIndex);

                bool tournamentIndexIsValid = createdTournaments.Count > tournamentIndex;
                bool roundIndexIsValid = createdTournaments[tournamentIndex].Rounds.Count > roundIndex;
                bool groupIndexIsValid = createdTournaments[tournamentIndex].Rounds[roundIndex].Groups.Count > groupIndex;

                if (!tournamentIndexIsValid || !roundIndexIsValid || !groupIndexIsValid)
                {
                    throw new IndexOutOfRangeException("Tournament, round, or group with given index does not exist");
                }

                SystemTimeMocker.Reset();
                randomizer = new Random(133742069);

                Tournament tournament = createdTournaments[tournamentIndex];
                RoundBase round = tournament.Rounds[roundIndex];
                GroupBase group = round.Groups[groupIndex];

                while (group.GetPlayState() != PlayState.IsFinished)
                {
                    PlaceBetsOnAvailableMatchesInGroup(tournament.Betters, group);

                    foreach (Match match in group.Matches)
                    {
                        if (match.IsReady() && match.GetPlayState() == PlayState.NotBegun)
                        {
                            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
                            break;
                        }
                    }

                    PlayAvailableMatches(group);
                }
            }
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
                ParseRoundTable(row, out string roundType, out string name, out int bestOf, out int advancingAmount);

                RoundBase createdRound = createdRounds[rowIndex];
                CheckRoundValidity(createdRound, name, bestOf);

                if (createdRound is BracketRound bracketRound)
                {
                    roundType.Should().Be("Bracket");
                    bracketRound.AdvancingPerGroupCount.Should().Be(1);
                }
                else if (createdRound is DualTournamentRound dualTournamentRound)
                {
                    roundType.Should().Be("Dual tournament");
                    dualTournamentRound.AdvancingPerGroupCount.Should().Be(2);
                }
                else if (createdRound is RoundRobinRound roundRobinRound)
                {
                    roundType.Should().Be("Round robin");
                    roundRobinRound.AdvancingPerGroupCount.Should().Be(advancingAmount);
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

            ParseRoundTable(table.Rows[0], out string roundType, out string name, out int bestOf, out int advancingAmount);

            RoundBase fetchedRound = fetchedRounds[roundIndex];
            CheckRoundValidity(fetchedRound, name, bestOf);

            if (fetchedRound is BracketRound bracketRound)
            {
                roundType.Should().Be("Bracket");
                bracketRound.AdvancingPerGroupCount.Should().Be(1);
            }
            else if (fetchedRound is DualTournamentRound dualTournamentRound)
            {
                roundType.Should().Be("Dual tournament");
                dualTournamentRound.AdvancingPerGroupCount.Should().Be(2);
            }
            else if (fetchedRound is RoundRobinRound roundRobinRound)
            {
                roundType.Should().Be("Round robin");
                roundRobinRound.AdvancingPerGroupCount.Should().Be(advancingAmount);
            }
        }

        [Then(@"fetched round (.*) in tournament should be invalid")]
        public void ThenFetchedRoundInTournamentShouldBeInvalid(int roundIndex)
        {
            fetchedRounds[roundIndex].Should().BeNull();
        }

        [Then(@"registered players in tournament (.*) should be exactly ""(.*)""")]
        public void ThenRegisteredPlayersInTournamentShouldBeExactly(int tournamentIndex, string commaSeparatedPlayerNames)
        {
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");
            RoundBase round = createdTournaments[tournamentIndex].Rounds.First();

            round.PlayerReferences.Should().HaveCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                round.PlayerReferences.SingleOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
            }
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

        protected static void ParseTargetGroupToPlaceBets(TableRow row, out int tournamentIndex, out int roundIndex, out int groupIndex)
        {
            tournamentIndex = 0;
            roundIndex = 0;
            groupIndex = 0;

            if (row.ContainsKey("Created tournament index"))
            {
                int.TryParse(row["Created tournament index"], out tournamentIndex);
            }

            if (row.ContainsKey("Round index"))
            {
                int.TryParse(row["Round index"], out roundIndex);
            }

            if (row.ContainsKey("Group index"))
            {
                int.TryParse(row["Group index"], out groupIndex);
            }
        }

        protected void PlayAvailableMatches(GroupBase group)
        {
            int winningScore = (int)Math.Ceiling(group.Round.BestOf / 2.0);

            foreach (Match match in group.Matches)
            {
                bool matchShouldHaveStarted = match.StartDateTime < SystemTime.Now;
                bool matchIsNotFinished = match.GetPlayState() != PlayState.IsFinished;

                if (matchShouldHaveStarted && matchIsNotFinished)
                {
                    bool increasePlayer1Score = randomizer.Next(2) == 0;

                    if (increasePlayer1Score)
                    {
                        match.Player1.IncreaseScore(winningScore);
                    }
                    else
                    {
                        match.Player2.IncreaseScore(winningScore);
                    }
                }
            }
        }

        private void PlaceBetsOnAvailableMatchesInGroup(List<Better> betters, GroupBase group)
        {
            Random random = new Random(133742069);
            int matchCounter = 0;

            foreach (Domain.Match match in group.Matches)
            {
                if (match.GetPlayState() != PlayState.NotBegun)
                {
                    continue;
                }

                Better better = betters[0];

                if (matchCounter % 4 != 0)
                {
                    better.PlaceMatchBet(match, match.Player1);
                }

                for (int betterIndex = 1; betterIndex < betters.Count; ++betterIndex)
                {
                    Player player = random.Next(2) == 0 ? match.Player1 : match.Player2;

                    better = betters[betterIndex];
                    better.PlaceMatchBet(match, player);
                }

                matchCounter++;
            }
        }
    }
}
