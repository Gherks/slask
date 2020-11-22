using AutoMapper;
using CSharpFunctionalExtensions;
using Slask.Application.Commands;
using Slask.Application.Interfaces.Persistence;
using Slask.Application.Queries.Interfaces;
using Slask.Domain;
using Slask.Dto;

namespace Slask.Application.Querys
{
    public sealed class GetTournament : QueryInterface<TournamentDto>
    {
        public string TournamentIdentifier { get; }

        public GetTournament(string tournamentIdentifier)
        {
            TournamentIdentifier = tournamentIdentifier;
        }
    }

    public sealed class GetTournamentHandler : QueryHandlerInterface<GetTournament, TournamentDto>
    {
        private readonly TournamentRepositoryInterface _tournamentRepository;
        private readonly IMapper _mapper;

        public GetTournamentHandler(TournamentRepositoryInterface tournamentRepository, IMapper mapper)
        {
            _tournamentRepository = tournamentRepository;
            _mapper = mapper;
        }

        public Result<TournamentDto> Handle(GetTournament query)
        {
            Tournament tournament = CommandQueryUtilities.GetTournamentByIdentifier(_tournamentRepository, query.TournamentIdentifier);

            if (tournament == null)
            {
                return Result.Failure<TournamentDto>($"Could not find tournament ({ query.TournamentIdentifier })");
            }

            TournamentDto tournamentDto = _mapper.Map<TournamentDto>(tournament);

            return Result.Success(tournamentDto);
        }
    }
}
