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
        private readonly TournamentRepositoryInterface tournamentRepository;

        public GetTournamentByNameHandler(TournamentRepositoryInterface tournamentRepository)
        {
            tournamentRepository = tournamentRepository;
        }

        public TournamentDto Handle(GetTournamentByName query)
        {
            Tournament tournament = tournamentRepository.GetTournamentByName(query.TournamentName);

            return DomainToDtoConverters.ConvertToTournamentDto(tournament);
        }
    }
}
