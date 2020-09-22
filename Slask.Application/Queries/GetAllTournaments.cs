using Slask.Application.Queries.Interfaces;
using Slask.Domain;
using Slask.Dto;
using Slask.Persistence.Services;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Application.Querys
{
    public sealed class GetAllTournaments : QueryInterface<IEnumerable<TournamentDto>>
    {
    }

    public sealed class GetAllTournamentsHandler : QueryHandlerInterface<GetAllTournaments, IEnumerable<TournamentDto>>
    { 
        private readonly TournamentServiceInterface _tournamentService;

        public GetAllTournamentsHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public IEnumerable<TournamentDto> Handle(GetAllTournaments query)
        {
            return _tournamentService.GetTournaments().Select(tournament => ConvertToTournamentDto(tournament)).ToList();
        }

        private TournamentDto ConvertToTournamentDto(Tournament tournament)
        {
            return new TournamentDto();
        }
    }
}
