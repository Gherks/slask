using FluentAssertions;
using Slask.Common;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Slask.Domain.SpecFlow.IntegrationTests
{
    [Binding, Scope(Feature = "Tournament")]
    public class TournamentSteps : TournamentStepDefinitions
    {

    }

    public class TournamentStepDefinitions
    {
        protected readonly List<Tournament> createdTournaments;
        protected readonly List<RoundBase> createdRounds;
        protected readonly List<GroupBase> createdGroups;

        public TournamentStepDefinitions()
        {
            createdTournaments = new List<Tournament>();
            createdRounds = new List<RoundBase>();
            createdGroups = new List<GroupBase>();
        }

        [Given(@"a tournament named ""(.*)"" has been created")]
        [When(@"a tournament named ""(.*)"" has been created")]
        public Tournament GivenATournamentNamedHasBeenCreated(string name)
        {
            createdTournaments.Add(Tournament.Create(name));

            return createdTournaments.Last();
        }

        [Given(@"a tournament named ""(.*)"" has been created with users ""(.*)"" added to it")]
        [When(@"a tournament named ""(.*)"" has been created with users ""(.*)"" added to it")]
        public Tournament GivenATournamentNamedHasBeenCreatedWithUsersAddedToIt(string tournamentName, string commaSeparatedUserNames)
        {
            Tournament tournament = Tournament.Create(tournamentName);
            createdTournaments.Add(tournament);

            List<string> userNames = StringUtility.ToStringList(commaSeparatedUserNames, ",");
            foreach (string userName in userNames)
            {
                tournament.AddBetter(User.Create(userName));
            }

            return createdTournaments.Last();
        }

        [Given(@"tournament (.*) adds rounds")]
        [When(@"tournament (.*) adds rounds")]
        public void GivenTournamentAddsRounds(int tournamentIndex, Table table)
        {
            if (createdTournaments.Count <= tournamentIndex)
            {
                throw new IndexOutOfRangeException("Given tournament index is out of bounds");
            }

            Tournament tournament = createdTournaments[tournamentIndex];

            foreach (TableRow row in table.Rows)
            {
                TestUtilities.ParseRoundTable(row, out string type, out string name, out int advancingCount, out int playersPerGroupCount);

                if (type.Length > 0)
                {
                    type = TestUtilities.ParseRoundGroupTypeString(type);
                    RoundBase round = null;

                    if (type == "BRACKET")
                    {
                        round = tournament.AddBracketRound();
                    }
                    else if (type == "DUALTOURNAMENT")
                    {
                        round = tournament.AddDualTournamentRound();
                    }
                    else if (type == "ROUNDROBIN")
                    {
                        round = tournament.AddRoundRobinRound();
                    }

                    if (round == null)
                    {
                        return;
                    }

                    round.RenameTo(name);
                    round.SetAdvancingPerGroupCount(advancingCount);
                    round.SetPlayersPerGroupCount(playersPerGroupCount);

                    createdRounds.Add(round);

                    if (round != null)
                    {
                        createdGroups.AddRange(round.Groups);
                    }
                }
            }
        }

        [Given(@"tournament (.*) removes round (.*)")]
        [When(@"tournament (.*) removes round (.*)")]
        public void WhenRoundIsRemovedFromTournament(int tournamentIndex, int roundIndex)
        {
            RoundBase round = createdRounds[roundIndex];
            Tournament tournament = createdTournaments[tournamentIndex];

            tournament.RemoveRound(round);

            createdRounds.Clear();
            createdRounds.AddRange(tournament.Rounds);
        }

        [Given(@"players ""(.*)"" is registered to round (.*)")]
        [When(@"players ""(.*)"" is registered to round (.*)")]
        [Then(@"players ""(.*)"" is registered to round (.*)")]
        public void GivenPlayersIsRegisteredToRound(string commaSeparatedPlayerNames, int roundIndex)
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

        [Given(@"groups within tournament is played out")]
        [When(@"groups within tournament is played out")]
        public void GivenGroupsWithinTournamentIsPlayedOut(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            foreach (TableRow row in table.Rows)
            {
                TestUtilities.ParseTargetGroupToPlay(row, out int tournamentIndex, out int roundIndex, out int groupIndex);

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
                    foreach (Match match in group.Matches)
                    {
                        if (match.IsReady() && match.GetPlayState() == PlayState.NotBegun)
                        {
                            SystemTimeMocker.SetOneSecondAfter(match.StartDateTime);
                            break;
                        }
                    }

                    bool playedMatchesSuccessfully = PlayAvailableMatches(group);

                    if (!playedMatchesSuccessfully)
                    {
                        break;
                    }
                }
            }
        }

        [Given(@"groups within tournament is played out and betted on")]
        [When(@"groups within tournament is played out and betted on")]
        public void GivenGroupsWithinTournamentIsPlayedOutAndBettedOn(Table table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            foreach (TableRow row in table.Rows)
            {
                TestUtilities.ParseTargetGroupToPlay(row, out int tournamentIndex, out int roundIndex, out int groupIndex);

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

                    bool playedMatchesSuccessfully = PlayAvailableMatches(group);

                    if (!playedMatchesSuccessfully)
                    {
                        break;
                    }
                }
            }
        }

        [Given(@"tournament (.*) contains (.*) rounds")]
        [When(@"tournament (.*) contains (.*) rounds")]
        [Then(@"tournament (.*) contains (.*) rounds")]
        public void ThenTournamentContainsRounds(int tournamentIndex, int roundCount)
        {
            Tournament tournament = createdTournaments[tournamentIndex];
            tournament.Rounds.Should().HaveCount(roundCount);
        }

        [Then(@"tournament (.*) should contain exactly these player references with names: ""(.*)""")]
        public void ThenTournamentShouldContainExactlyThesePlayerReferencesWithNames(int tournamentIndex, string commaSeparetedPlayerNames)
        {
            Tournament tournament = createdTournaments[tournamentIndex];
            List<string> playerNames = StringUtility.ToStringList(commaSeparetedPlayerNames, ",");

            List<PlayerReference> playerReferences = tournament.GetPlayerReferences();

            playerReferences.Should().HaveCount(playerNames.Count);

            foreach (string playerName in playerNames)
            {
                playerReferences.FirstOrDefault(playerReference => playerReference.Name == playerName).Should().NotBeNull();
            }
        }

        [Then(@"participating players in round (.*) should be exactly ""(.*)""")]
        public void ThenParticipatingPlayersInRoundShouldBeExactly(int roundIndex, string commaSeparatedPlayerNames)
        {
            RoundBase round = createdRounds[roundIndex];
            List<string> playerNames = StringUtility.ToStringList(commaSeparatedPlayerNames, ",");

            round.PlayerReferences.Should().HaveCount(playerNames.Count);

            for (int index = 0; index < round.PlayerReferences.Count; ++index)
            {
                round.PlayerReferences[index].Name.Should().Be(playerNames[index]);
            }
        }

        [Then(@"play state of tournament (.*) is set to ""(.*)""")]
        public void ThenPlayStateOfTournamentIsSetTo(int tournamentIndex, string playStateString)
        {
            Tournament tournament = createdTournaments[tournamentIndex];

            PlayState playState = ParsePlayStateString(playStateString);

            tournament.GetPlayState().Should().Be(playState);
        }

        protected bool PlayAvailableMatches(GroupBase group)
        {
            foreach (Match match in group.Matches)
            {
                int winningScore = (int)Math.Ceiling(match.BestOf / 2.0);

                bool matchShouldHaveStarted = match.StartDateTime < SystemTime.Now;
                bool matchIsNotFinished = match.GetPlayState() != PlayState.Finished;

                if (matchShouldHaveStarted && matchIsNotFinished)
                {
                    // Give points to player with name that precedes the other alphabetically
                    bool increasePlayer1Score = match.Player1.Name.CompareTo(match.Player2.Name) <= 0;
                    bool scoreIncreased;

                    if (increasePlayer1Score)
                    {
                        scoreIncreased = match.Player1.IncreaseScore(winningScore);
                    }
                    else
                    {
                        scoreIncreased = match.Player2.IncreaseScore(winningScore);
                    }

                    if (!scoreIncreased)
                    {
                        return false;
                    }
                }
            }

            return true;
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
