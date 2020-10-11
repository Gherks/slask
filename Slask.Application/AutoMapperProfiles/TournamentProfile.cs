using AutoMapper;
using Slask.Domain;
using Slask.Domain.Bets.BetTypes;
using Slask.Domain.Groups;
using Slask.Domain.Rounds;
using Slask.Domain.Utilities;
using Slask.Dto;
using System.Linq;

namespace Slask.Application.AutoMapperProfiles
{
    public class TournamentProfile : Profile
    {
        public TournamentProfile()
        {
            CreateMap<Tournament, TournamentDto>()
                .ForMember(
                    tournamentDto => tournamentDto.Issues,
                    options => options.MapFrom(tournament => tournament.TournamentIssueReporter.Issues));

            CreateMap<Better, BetterDto>()
                .ForMember(
                    betterDto => betterDto.UserId,
                    options => options.MapFrom(better => better.User.Id))
                .ForMember(
                    betterDto => betterDto.Name,
                    options => options.MapFrom(better => better.User.Name))
                .ForMember(
                    betterDto => betterDto.MatchBets,
                    options => options.MapFrom(better => better.Bets.OfType<MatchBet>()));

            CreateMap<MatchBet, MatchBetDto>();

            CreateMap<RoundBase, RoundDto>()
                .ForMember(
                    betterDto => betterDto.ContestType,
                    options => options.MapFrom(better => better.ContestType.ToString()));

            CreateMap<GroupBase, GroupDto>();

            CreateMap<Match, MatchDto>();

            CreateMap<Player, PlayerDto>()
                .ForMember(
                    betterDto => betterDto.Name,
                    options => options.MapFrom(better => better.GetName()));

            CreateMap<TournamentIssue, TournamentIssueDto>();
        }
    }
}
