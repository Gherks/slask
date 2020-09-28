using CSharpFunctionalExtensions;
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
        private readonly TournamentRepositoryInterface _tournamentRepository;

        public GetTournamentByIdHandler(TournamentRepositoryInterface tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Result<TournamentDto> Handle(GetTournamentById query)
        {
            Tournament tournament = _tournamentRepository.GetTournamentById(query.TournamentId);

            if (tournament == null)
            {
                return Result.Failure<TournamentDto>($"Could not find tournament ({ query.TournamentId })");
            }

            TournamentDto tournamentDto = DomainToDtoConverters.ConvertToTournamentDto(tournament);

            return Result.Success(tournamentDto);
        }
    }
}
