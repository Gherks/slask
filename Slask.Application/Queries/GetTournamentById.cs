using Slask.Application.Queries.Interfaces;
using Slask.Application.Utilities;
using Slask.Domain;
using Slask.Dto;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Querys
{
    public sealed class GetTournamentById : QueryInterface<TournamentDto>
    {
        public Guid TournamentId { get; }
    }

    public sealed class GetTournamentByIdHandler : QueryHandlerInterface<GetTournamentById, TournamentDto>
    {
        private readonly TournamentServiceInterface _tournamentService;

        public GetTournamentByIdHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public TournamentDto Handle(GetTournamentById query)
        {
            Tournament tournament = _tournamentService.GetTournamentById(query.TournamentId);

            return DomainToDtoConverters.ConvertToTournamentDto(tournament);
        }
    }
}
