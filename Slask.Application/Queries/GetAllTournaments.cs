using Slask.Application.Queries.Interfaces;
using Slask.Domain;
using Slask.Dto;
using Slask.Persistence.Services;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Application.Querys
{
    public sealed class GetAllTournaments : QueryInterface<IEnumerable<BareTournamentDto>>
    {
    }

    public sealed class GetAllTournamentsHandler : QueryHandlerInterface<GetAllTournaments, IEnumerable<BareTournamentDto>>
    {
        private readonly TournamentServiceInterface _tournamentService;

        public GetAllTournamentsHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public IEnumerable<BareTournamentDto> Handle(GetAllTournaments query)
        {
            return _tournamentService.GetTournaments().Select(tournament => ConvertToTournamentDto(tournament)).ToList();
        }

        private BareTournamentDto ConvertToTournamentDto(Tournament tournament)
        {
            return new BareTournamentDto()
            {
                Id = tournament.Id,
                Name = tournament.Name,
                Created = tournament.Created
            };
        }
    }
}
