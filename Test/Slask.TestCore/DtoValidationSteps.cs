using FluentAssertions;
using Slask.Common;
using Slask.Domain.Utilities;
using Slask.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Slask.SpecFlow.IntegrationTests.PersistenceTests
{
    public class DtoValidationSteps : SpecflowBaseSteps
    {
        private const int _acceptableInaccuracy = 10000;
        private readonly DateTime _tournamentCreated = SystemTime.Now;

        protected readonly List<UserDto> _userDtos = new List<UserDto>();
        protected readonly List<BareTournamentDto> _bareTournamentDtos = new List<BareTournamentDto>();
        protected readonly List<TournamentDto> _tournamentDtos = new List<TournamentDto>();

        [Then(@"all user DTOs should be valid")]
        public void ThenAllUserDTOsShouldBeValid()
        {
            foreach (UserDto userDto in _userDtos)
            {
                userDto.Id.Should().NotBeEmpty();
                userDto.Name.Should().NotBeNullOrEmpty();
            }
        }

        [Then(@"all bare tournament DTOs should be valid with names ""(.*)""")]
        public void ThenAllBareTournamentDTOsShouldBeValid(string commaSeparatedTournamentNames)
        {
            List<string> tournamentNames = commaSeparatedTournamentNames.ToStringList(",");

            _bareTournamentDtos.Should().HaveCount(tournamentNames.Count);

            for (int index = 0; index < _bareTournamentDtos.Count; ++index)
            {
                BareTournamentDto bareTournamentDto = _bareTournamentDtos[index];

                bareTournamentDto.Id.Should().NotBeEmpty();
                bareTournamentDto.Name.Should().Be(tournamentNames[index]);
                bareTournamentDto.Created.Should().BeCloseTo(SystemTime.Now, _acceptableInaccuracy);
            }
        }

        [Then(@"tournament DTO named ""(.*)"" should be valid with (.*) betters, (.*) rounds, and (.*) issues")]
        public void ThenTournamentDTONamedShouldBeValid(string tournamentName, int betterCount, int roundCount, int issueCount)
        {
            TournamentDto tournamentDto = _tournamentDtos.FirstOrDefault(tournamentDto => tournamentDto.Name == tournamentName);

            tournamentDto.Id.Should().NotBeEmpty();
            tournamentDto.Name.Should().NotBeNullOrEmpty();
            tournamentDto.Created.Should().BeCloseTo(_tournamentCreated, _acceptableInaccuracy);
            tournamentDto.Betters.Should().HaveCount(betterCount);
            tournamentDto.Rounds.Should().HaveCount(roundCount);
            tournamentDto.Issues.Should().HaveCount(issueCount);
        }

        [Then(@"tournament DTO named ""(.*)"" should contain betters ""(.*)""")]
        public void ThenTournamentDTONamedShouldContainBetters(string tournamentName, string commaSeparatedUserNames)
        {
            TournamentDto tournamentDto = _tournamentDtos.FirstOrDefault(tournamentDto => tournamentDto.Name == tournamentName);

            List<string> userNames = commaSeparatedUserNames.ToStringList(",");

            tournamentDto.Betters.Should().HaveCount(userNames.Count);

            foreach (string userName in userNames)
            {
                tournamentDto.Betters.FirstOrDefault(better => better.Name == userName).Should().NotBeNull();
            }
        }

        [Then(@"tournament DTO named ""(.*)"" should contain rounds")]
        public void ThenTournamentDTONamedShouldContainRounds(string tournamentName, Table table)
        {
            TournamentDto tournamentDto = _tournamentDtos.FirstOrDefault(tournamentDto => tournamentDto.Name == tournamentName);

            tournamentDto.Rounds.Should().HaveCount(table.RowCount);

            for (int index = 0; index < table.RowCount; ++index)
            {
                TournamentDtoRoundLayout layout = table.Rows[index].CreateInstance<TournamentDtoRoundLayout>();

                RoundDto roundDto = tournamentDto.Rounds[index];

                roundDto.Id.Should().NotBeEmpty();
                roundDto.ContestType.Should().Be(layout.ContestType.ToString());
                roundDto.Name.Should().Be(layout.RoundName);
            }
        }

        [Then(@"tournament DTO named ""(.*)"" should contain groups")]
        public void ThenTournamentDTONamedShouldContainGroups(string tournamentName, Table table)
        {
            TournamentDto tournamentDto = _tournamentDtos.FirstOrDefault(tournamentDto => tournamentDto.Name == tournamentName);

            foreach (TournamentDtoGroupLayout layout in table.CreateSet<TournamentDtoGroupLayout>())
            {
                RoundDto roundDto = tournamentDto.Rounds[layout.RoundIndex];
                GroupDto groupDto = roundDto.Groups[layout.GroupIndex];

                groupDto.Id.Should().NotBeEmpty();
                groupDto.ContestType.Should().Be(layout.ContestType.ToString());
                groupDto.SortOrder.Should().Be(layout.SortOrder);
                groupDto.Name.Should().Be(layout.GroupName);
            }
        }

        [Then(@"tournament DTO named ""(.*)"" should contain matches")]
        public void ThenTournamentDTONamedShouldContainMatches(string tournamentName, Table table)
        {
            TournamentDto tournamentDto = _tournamentDtos.FirstOrDefault(tournamentDto => tournamentDto.Name == tournamentName);

            foreach (TournamentMatchLayout tournamentMatchLayout in table.CreateSet<TournamentMatchLayout>())
            {
                RoundDto roundDto = tournamentDto.Rounds[tournamentMatchLayout.RoundIndex];
                GroupDto groupDto = roundDto.Groups[tournamentMatchLayout.GroupIndex];
                MatchDto matchDto = groupDto.Matches[tournamentMatchLayout.MatchIndex];

                matchDto.Id.Should().NotBeEmpty();
                matchDto.SortOrder.Should().Be(tournamentMatchLayout.SortOrder);
                matchDto.BestOf.Should().Be(tournamentMatchLayout.BestOf);
                matchDto.StartDateTime.Should().BeBefore(SystemTime.Now);
                matchDto.Player1.Name.Should().Be(tournamentMatchLayout.Player1Name);
                matchDto.Player1.Score.Should().Be(tournamentMatchLayout.Player1Score);
                matchDto.Player2.Name.Should().Be(tournamentMatchLayout.Player2Name);
                matchDto.Player2.Score.Should().Be(tournamentMatchLayout.Player2Score);
            }
        }

        [Then(@"tournament DTO named ""(.*)"" should contain issues")]
        public void ThenTournamentDTONamedShouldContainIssues(string tournamentName, Table table)
        {
            TournamentDto tournamentDto = _tournamentDtos.FirstOrDefault(tournamentDto => tournamentDto.Name == tournamentName);

            tournamentDto.Issues.Should().HaveCount(table.RowCount);

            for (int index = 0; index < table.RowCount; ++index)
            {
                TournamentIssueDto soughtTournamentIssueDto = table.Rows[index].CreateInstance<TournamentIssueDto>();

                TournamentIssueDto tournamentIssueDto = tournamentDto.Issues[index];

                tournamentIssueDto.Round.Should().Be(soughtTournamentIssueDto.Round);
                tournamentIssueDto.Group.Should().Be(soughtTournamentIssueDto.Group);
                tournamentIssueDto.Match.Should().Be(soughtTournamentIssueDto.Match);
                tournamentIssueDto.Description.Should().Be(soughtTournamentIssueDto.Description);
            }
        }

        [Then(@"tournament DTO named ""(.*)"" should contain no issues")]
        public void ThenTournamentDTONamedShouldContainNoIssues(string tournamentName)
        {
            TournamentDto tournamentDto = _tournamentDtos.FirstOrDefault(tournamentDto => tournamentDto.Name == tournamentName);

            tournamentDto.Issues.Should().BeEmpty();
        }

        [Then(@"tournament DTO named ""(.*)"" should contain better standings")]
        public void ThenTournamentDTONamedShouldContainBetterStandings(string tournamentName, Table table)
        {
            TournamentDto tournamentDto = _tournamentDtos.FirstOrDefault(tournamentDto => tournamentDto.Name == tournamentName);

            tournamentDto.Betters.Should().HaveCount(table.RowCount);

            foreach (BetterStanding betterStanding in table.CreateSet<BetterStanding>())
            {
                BetterDto betterDto = tournamentDto.Betters.FirstOrDefault(betterDto => betterDto.Name == betterStanding.BetterName);

                int betterPoints = CalculateBetterPoints(tournamentDto, betterDto.MatchBets);

                betterDto.Id.Should().NotBeEmpty();
                betterDto.UserId.Should().NotBeEmpty();
                betterDto.Name.Should().Be(betterStanding.BetterName);
                betterPoints.Should().Be(betterStanding.Points);
            }
        }

        private MatchDto FindMatchInTournamentDto(TournamentDto tournamentDto, Guid matchId)
        {
            foreach (RoundDto roundDto in tournamentDto.Rounds)
            {
                foreach (GroupDto groupDto in roundDto.Groups)
                {
                    foreach (MatchDto matchDto in groupDto.Matches)
                    {
                        if (matchDto.Id == matchId)
                        {
                            return matchDto;
                        }
                    }
                }
            }

            return null;
        }

        private PlayerDto GetWinningPlayerDtoOfMatchDto(MatchDto matchDto)
        {
            if (matchDto.Player1.Score > matchDto.Player2.Score)
            {
                return matchDto.Player1;
            }
            else if (matchDto.Player1.Score < matchDto.Player2.Score)
            {
                return matchDto.Player2;
            }
            else
            {
                throw new ArgumentException("No winner found in match DTO");
            }
        }

        private int CalculateBetterPoints(TournamentDto tournamentDto, List<MatchBetDto> matchBetDtos)
        {
            int points = 0;

            foreach (MatchBetDto matchBetDto in matchBetDtos)
            {
                MatchDto matchDto = FindMatchInTournamentDto(tournamentDto, matchBetDto.MatchId);
                PlayerDto winningPlayerDto = GetWinningPlayerDtoOfMatchDto(matchDto);

                if (winningPlayerDto.Id == matchBetDto.PlayerId)
                {
                    points++;
                }
            }

            return points;
        }

        private sealed class TournamentDtoRoundLayout
        {
            public ContestTypeEnum ContestType { get; set; }
            public string RoundName { get; set; }
        }

        private sealed class TournamentDtoGroupLayout
        {
            public int RoundIndex { get; set; }
            public int GroupIndex { get; set; }
            public ContestTypeEnum ContestType { get; set; }
            public int SortOrder { get; set; }
            public string GroupName { get; set; }
        }

        private sealed class TournamentMatchLayout
        {
            public int RoundIndex { get; set; }
            public int GroupIndex { get; set; }
            public int MatchIndex { get; set; }
            public int SortOrder { get; set; }
            public int BestOf { get; set; }
            public string Player1Name { get; set; }
            public int Player1Score { get; set; }
            public string Player2Name { get; set; }
            public int Player2Score { get; set; }
        }

        private sealed class BetterStanding
        {
            public string BetterName { get; set; }
            public int Points { get; set; }
        }
    }
}
