using Slask.Application.Interfaces.Persistence;
using Slask.Application.Queries.Interfaces;
using Slask.Application.Utilities;
using Slask.Domain;
using Slask.Dto;
using System;

namespace Slask.Application.Querys
{
    public sealed class GetTournamentById : QueryInterface<TournamentDto>
    {
        public Guid TournamentId { get; }
    }

    public sealed class GetTournamentByIdHandler : QueryHandlerInterface<GetTournamentById, TournamentDto>
    {
        private readonly TournamentRepositoryInterface tournamentRepository;

        public GetTournamentByIdHandler(TournamentRepositoryInterface tournamentRepository)
        {
            tournamentRepository = tournamentRepository;
        }

        public TournamentDto Handle(GetTournamentById query)
        {
            Tournament tournament = tournamentRepository.GetTournamentById(query.TournamentId);

            return DomainToDtoConverters.ConvertToTournamentDto(tournament);
        }
    }
}
