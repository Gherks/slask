using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Slask.Application.Commands;
using Slask.Application.Querys;
using Slask.Application.Utilities;
using Slask.Dto;
using Slask.Dto.CreationDtos;
using System;
using System.Collections.Generic;

namespace Slask.API.Controllers
{
    [ApiController]
    [Route("api/tournaments")]
    public sealed class TournamentController : ControllerBase
    {
        private readonly CommandQueryDispatcher _commandQueryDispatcher;

        public TournamentController(CommandQueryDispatcher commandQueryDispatcher)
        {
            _commandQueryDispatcher = commandQueryDispatcher;
        }

        [HttpPost]
        public ActionResult CreateTournament(TournamentCreationDto tournament)
        {
            CreateTournament command = new CreateTournament(tournament.TournamentName);
            Result result = _commandQueryDispatcher.Dispatch(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return StatusCode(StatusCodes.Status201Created);
        }
    }
}
