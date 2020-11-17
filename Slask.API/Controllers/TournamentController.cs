using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Slask.Application.Commands;
using Slask.Application.Querys;
using Slask.Application.Utilities;
using Slask.Dto;
using Slask.Dto.CreationDtos;
using Slask.Dto.UpdateDtos;
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

        [HttpGet("{tournamentIdentifier}")]
        [HttpHead("{tournamentIdentifier}")]
        public ActionResult<TournamentDto> GetTournament(string tournamentIdentifier)
        {
            GetTournament query = new GetTournament(tournamentIdentifier);
            Result<TournamentDto> result = _commandQueryDispatcher.Dispatch(query);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPut("{tournamentIdentifier}")]
        public ActionResult RenameTournament(string tournamentIdentifier, TournamentRenameDto tournamentRenameDto)
        {
            RenameTournament command = new RenameTournament(tournamentIdentifier, tournamentRenameDto.NewName);
            Result result = _commandQueryDispatcher.Dispatch(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpDelete("{tournamentIdentifier}")]
        public ActionResult DeleteTournament(string tournamentIdentifier)
        {
            RemoveTournament command = new RemoveTournament(tournamentIdentifier);
            Result result = _commandQueryDispatcher.Dispatch(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPut("{tournamentIdentifier}/rounds")]
        public ActionResult AddRoundToTournament(string tournamentIdentifier, RoundCreationDto roundCreationDto)
        {
            AddRoundToTournament command = new AddRoundToTournament(tournamentIdentifier, roundCreationDto.RoundType);
            Result result = _commandQueryDispatcher.Dispatch(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPut("{tournamentIdentifier}/rounds/{roundIdentifier}")]
        public ActionResult ChangePlayersPerGroupCountInRound(string tournamentIdentifier, string roundIdentifier, PlayersPerGroupCountChangeDto playersPerGroupCountChangeDto)
        {
            ChangeGroupSizeInRound command = new ChangeGroupSizeInRound(tournamentIdentifier, roundIdentifier, playersPerGroupCountChangeDto.PlayersPerGroupCount);
            Result result = _commandQueryDispatcher.Dispatch(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpDelete("{tournamentIdentifier}/rounds/{roundIdentifier}")]
        public ActionResult DeleteRoundFromTournament(string tournamentIdentifier, string roundIdentifier)
        {
            RemoveRoundFromTournament command = new RemoveRoundFromTournament(tournamentIdentifier, roundIdentifier);
            Result result = _commandQueryDispatcher.Dispatch(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpOptions]
        public ActionResult GetTournamentOptions()
        {
            Response.Headers.Add("Allow", "GET,HEAD,POST,PUT,DELETE,OPTIONS");
            return Ok();
        }
    }
}
