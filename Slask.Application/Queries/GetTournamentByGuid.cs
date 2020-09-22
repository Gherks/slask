using Slask.Application.Queries.Interfaces;
using Slask.Domain;
using Slask.Dto;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Querys
{
    public sealed class GetTournamentByGuid : QueryInterface<TournamentDto>
    {
        public Guid TournamentId { get; }
    }

    public sealed class GetTournamentByGuidHandler : QueryHandlerInterface<GetTournamentByGuid, TournamentDto>
    {
        private readonly TournamentServiceInterface _tournamentService;

        public GetTournamentByGuidHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public TournamentDto Handle(GetTournamentByGuid query)
        {
            Tournament tournament = _tournamentService.GetTournamentById(query.TournamentId);

            return ConvertToTournamentDto(tournament);
        }

        private TournamentDto ConvertToTournamentDto(Tournament tournament)
        {
            return new TournamentDto();
        }
    }
}
