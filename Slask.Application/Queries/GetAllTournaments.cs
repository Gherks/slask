using Slask.Application.Interfaces.Persistence;
using Slask.Application.Queries.Interfaces;
using Slask.Application.Utilities;
using Slask.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Application.Querys
{
    public sealed class GetAllTournaments : QueryInterface<IEnumerable<BareTournamentDto>>
    {
    }

    public sealed class GetAllTournamentsHandler : QueryHandlerInterface<GetAllTournaments, IEnumerable<BareTournamentDto>>
    {
        private readonly TournamentRepositoryInterface tournamentRepository;

        public GetAllTournamentsHandler(TournamentRepositoryInterface tournamentRepository)
        {
            tournamentRepository = tournamentRepository;
        }

        public IEnumerable<BareTournamentDto> Handle(GetAllTournaments query)
        {
            return tournamentRepository.GetTournaments()
                .Select(tournament => DomainToDtoConverters.ConvertToBareTournamentDto(tournament))
                .ToList();
        }
    }
}
