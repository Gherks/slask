using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups.Bases;
using Slask.Domain.Rounds;
using Slask.Domain.Rounds.Bases;
using Slask.Domain.Utilities;
using Slask.SpecFlow.IntegrationTests.ServiceTests;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests
{
    [Binding, Scope(Feature = "Tournament")]
    public class TournamentSteps : TournamentStepDefinitions
    {

    }

    public class TournamentStepDefinitions : TournamentServiceStepDefinitions
    {
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
                ParseRoundTable(row, out string type, out string name, out int bestOf, out int advancingCount, out int playersPerGroupCount);

                if (type.Length > 0)
                {
                    type = GetRoundType(type);
                    RoundBase round = null;

                    if (type == "BRACKET")
                    {
                        round = tournament.AddBracketRound();

                        round.RenameTo(name);
                        round.SetBestOf(bestOf);
                        (round as BracketRound).SetPlayersPerGroupCount(playersPerGroupCount);
                    }
                    else if (type == "DUALTOURNAMENT")
                    {
                        round = tournament.AddDualTournamentRound();

                        round.RenameTo(name);
                        round.SetBestOf(bestOf);
                    }
                    else if (type == "ROUNDROBIN")
                    {
                        round = tournament.AddRoundRobinRound();

                        round.RenameTo(name);
                        round.SetBestOf(bestOf);
                        round.SetAdvancingPerGroupCount(advancingCount);
                        (round as RoundRobinRound).SetPlayersPerGroupCount(playersPerGroupCount);
                    }

                    createdRounds.Add(round);
                    bool addedValidRound = round != null;

                    if (addedValidRound)
                    {
                        createdGroups.AddRange(round.Groups);
                    }
                }
            }
        }

        [Given(@"round (.*) is removed from tournament (.*)")]
        [When(@"round (.*) is removed from tournament (.*)")]
        public void WhenRoundIsRemovedFromTournament(int roundIndex, int tournamentIndex)
        {
            RoundBase round = createdRounds[roundIndex];
            Tournament tournament = createdTournaments[tournamentIndex];

            tournament.RemoveRound(round);

            createdRounds.Clear();
            createdRounds.AddRange(tournament.Rounds);
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

            round = round.Tournament.GetFirstRound();

            createdGroups.Clear();
            while (round != null)
            {
                createdGroups.AddRange(round.Groups);
                round = round.GetNextRound();
            }
        }

        [Given(@"players ""(.*)"" is excluded from round (.*)")]
        [When(@"players ""(.*)"" is excluded from round (.*)")]
        public void GivenPlayersIsExcludedFromRound(string commaSeparatedPlayerNames, int roundIndex)
        {
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");
            RoundBase round = createdRounds[roundIndex];

            foreach (string playerName in playerNames)
            {
                round.ExcludePlayerReference(playerName);
            }
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
                Tournament tournament = createdTournaments[tournamentIndex];
                RoundBase round = tournament.Rounds[roundIndex];
                GroupBase group = round.Groups[groupIndex];

                while (group.GetPlayState() != PlayState.Finished)
                {
                    bool tournamentHasBetters = tournament.Betters.Count > 0;
                    if (tournamentHasBetters)
                    {
                        PlaceBetsOnAvailableMatchesInGroup(tournament.Betters, group);
                    }

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

        [Then(@"participating players in round (.*) should be exactly ""(.*)""")]
        public void ThenParticipatingPlayersInCreatedGroupShouldBeMappedAccordingly(int roundIndex, string commaSeparatedPlayerNames)
        {
            RoundBase round = createdRounds[roundIndex];
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");

            round.PlayerReferences.Should().HaveCount(playerNames.Count);

            for(int index = 0; index < round.PlayerReferences.Count; ++index)
            {
                round.PlayerReferences[index].Name.Should().Be(playerNames[index]);
            }
        }

        [Then(@"tournament (.*) contains (.*) rounds")]
        public void ThenTournamentContainsRounds(int tournamentIndex, int roundCount)
        {
            Tournament tournament = createdTournaments[tournamentIndex];
            tournament.Rounds.Should().HaveCount(roundCount);
        }

        [Then(@"play state of tournament (.*) is set to ""(.*)""")]
        public void ThenPlayStateOfTournamentIsSetTo(int tournamentIndex, string playStateString)
        {
            Tournament tournament = createdTournaments[tournamentIndex];

            PlayState playState = ParsePlayStateString(playStateString);

            tournament.GetPlayState().Should().Be(playState);
        }

        protected static void ParseRoundTable(TableRow row, out string typeName, out string name, out int bestOf, out int advancingCount, out int playersPerGroupCount)
        {
            typeName = "";
            name = "";
            bestOf = 1;
            advancingCount = 1;
            playersPerGroupCount = 2;

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

            if (row.ContainsKey("Advancing per group count"))
            {
                int.TryParse(row["Advancing per group count"], out advancingCount);
            }

            if (row.ContainsKey("Players per group count"))
            {
                int.TryParse(row["Players per group count"], out playersPerGroupCount);
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

            if (row.ContainsKey("Tournament index"))
            {
                int.TryParse(row["Tournament index"], out tournamentIndex);
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
                bool matchIsNotFinished = match.GetPlayState() != PlayState.Finished;

                if (matchShouldHaveStarted && matchIsNotFinished)
                {
                    // Give points to player with name that precedes the other alphabetically
                    bool increasePlayer1Score = match.Player1.Name.CompareTo(match.Player2.Name) <= 0;

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

        protected PlayState ParsePlayStateString(string playStateString)
        {
            playStateString = StringUtility.ToUpperNoSpaces(playStateString);

            if (playStateString.Contains("NOTBEGUN", StringComparison.CurrentCulture))
            {
                return PlayState.NotBegun;
            }
            else if (playStateString.Contains("ONGOING", StringComparison.CurrentCulture))
            {
                return PlayState.Ongoing;
            }
            else if (playStateString.Contains("FINISHED", StringComparison.CurrentCulture))
            {
                return PlayState.Finished;
            }

            throw new NotImplementedException();
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
