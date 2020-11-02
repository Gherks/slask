﻿using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Slask.Application.Commands;
using Slask.Application.Querys;
using Slask.Application.Utilities;
using Slask.Dto;
using Slask.Dto.CreationDtos;
using Slask.Dto.UpdateDtos;
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

        [HttpGet("{tournamentIdentifier}")]
        [HttpHead]
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

        [HttpPut("{tournamentId:guid}")]
        public ActionResult RenameTournament(Guid tournamentId, TournamentRenameDto tournamentRenameDto)
        {
            RenameTournament command = new RenameTournament(tournamentId, tournamentRenameDto.NewName);
            Result result = _commandQueryDispatcher.Dispatch(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpDelete("{tournamentId:guid}")]
        public ActionResult DeleteTournament(Guid tournamentId)
        {
            RemoveTournament command = new RemoveTournament(tournamentId);
            Result result = _commandQueryDispatcher.Dispatch(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return StatusCode(StatusCodes.Status204NoContent);
        }

        [HttpPut("{tournamentId:guid}/rounds")]
        public ActionResult AddRoundToTournament(Guid tournamentId, RoundCreationDto roundCreationDto)
        {
            AddRoundToTournament command = new AddRoundToTournament(tournamentId, roundCreationDto.RoundType);
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
    }
}
