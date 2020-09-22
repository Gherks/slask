using Slask.Application.Queries.Interfaces;
using Slask.Domain;
using Slask.Dto;
using Slask.Persistence.Services;

namespace Slask.Application.Querys
{
    public sealed class GetTournamentByName : QueryInterface<TournamentDto>
    {
        public string TournamentName { get; }
    }

    public sealed class GetTournamentByNameHandler : QueryHandlerInterface<GetTournamentByName, TournamentDto>
    { 
        private readonly TournamentServiceInterface _tournamentService;

        public GetTournamentByNameHandler(TournamentServiceInterface tournamentService)
        {
            _tournamentService = tournamentService;
        }

        public TournamentDto Handle(GetTournamentByName query)
        {
            Tournament tournament = _tournamentService.GetTournamentByName(query.TournamentName);

            return ConvertToTournamentDto(tournament);
        }

        private TournamentDto ConvertToTournamentDto(Tournament tournament)
        {
            return new TournamentDto();
        }
    }
}
