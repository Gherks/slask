using FluentAssertions;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Rounds;
using Slask.SpecFlow.IntegrationTests.DomainTests.RoundTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using TechTalk.SpecFlow;

namespace Slask.SpecFlow.IntegrationTests.DomainTests.GroupTests
{
    [Binding, Scope(Feature = "Group")]
    public class GroupSteps : GroupStepDefinitions
    {

    }

    public class GroupStepDefinitions : RoundStepDefinitions
    {
        protected readonly List<GroupBase> createdGroups;

        public GroupStepDefinitions()
        {
            createdGroups = new List<GroupBase>();
        }

        [When(@"created rounds (.*) to (.*) creates (.*) groups each")]
        public void WhenCreatedRoundsToCreatesGroupsEach(int roundStartIndex, int roundEndIndex, int groupAmount)
        {
            for (int roundIndex = roundStartIndex; roundIndex < roundEndIndex; ++roundIndex)
            {
                for (int groupCounter = 0; groupCounter < groupAmount; ++groupCounter)
                {
                    createdRounds[roundIndex].AddGroup();
                }
            }
        }

        [Given(@"group is added to created round (.*)")]
        [When(@"group is added to created round (.*)")]
        public void GivenGroupIsAddedToCreatedRound(int roundIndex)
        {
            if (createdRounds.Count <= roundIndex)
            {
                throw new IndexOutOfRangeException("Given created round index is out of bounds");
            }

            createdGroups.Add(createdRounds[roundIndex].AddGroup());
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
                group.AddPlayerReference(playerName);
            }
        }

        [Given(@"groups within created tournament is played out and betted on")]
        public void GivenCreatedGroupInCreatedRoundIsPlayedOutAndBettedOn(Table table)
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

                Tournament tournament = createdTournaments[tournamentIndex];
                RoundBase round = tournament.Rounds[roundIndex];
                GroupBase group = round.Groups[groupIndex];

                while (group.GetPlayState() != PlayState.IsFinished)
                {
                    PlaceBetsOnAvailableMatchesInGroup(tournament.Betters, group);

                    foreach (Domain.Match match in group.Matches)
                    {
                        if (match.IsReady() && match.GetPlayState() == PlayState.NotBegun)
                        {
                            SystemTimeMocker.Set(match.StartDateTime.AddMinutes(1));
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
                CheckGroupValidity<BracketGroup>(group, 0);
            }
            else if (type == "DUALTOURNAMENT")
            {
                CheckGroupValidity<DualTournamentGroup>(group, 5);
            }
            else if (type == "ROUNDROBIN")
            {
                CheckGroupValidity<RoundRobinGroup>(group, 0);
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

            List<PlayerReference> playerReferences = group.GetAdvancingPlayers();

            playerReferences.Should().NotBeEmpty();
            playerReferences.Should().HaveCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                playerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
            }
        }


        [Then(@"pariticpating players in created group (.*) should be mapped accordingly")]
        public void ThenPariticpatingPlayersInCreatedGroupShouldBeMappedAccordingly(int createdGroupIndex, Table table)
        {
            GroupBase group = createdGroups[createdGroupIndex];

            foreach (TableRow row in table.Rows)
            {
                row["Match index"].Should().NotBeNullOrEmpty();
                row["Player 1 name"].Should().NotBeNull();
                row["Player 2 name"].Should().NotBeNull();

                ParseBracketGroupMatchSetup(row, out int matchIndex, out string player1Name, out string player2Name);

                group.Matches[matchIndex].Player1.Name.Should().Be(player1Name);
                group.Matches[matchIndex].Player2.Name.Should().Be(player2Name);
            }
        }

        protected static void CheckGroupValidity<GroupType>(GroupBase group, int matchesUponCreation)
        {
            group.Should().NotBeNull();
            group.Should().BeOfType<GroupType>();
            group.Id.Should().NotBeEmpty();
            group.ParticipatingPlayers.Should().BeEmpty();
            group.Matches.Should().HaveCount(matchesUponCreation);
            group.RoundId.Should().NotBeEmpty();
            group.Round.Should().NotBeNull();
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

        protected virtual void PlayAvailableMatches(GroupBase group)
        {
            throw new NotImplementedException("Call this step within specific group type feature to test group progressions");
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