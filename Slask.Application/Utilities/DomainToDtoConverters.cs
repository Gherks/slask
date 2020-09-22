using Slask.Domain;
using Slask.Domain.Bets.BetTypes;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using Slask.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Application.Utilities
{
    public static class DomainToDtoConverters
    {
        public static UserDto ConvertToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name
            };
        }

        public static BareTournamentDto ConvertToBareTournamentDto(Tournament tournament)
        {
            return new BareTournamentDto()
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Created = tournament.Created
            };
        }

        public static TournamentDto ConvertToTournamentDto(Tournament tournament)
        {
            List<TournamentIssue> issues = tournament.TournamentIssueReporter.Issues;

            return new TournamentDto()
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Created = tournament.Created,
                Betters = tournament.Betters.Select(better => ConvertToBetterDto(better)).ToList(),
                Rounds = tournament.Rounds.Select(round => ConvertToRoundDto(round)).ToList(),
                Issues = issues.Select(issue => ConvertToTournamentIssueDto(issue)).ToList()
            };
        }

        public static BetterDto ConvertToBetterDto(Better better)
        {
            return new BetterDto()
            {
                Id = better.Id,
                UserId = better.User.Id,
                Name = better.User.Name,
                MatchBets = better.Bets.OfType<MatchBet>()
                    .Select(matchBet => ConvertToMatchBetDto(matchBet))
                    .ToList()
            };
        }

        public static MatchBetDto ConvertToMatchBetDto(MatchBet bet)
        {
            return new MatchBetDto()
            {
                Id = bet.Id,
                BetterId = bet.BetterId,
                MatchId = bet.MatchId,
                PlayerId = bet.PlayerId
            };
        }

        public static RoundDto ConvertToRoundDto(RoundBase round)
        {
            return new RoundDto()
            {
                Id = round.Id,
                ContestType = round.ContestType.ToString(),
                Name = round.Name,
                Groups = round.Groups.Select(group => ConvertToGroupDto(group)).ToList()
            };
        }

        public static GroupDto ConvertToGroupDto(GroupBase group)
        {
            return new GroupDto()
            {
                Id = group.Id,
                ContestType = group.ContestType.ToString(),
                SortOrder = group.SortOrder,
                Name = group.Name,
                Matches = group.Matches.Select(match => ConvertToMatchDto(match)).ToList()
            };
        }

        public static MatchDto ConvertToMatchDto(Match match)
        {
            return new MatchDto()
            {
                Id = match.Id,
                SortOrder = match.SortOrder,
                BestOf = match.BestOf,
                StartDateTime = match.StartDateTime,
                Player1 = ConvertToPlayerDto(match.Player1),
                Player2 = ConvertToPlayerDto(match.Player2)
            };
        }

        public static PlayerDto ConvertToPlayerDto(Player player)
        {
            Tournament tournament = player.Match.Group.Round.Tournament;
            PlayerReference playerReference = tournament.GetPlayerReferenceById(player.Id);

            return new PlayerDto()
            {
                Id = player.Id,
                Name = playerReference.Name,
                Score = player.Score
            };
        }

        public static TournamentIssueDto ConvertToTournamentIssueDto(TournamentIssue issue)
        {
            return new TournamentIssueDto()
            {
                Round = issue.Round,
                Group = issue.Group,
                Match = issue.Match,
                Description = issue.Description
            };
        }
    }
}
