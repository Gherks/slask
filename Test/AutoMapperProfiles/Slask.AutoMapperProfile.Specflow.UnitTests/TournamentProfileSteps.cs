using AutoMapper;
using FluentAssertions;
using Slask.Application.AutoMapperProfiles;
using Slask.Common;
using Slask.Domain;
using Slask.Domain.Utilities;
using Slask.Dto;
using Slask.Persistence.Repositories;
using Slask.SpecFlow.IntegrationTests.PersistenceTests;
using Slask.TestCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace Slask.AutoMapperProfile.Specflow.UnitTests
{
    [Binding, Scope(Feature = "TournamentProfile")]
    public class TournamentProfileSteps : SpecflowCoreSteps
    {
        private const int acceptableInaccuracy = 10000;
        private readonly DateTime tournamentCreated;

        private readonly IMapper _mapper;
        private readonly List<TournamentDto> _tournamentDtos;

        public TournamentProfileSteps()
        {
            tournamentCreated = SystemTime.Now;

            MapperConfiguration mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<TournamentProfile>();
            });
            _mapper = mapperConfiguration.CreateMapper();

            _tournamentDtos = new List<TournamentDto>();
        }

        [When(@"automapper maps domain tournament ""(.*)"" to a tournament DTO")]
        public void WhenAutomapperMapssDomainTournamentToATournamentDTO(string tournamentName)
        {
            using (TournamentRepository userRepository = CreateTournamentRepository())
            {
                Tournament tournament = userRepository.GetTournamentByName(tournamentName);
                _tournamentDtos.Add(_mapper.Map<TournamentDto>(tournament));
            }
        }

        [Then(@"automapped tournament DTO named ""(.*)"" should be valid with")]
        public void ThenAutomappedTournamentDTONamedShouldBeValid(string tournamentName, Table table)
        {
            TournamentDto tournamentDto = _tournamentDtos.FirstOrDefault(tournamentDto => tournamentDto.Name == tournamentName);

            tournamentDto.Id.Should().NotBeEmpty();
            tournamentDto.Name.Should().NotBeNullOrEmpty();
            tournamentDto.Created.Should().BeCloseTo(tournamentCreated, acceptableInaccuracy);
        }

        [Then(@"automapped tournament DTO named ""(.*)"" should contain betters ""(.*)""")]
        public void ThenAutomappedTournamentDTONamedShouldContainBetters(string tournamentName, string commaSeparatedUserNames)
        {
            TournamentDto tournamentDto = _tournamentDtos.FirstOrDefault(tournamentDto => tournamentDto.Name == tournamentName);

            List<string> userNames = StringUtility.ToStringList(commaSeparatedUserNames, ",");

            tournamentDto.Betters.Should().HaveCount(userNames.Count);

            foreach (string userName in userNames)
            {
                tournamentDto.Betters.FirstOrDefault(better => better.Name == userName).Should().NotBeNull();
            }
        }

        [Then(@"automapped tournament DTO named ""(.*)"" should contain rounds")]
        public void ThenAutomappedTournamentDTONamedShouldContainRounds(string tournamentName, Table table)
        {
            TournamentDto tournamentDto = _tournamentDtos.FirstOrDefault(tournamentDto => tournamentDto.Name == tournamentName);

            tournamentDto.Rounds.Should().HaveCount(table.Rows.Count);

            for (int index = 0; index < table.Rows.Count; ++index)
            {
                ParseTournamentDtoRound(table.Rows[index], out ContestTypeEnum contestType, out string roundName);

                RoundDto roundDto = tournamentDto.Rounds[index];

                roundDto.Id.Should().NotBeEmpty();
                roundDto.ContestType.Should().Be(contestType.ToString());
                roundDto.Name.Should().Be(roundName);
            }
        }

        [Then(@"automapped tournament DTO named ""(.*)"" should contain groups")]
        public void ThenAutomappedTournamentDTONamedShouldContainGroups(string tournamentName, Table table)
        {
            TournamentDto tournamentDto = _tournamentDtos.FirstOrDefault(tournamentDto => tournamentDto.Name == tournamentName);

            foreach (TableRow row in table.Rows)
            {
                ParseTournamentDtoGroup(row, out int roundIndex, out int groupIndex, out ContestTypeEnum contestType, out int sortOrder, out string groupName);

                RoundDto roundDto = tournamentDto.Rounds[roundIndex];
                GroupDto groupDto = roundDto.Groups[groupIndex];

                groupDto.Id.Should().NotBeEmpty();
                groupDto.ContestType.Should().Be(contestType.ToString());
                groupDto.SortOrder.Should().Be(sortOrder);
                groupDto.Name.Should().Be(groupName);
            }
        }

        [Then(@"automapped tournament DTO named ""(.*)"" should contain matches")]
        public void ThenAutomappedTournamentDTONamedShouldContainMatches(string tournamentName, Table table)
        {
            TournamentDto tournamentDto = _tournamentDtos.FirstOrDefault(tournamentDto => tournamentDto.Name == tournamentName);

            foreach (TableRow row in table.Rows)
            {
                ParseTournamentDtoMatch(row, out int roundIndex, out int groupIndex, out int matchIndex, out int sortOrder, out int bestOf, out string player1Name, out int player1Score, out string player2Name, out int player2Score);

                RoundDto roundDto = tournamentDto.Rounds[roundIndex];
                GroupDto groupDto = roundDto.Groups[groupIndex];
                MatchDto matchDto = groupDto.Matches[matchIndex];

                matchDto.Id.Should().NotBeEmpty();
                matchDto.SortOrder.Should().Be(sortOrder);
                matchDto.BestOf.Should().Be(bestOf);
                matchDto.StartDateTime.Should().BeBefore(SystemTime.Now);
                matchDto.Player1.Name.Should().Be(player1Name);
                matchDto.Player1.Score.Should().Be(player1Score);
                matchDto.Player2.Name.Should().Be(player2Name);
                matchDto.Player2.Score.Should().Be(player2Score);
            }
        }

        [Then(@"automapped tournament DTO named ""(.*)"" should contain issues")]
        public void ThenAutomappedTournamentDTONamedShouldContainIssues(string tournamentName, Table table)
        {
            TournamentDto tournamentDto = _tournamentDtos.FirstOrDefault(tournamentDto => tournamentDto.Name == tournamentName);

            tournamentDto.Issues.Should().HaveCount(table.Rows.Count);

            for (int index = 0; index < table.Rows.Count; ++index)
            {
                ParseTournamentDtoIssue(table.Rows[index], out int roundIndex, out int groupIndex, out int matchIndex, out string description);

                TournamentIssueDto tournamentIssueDto = tournamentDto.Issues[index];

                tournamentIssueDto.Round.Should().Be(roundIndex);
                tournamentIssueDto.Group.Should().Be(groupIndex);
                tournamentIssueDto.Match.Should().Be(matchIndex);
                tournamentIssueDto.Description.Should().Be(description);
            }
        }

        [Then(@"automapped tournament DTO named ""(.*)"" should contain no issues")]
        public void ThenAutomappedTournamentDTONamedShouldContainNoIssues(string tournamentName)
        {
            TournamentDto tournamentDto = _tournamentDtos.FirstOrDefault(tournamentDto => tournamentDto.Name == tournamentName);

            tournamentDto.Issues.Should().BeEmpty();
        }

        [Then(@"automapped tournament DTO named ""(.*)"" should contain better standings")]
        public void ThenAutomappedTournamentDTONamedShouldContainBetterStandings(string tournamentName, Table table)
        {
            TournamentDto tournamentDto = _tournamentDtos.FirstOrDefault(tournamentDto => tournamentDto.Name == tournamentName);

            tournamentDto.Betters.Should().HaveCount(table.Rows.Count);

            foreach (TableRow row in table.Rows)
            {
                ParseTournamentDtoBetter(row, out string betterName, out int points);

                BetterDto betterDto = tournamentDto.Betters.FirstOrDefault(betterDto => betterDto.Name == betterName);

                int betterPoints = CalculateBetterPoints(tournamentDto, betterDto.MatchBets);

                betterDto.Id.Should().NotBeEmpty();
                betterDto.UserId.Should().NotBeEmpty();
                betterDto.Name.Should().Be(betterName);
                betterPoints.Should().Be(points);
            }
        }

        private void ParseTournamentDtoRound(TableRow row, out ContestTypeEnum contestType, out string roundName)
        {
            contestType = ContestTypeEnum.None;
            roundName = "";

            if (row.ContainsKey("Contest type"))
            {
                contestType = TestUtilities.ParseContestTypeString(row["Contest type"]);
            }

            if (row.ContainsKey("Round name"))
            {
                roundName = row["Round name"];
            }
        }

        private void ParseTournamentDtoGroup(TableRow row, out int roundIndex, out int groupIndex, out ContestTypeEnum contestType, out int sortOrder, out string groupName)
        {
            roundIndex = -1;
            groupIndex = -1;
            contestType = ContestTypeEnum.None;
            sortOrder = -1;
            groupName = "";

            if (row.ContainsKey("Round index"))
            {
                int.TryParse(row["Round index"], out roundIndex);
            }

            if (row.ContainsKey("Group index"))
            {
                int.TryParse(row["Group index"], out groupIndex);
            }

            if (row.ContainsKey("Contest type"))
            {
                contestType = TestUtilities.ParseContestTypeString(row["Contest type"]);
            }

            if (row.ContainsKey("Sort order"))
            {
                int.TryParse(row["Sort order"], out sortOrder);
            }

            if (row.ContainsKey("Group name"))
            {
                groupName = row["Group name"];
            }
        }

        private void ParseTournamentDtoMatch(TableRow row, out int roundIndex, out int groupIndex, out int matchIndex, out int sortOrder, out int bestOf, out string player1Name, out int player1Score, out string player2Name, out int player2Score)
        {
            roundIndex = -1;
            groupIndex = -1;
            matchIndex = -1;
            sortOrder = -1;
            bestOf = -1;
            player1Name = "";
            player1Score = -1;
            player2Name = "";
            player2Score = -1;

            if (row.ContainsKey("Round index"))
            {
                int.TryParse(row["Round index"], out roundIndex);
            }

            if (row.ContainsKey("Group index"))
            {
                int.TryParse(row["Group index"], out groupIndex);
            }

            if (row.ContainsKey("Match index"))
            {
                int.TryParse(row["Match index"], out matchIndex);
            }

            if (row.ContainsKey("Sort order"))
            {
                int.TryParse(row["Sort order"], out sortOrder);
            }

            if (row.ContainsKey("Best of"))
            {
                int.TryParse(row["Best of"], out bestOf);
            }

            if (row.ContainsKey("Player1 name"))
            {
                player1Name = row["Player1 name"];
            }

            if (row.ContainsKey("Player1 score"))
            {
                int.TryParse(row["Player1 score"], out player1Score);
            }

            if (row.ContainsKey("Player2 name"))
            {
                player2Name = row["Player2 name"];
            }

            if (row.ContainsKey("Player2 score"))
            {
                int.TryParse(row["Player2 score"], out player2Score);
            }
        }

        private void ParseTournamentDtoIssue(TableRow row, out int roundIndex, out int groupIndex, out int matchIndex, out string description)
        {
            roundIndex = -1;
            groupIndex = -1;
            matchIndex = -1;
            description = "";

            if (row.ContainsKey("Round index"))
            {
                int.TryParse(row["Round index"], out roundIndex);
            }

            if (row.ContainsKey("Group index"))
            {
                int.TryParse(row["Group index"], out groupIndex);
            }

            if (row.ContainsKey("Match index"))
            {
                int.TryParse(row["Match index"], out matchIndex);
            }

            if (row.ContainsKey("Description"))
            {
                description = row["Description"];
            }
        }

        private void ParseTournamentDtoBetter(TableRow row, out string betterName, out int points)
        {
            betterName = "";
            points = -1;

            if (row.ContainsKey("Better name"))
            {
                betterName = row["Better name"];
            }

            if (row.ContainsKey("Points"))
            {
                int.TryParse(row["Points"], out points);
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
    }
}
