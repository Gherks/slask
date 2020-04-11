using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using Slask.SpecFlow.IntegrationTests.DomainTests.RoundTests;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests.GroupTests
{
    [Binding, Scope(Feature = "Group")]
    public class GroupSteps : GroupStepDefinitions
    {
        [AfterScenario]
        public static void AfterScenario()
        {
            SystemTimeMocker.Reset();
        }
    }

    public class GroupStepDefinitions : RoundStepDefinitions
    {
        private Random randomizer;

        [Given(@"created round (.*) adds (.*) groups")]
        [When(@"created round (.*) adds (.*) groups")]
        public void GivenCreatedRoundAddsGroups(int roundIndex, int groupCount)
        {
            if (createdRounds.Count <= roundIndex)
            {
                throw new IndexOutOfRangeException("Given created round index is out of bounds");
            }

            for(int counter = 0; counter < groupCount; ++counter)
            {
                createdGroups.Add(createdRounds[roundIndex].AddGroup());
            }
        }

        [Given(@"players ""(.*)"" is added to created group (.*)")]
        [When(@"players ""(.*)"" is added to created group (.*)")]
        public void GivenPlayersIsAddedToCreatedGroup(string commaSeparatedPlayerNames, int groupIndex)
        {
            if (createdGroups.Count <= groupIndex)
            {
                throw new IndexOutOfRangeException("Given created group index is out of bounds");
            }

            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");
            GroupBase group = createdGroups[groupIndex];

            foreach (string playerName in playerNames)
            {
                group.AddNewPlayerReference(playerName);
            }
        }

        [Given(@"score is added to players in given matches in created groups")]
        [When(@"score is added to players in given matches in created groups")]
        public void WhenScoreIsAddedToPlayersInGivenMatchesInCreatedGroups(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            foreach (TableRow row in table.Rows)
            {
                ParseSoreAddedToMatchPlayer(row, out int createdGroupIndex, out int matchIndex, out string scoringPlayer, out int scoreAdded);

                GroupBase group = createdGroups[createdGroupIndex];
                Match match = group.Matches[matchIndex];

                SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);

                Player player = match.FindPlayer(scoringPlayer);

                if(player != null)
                {
                    player.IncreaseScore(scoreAdded);
                }
                else
                {
                    throw new Exception("Invalid player name in given match within given created group");
                }
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

        //[Given(@"betters has placed their bets on")]
        //public void GivenBettersHasPlacedTheirBetsOn(Table table)
        //{
        //    if (table == null)
        //    {
        //        throw new ArgumentNullException(nameof(table));
        //    }

        //    foreach (TableRow row in table.Rows)
        //    {
        //        ParseTargetGroupToPlaceBets(row, out int tournamentIndex, out int roundIndex, out int groupIndex);

        //        bool tournamentIndexIsValid = createdTournaments.Count > tournamentIndex;
        //        bool roundIndexIsValid = createdTournaments[tournamentIndex].Rounds.Count > roundIndex;
        //        bool groupIndexIsValid = createdTournaments[tournamentIndex].Rounds[roundIndex].Groups.Count > groupIndex;

        //        if (!tournamentIndexIsValid || !roundIndexIsValid || !groupIndexIsValid)
        //        {
        //            throw new IndexOutOfRangeException("Tournament, round, or group with given index does not exist");
        //        }

        //        Tournament tournament = createdTournaments[tournamentIndex];
        //        RoundBase round = tournament.Rounds[roundIndex];
        //        GroupBase group = round.Groups[groupIndex];

        //        Random random = new Random(133742069);
        //        int matchCounter = 0;

        //        foreach (Domain.Match match in group.Matches)
        //        {
        //            Better better = tournament.Betters[0];

        //            if (matchCounter % 4 != 0)
        //            {
        //                better.PlaceMatchBet(match, match.Player1);
        //            }

        //            for (int betterIndex = 1; betterIndex < tournament.Betters.Count; ++betterIndex)
        //            {
        //                Player player = random.Next(2) == 0 ? match.Player1 : match.Player2;

        //                better = tournament.Betters[betterIndex];
        //                better.PlaceMatchBet(match, player);
        //            }

        //            matchCounter++;
        //        }
        //    }
        //}

        [When(@"created rounds (.*) to (.*) creates (.*) groups each")]
        public void WhenCreatedRoundsToCreatesGroupsEach(int roundStartIndex, int roundEndIndex, int groupAmount)
        {
            for (int roundIndex = roundStartIndex; roundIndex < roundEndIndex; ++roundIndex)
            {
                for (int groupCounter = 0; groupCounter < groupAmount; ++groupCounter)
                {
                    createdGroups.Add(createdRounds[roundIndex].AddGroup());
                }
            }
        }

        [Then(@"created rounds (.*) to (.*) should contain (.*) groups each")]
        public void ThenCreatedRoundsToShouldContainGroupsEach(int roundStartIndex, int roundEndIndex, int groupAmount)
        {
            for (int roundIndex = roundStartIndex; roundIndex < roundEndIndex; ++roundIndex)
            {
                for (int groupCounter = 0; groupCounter < groupAmount; ++groupCounter)
                {
                    createdRounds[roundIndex].Groups.Should().HaveCount(groupAmount);
                }
            }
        }

        [Then(@"group (.*) should be valid of type ""(.*)""")]
        public void ThenGroupShouldBeValidOfType(int groupIndex, string roundType)
        {
            string type = GetRoundType(roundType);

            GroupBase group = createdGroups[groupIndex];

            if (type == "BRACKET")
            {
                CheckGroupValidity<BracketGroup>(group);
            }
            else if (type == "DUALTOURNAMENT")
            {
                CheckGroupValidity<DualTournamentGroup>(group);
            }
            else if (type == "ROUNDROBIN")
            {
                CheckGroupValidity<RoundRobinGroup>(group);
            }
        }

        [Then(@"minutes between matches in created group (.*) should be (.*)")]
        public void ThenMinutesBetweenMatchesInCreatedGroupShouldBe(int groupIndex, int expectedMinutes)
        {
            GroupBase group = createdGroups[groupIndex];

            group.Matches.Should().HaveCountGreaterOrEqualTo(2);

            for (int matchIndex = 1; matchIndex < group.Matches.Count; ++matchIndex)
            {
                Domain.Match previousMatch = group.Matches[matchIndex - 1];
                Domain.Match currentMatch = group.Matches[matchIndex];

                DateTime previousDateTime = previousMatch.StartDateTime;
                DateTime currentDateTime = currentMatch.StartDateTime;

                int minuteDifference = (int)currentDateTime.Subtract(previousDateTime).TotalMinutes;
                minuteDifference.Should().Be(expectedMinutes);
            }
        }

        [Then(@"advancing players in created group (.*) is exactly ""(.*)""")]
        public void ThenWinningPlayersInGroupIs(int groupIndex, string commaSeparatedPlayerNames)
        {
            GroupBase group = createdGroups[groupIndex];
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");

            List<PlayerReference> playerReferences = PlayerStandingsCalculator.GetAdvancingPlayers(group);

            playerReferences.Should().NotBeEmpty();
            playerReferences.Should().HaveCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                playerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
            }
        }


        [Then(@"participating players in created group (.*) should be mapped accordingly")]
        public void ThenparticipatingPlayersInCreatedGroupShouldBeMappedAccordingly(int createdGroupIndex, Table table)
        {
            GroupBase group = createdGroups[createdGroupIndex];

            foreach (TableRow row in table.Rows)
            {
                row["Match index"].Should().NotBeNullOrEmpty();
                row["Player 1 name"].Should().NotBeNull();
                row["Player 2 name"].Should().NotBeNull();

                ParseBracketGroupMatchSetup(row, out int matchIndex, out string player1Name, out string player2Name);

                if(player1Name.Length > 0)
                {
                    group.Matches[matchIndex].Player1.Name.Should().Be(player1Name);
                }
                else
                {
                    group.Matches[matchIndex].Player1.Should().BeNull();
                }

                if(player2Name.Length > 0)
                {
                    group.Matches[matchIndex].Player2.Name.Should().Be(player2Name);
                }
                else
                {
                    group.Matches[matchIndex].Player2.Should().BeNull();
                }
            }
        }

        protected static void CheckGroupValidity<GroupType>(GroupBase group)
        {
            group.Should().NotBeNull();
            group.Should().BeOfType<GroupType>();
            group.Id.Should().NotBeEmpty();
            group.ParticipatingPlayers.Should().BeEmpty();
            group.Matches.Should().BeEmpty();
            group.RoundId.Should().NotBeEmpty();
            group.Round.Should().NotBeNull();
        }

        protected static void ParseSoreAddedToMatchPlayer(TableRow row, out int createdGroupIndex, out int matchIndex, out string scoringPlayer, out int scoreAdded)
        {
            createdGroupIndex = 0;
            matchIndex = 0;
            scoringPlayer = "";
            scoreAdded = 0;

            if (row.ContainsKey("Created group index"))
            {
                int.TryParse(row["Created group index"], out createdGroupIndex);
            }

            if (row.ContainsKey("Match index"))
            {
                int.TryParse(row["Match index"], out matchIndex);
            }

            if (row.ContainsKey("Scoring player"))
            {
                scoringPlayer = row["Scoring player"];
            }

            if (row.ContainsKey("Added score"))
            {
                int.TryParse(row["Added score"], out scoreAdded);
            }
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

        protected static void ParseBracketGroupMatchSetup(TableRow row, out int matchIndex, out string player1Name, out string player2Name)
        {
            matchIndex = -1;
            player1Name = "";
            player2Name = "";

            if (row.ContainsKey("Match index"))
            {
                int.TryParse(row["Match index"], out matchIndex);
            }

            matchIndex.Should().BeGreaterOrEqualTo(0);

            if (row.ContainsKey("Player 1 name"))
            {
                player1Name = row["Player 1 name"];
            }

            if (row.ContainsKey("Player 2 name"))
            {
                player2Name = row["Player 2 name"];
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
    }
}