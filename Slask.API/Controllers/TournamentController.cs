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

        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<BareTournamentDto>> GetTournaments()
        {
            GetAllTournaments query = new GetAllTournaments();
            Result<IEnumerable<BareTournamentDto>> result = _commandQueryDispatcher.Dispatch(query);

            if (result.IsFailure)
            {
                if (result.Value == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                return NotFound(result.Error);
            }

            return Ok(result.Value);
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
