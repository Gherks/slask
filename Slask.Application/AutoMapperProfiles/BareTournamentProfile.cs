using AutoMapper;
using Slask.Domain;
using Slask.Dto;

namespace Slask.Application.AutoMapperProfiles
{
    public class BareTournamentProfile : Profile
    {
        public BareTournamentProfile()
        {
            CreateMap<Tournament, BareTournamentDto>();
        }
    }
}
