using CSharpFunctionalExtensions;
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
        private readonly TournamentRepositoryInterface _tournamentRepository;

        public GetAllTournamentsHandler(TournamentRepositoryInterface tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Result<IEnumerable<BareTournamentDto>> Handle(GetAllTournaments query)
        {
            return Result.Success(_tournamentRepository.GetTournaments()
                .Select(tournament => DomainToDtoConverters.ConvertToBareTournamentDto(tournament)));
        }
    }
}
