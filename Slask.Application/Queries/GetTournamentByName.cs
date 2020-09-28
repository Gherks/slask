using CSharpFunctionalExtensions;
using Slask.Application.Interfaces.Persistence;
using Slask.Application.Queries.Interfaces;
using Slask.Application.Utilities;
using Slask.Domain;
using Slask.Dto;

namespace Slask.Application.Querys
{
    public sealed class GetTournamentByName : QueryInterface<TournamentDto>
    {
        public string TournamentName { get; }
    }

    public sealed class GetTournamentByNameHandler : QueryHandlerInterface<GetTournamentByName, TournamentDto>
    {
        private readonly TournamentRepositoryInterface _tournamentRepository;

        public GetTournamentByNameHandler(TournamentRepositoryInterface tournamentRepository)
        {
            _tournamentRepository = tournamentRepository;
        }

        public Result<TournamentDto> Handle(GetTournamentByName query)
        {
            Tournament tournament = _tournamentRepository.GetTournamentByName(query.TournamentName);

            if (tournament == null)
            {
                return Result.Failure<TournamentDto>($"Could not find tournament ({ query.TournamentName })");
            }

            TournamentDto tournamentDto = DomainToDtoConverters.ConvertToTournamentDto(tournament);

            return Result.Success(tournamentDto);
        }
    }
}
