﻿using Slask.Application.Queries.Interfaces;
using Slask.Application.Utilities;
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
            return _tournamentService.GetTournaments()
                .Select(tournament => DomainToDtoConverters.ConvertToBareTournamentDto(tournament))
                .ToList();
        }
    }
}
