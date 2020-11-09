﻿using FluentAssertions;
using Slask.Domain;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using Slask.Persistence.Repositories;
using Slask.SpecFlow.IntegrationTests.PersistenceTests;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Slask.Persistence.Specflow.IntegrationTests
{
    [Binding, Scope(Feature = "ChangeTournamentSettings")]
    public sealed class ChangeTournamentSettings : SpecflowBaseSteps
    {
        private readonly Dictionary<Guid, DateTime> oldMatchStartTimes;

        public ChangeTournamentSettings()
        {
            oldMatchStartTimes = new Dictionary<Guid, DateTime>();
        }

        [Given(@"round named ""(.*)"" is removed from tournament named ""(.*)""")]
        [When(@"round named ""(.*)"" is removed from tournament named ""(.*)""")]
        public void GivenRoundNamedIsRemovedFromTournamentNamed(string roundName, string tournamentName)
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(tournamentName);
                tournamentRepository.RemoveRoundFromTournament(tournament, roundName);
                tournamentRepository.Save();
            }
        }

        [Given(@"round named ""(.*)"" changes advancing players per group count to ""(.*)"" in tournament named ""(.*)""")]
        [When(@"round named ""(.*)"" changes advancing players per group count to ""(.*)"" in tournament named ""(.*)""")]
        public void GivenRoundNamedChangesAdvancingPlayersPerGroupCountToInTournamentNamed(string roundName, int newAdvancingPlayerCount, string tournamentName)
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(tournamentName);
                RoundBase round = tournament.GetRoundByName(roundName);

                tournamentRepository.SetAdvancingPerGroupCountInRound(round, newAdvancingPlayerCount);
                tournamentRepository.Save();
            }
        }

        [Given(@"round named ""(.*)"" changes players per group count to ""(.*)"" in tournament named ""(.*)""")]
        [When(@"round named ""(.*)"" changes players per group count to ""(.*)"" in tournament named ""(.*)""")]
        public void GivenRoundNamedChangesPlayersPerGroupCountToInTournamentNamed(string roundName, int newPlayersPerGroupCount, string tournamentName)
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(tournamentName);
                RoundBase round = tournament.GetRoundByName(roundName);

                tournamentRepository.SetPlayersPerGroupCountInRound(round, newPlayersPerGroupCount);
                tournamentRepository.Save();
            }
        }

        [Given(@"matches in tournament named ""(.*)"" changes best of setting")]
        [When(@"matches in tournament named ""(.*)"" changes best of setting")]
        public void WhenMatchesInTournamentNamedChangesBestOfSetting(string tournamentName, Table table)
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(tournamentName);

                foreach (MatchBestOfSelection selection in table.CreateSet<MatchBestOfSelection>())
                {
                    RoundBase roundBase = tournament.Rounds[selection.RoundIndex];
                    GroupBase groupBase = roundBase.Groups[selection.GroupIndex];
                    Match match = groupBase.Matches[selection.MatchIndex];

                    tournamentRepository.SetBestOfInMatch(match, selection.BestOf);
                }

                tournamentRepository.Save();
            }
        }

        [Given(@"player layout for matches in tournament named ""(.*)"" looks like this")]
        [When(@"player layout for matches in tournament named ""(.*)"" looks like this")]
        [Then(@"player layout for matches in tournament named ""(.*)"" looks like this")]
        public void GivenPlayerLayoutForMatchesInTournamentNamedLooksLikeThis(string tournamentName, Table table)
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(tournamentName);

                foreach (PlayerMatchLayout playerMatchLayout in table.CreateSet<PlayerMatchLayout>())
                {
                    RoundBase roundBase = tournament.Rounds[playerMatchLayout.RoundIndex];
                    GroupBase groupBase = roundBase.Groups[playerMatchLayout.GroupIndex];
                    Match match = groupBase.Matches[playerMatchLayout.MatchIndex];
                    string playerName = playerMatchLayout.PlayerIndex == 0 ? match.GetPlayer1Name() : match.GetPlayer2Name();

                    playerName.Should().Be(playerMatchLayout.PlayerName);
                }
            }
        }

        [When(@"tournament ""(.*)"" is renamed to ""(.*)""")]
        public void WhenTournamentIsRenamedTo(string oldTournamentName, string newTournamentName)
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(oldTournamentName);
                tournament.RenameTo(newTournamentName);
                tournamentRepository.Save();
            }
        }

        [When(@"move start time three hours forward for matches in tournament named ""(.*)""")]
        public void WhenMoveStartTimeThreeHoursForwardForMatchesInTournamentNamed(string tournamentName, Table table)
        {
            const int addedHours = 3;

            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(tournamentName);

                foreach (MatchBestOfSelection selection in table.CreateSet<MatchBestOfSelection>())
                {
                    RoundBase roundBase = tournament.Rounds[selection.RoundIndex];
                    GroupBase groupBase = roundBase.Groups[selection.GroupIndex];
                    Match match = groupBase.Matches[selection.MatchIndex];

                    oldMatchStartTimes[match.Id] = match.StartDateTime;

                    tournamentRepository.SetStartTimeForMatch(match, match.StartDateTime.AddHours(addedHours));
                }

                tournamentRepository.Save();
            }
        }

        [When(@"matches in tournament named ""(.*)"" switches player references")]
        public void WhenMatchesInTournamentNamedSwitchesPlayerReferences(string tournamentName, Table table)
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(tournamentName);

                foreach (PlayerSwitch playerSwitch in table.CreateSet<PlayerSwitch>())
                {
                    RoundBase roundBase = tournament.Rounds[playerSwitch.RoundIndex];

                    GroupBase groupBase1 = roundBase.Groups[playerSwitch.GroupIndex1];
                    Match match1 = groupBase1.Matches[playerSwitch.MatchIndex1];
                    Guid playerReference1Id = match1.FindPlayer(playerSwitch.PlayerName1);

                    GroupBase groupBase2 = roundBase.Groups[playerSwitch.GroupIndex2];
                    Match match2 = groupBase2.Matches[playerSwitch.MatchIndex2];
                    Guid playerReference2Id = match2.FindPlayer(playerSwitch.PlayerName2);

                    tournamentRepository.SwitchPlayersInMatches(match1, playerReference1Id, match2, playerReference2Id);
                }

                tournamentRepository.Save();
            }
        }

        [When(@"choosing players to solve tie in tournament named ""(.*)""")]
        public void WhenChoosingPlayersToSolveTie(string tournamentName, Table table)
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(tournamentName);

                foreach (TieSolving tieSolving in table.CreateSet<TieSolving>())
                {
                    RoundBase roundBase = tournament.Rounds[tieSolving.RoundIndex];
                    GroupBase groupBase = roundBase.Groups[tieSolving.GroupIndex];

                    PlayerReference playerReference = tournament.GetPlayerReferenceByName(tieSolving.PlayerName);

                    tournamentRepository.SolveTieByChoosingPlayerInGroup(groupBase, playerReference);
                }

                tournamentRepository.Save();
            }
        }

        [Then(@"best of for matches in tournament named ""(.*)"" should be set to")]
        public void ThenBestOfForMatchesInTournamentNamedShouldBeSetTo(string tournamentName, Table table)
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(tournamentName);

                foreach (MatchBestOfSelection selection in table.CreateSet<MatchBestOfSelection>())
                {
                    RoundBase roundBase = tournament.Rounds[selection.RoundIndex];
                    GroupBase groupBase = roundBase.Groups[selection.GroupIndex];
                    Match match = groupBase.Matches[selection.MatchIndex];

                    match.BestOf.Should().Be(selection.BestOf);
                }
            }
        }

        [Then(@"tournament named ""(.*)"" should exist")]
        public void ThenTournamentNamedShouldExist(string tournamentName)
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(tournamentName);

                tournament.Should().NotBeNull();
            }
        }

        [Then(@"tournament named ""(.*)"" should not exist")]
        public void ThenTournamentNamedShouldNotExist(string tournamentName)
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(tournamentName);

                tournament.Should().BeNull();
            }
        }

        [Then(@"round layout in tournament named ""(.*)"" is exactly as follows:")]
        public void ThenRoundLayoutInTournamentNamedIsExactlyAsFollows(string tournamentName, Table table)
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(tournamentName);

                tournament.Rounds.Should().HaveCount(table.RowCount);

                for (int rowIndex = 0; rowIndex < table.RowCount; ++rowIndex)
                {
                    RoundSettings roundSettings = table.Rows[rowIndex].CreateInstance<RoundSettings>();

                    RoundBase round = tournament.Rounds[rowIndex];

                    round.ContestType.Should().Be(roundSettings.ContestType);
                    round.Name.Should().Be(roundSettings.RoundName);
                    round.AdvancingPerGroupCount.Should().Be(roundSettings.AdvancingPerGroupCount);
                    round.PlayersPerGroupCount.Should().Be(roundSettings.PlayersPerGroupCount);
                }
            }
        }

        [Then(@"start time has been moved forward three hours for matches in tournament named ""(.*)""")]
        public void ThenStartTimeHasBeenMovedForwardThreeHoursForMatchesInTournamentNamed(string tournamentName, Table table)
        {
            using (TournamentRepository tournamentRepository = CreateTournamentRepository())
            {
                Tournament tournament = tournamentRepository.GetTournament(tournamentName);

                foreach (MatchBestOfSelection selection in table.CreateSet<MatchBestOfSelection>())
                {
                    RoundBase roundBase = tournament.Rounds[selection.RoundIndex];
                    GroupBase groupBase = roundBase.Groups[selection.GroupIndex];
                    Match match = groupBase.Matches[selection.MatchIndex];

                    DateTime oldStartDateTime = oldMatchStartTimes[match.Id];
                    match.StartDateTime.Should().BeCloseTo(oldStartDateTime.AddHours(3));
                }
            }
        }

        private sealed class RoundSettings
        {
            public ContestTypeEnum ContestType { get; set; }
            public string RoundName { get; set; }
            public int AdvancingPerGroupCount { get; set; }
            public int PlayersPerGroupCount { get; set; }
        }

        private sealed class MatchBestOfSelection
        {
            public int RoundIndex { get; set; }
            public int GroupIndex { get; set; }
            public int MatchIndex { get; set; }
            public int BestOf { get; set; }
        }

        private sealed class PlayerMatchLayout
        {
            public int RoundIndex { get; set; }
            public int GroupIndex { get; set; }
            public int MatchIndex { get; set; }
            public int PlayerIndex { get; set; }
            public string PlayerName { get; set; }
        }

        private sealed class PlayerSwitch
        {
            public int RoundIndex { get; set; }
            public int GroupIndex1 { get; set; }
            public int MatchIndex1 { get; set; }
            public string PlayerName1 { get; set; }
            public int GroupIndex2 { get; set; }
            public int MatchIndex2 { get; set; }
            public string PlayerName2 { get; set; }
        }

        private sealed class TieSolving
        {
            public int RoundIndex { get; set; }
            public int GroupIndex { get; set; }
            public string PlayerName { get; set; }
        }
    }
}
