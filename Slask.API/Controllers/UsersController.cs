﻿using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Slask.Application.Commands;
using Slask.Application.Querys;
using Slask.Application.Utilities;
using Slask.Dto;
using Slask.Dto.CreationDtos;
using System.Collections.Generic;

namespace Slask.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public sealed class UsersController : ControllerBase
    {
        private readonly CommandQueryDispatcher _commandQueryDispatcher;

        public UsersController(CommandQueryDispatcher commandQueryDispatcher)
        {
            _commandQueryDispatcher = commandQueryDispatcher;
        }

        [HttpGet]
        [HttpHead]
        public ActionResult<IEnumerable<UserDto>> GetUsers()
        {
            GetAllUsers query = new GetAllUsers();
            Result<IEnumerable<UserDto>> result = _commandQueryDispatcher.Dispatch(query);

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

        [HttpGet("{userIdentifier}")]
        [HttpHead("{userIdentifier}")]
        public ActionResult<UserDto> GetUser(string userIdentifier)
        {
            GetUser query = new GetUser(userIdentifier);
            Result<UserDto> result = _commandQueryDispatcher.Dispatch(query);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpPost]
        public ActionResult CreateUser(UserCreationDto user)
        {
            CreateUser command = new CreateUser(user.Username);
            Result result = _commandQueryDispatcher.Dispatch(command);

            if (result.IsFailure)
            {
                return BadRequest(result.Error);
            }

            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpOptions]
        public ActionResult GetUserOptions()
        {
            Response.Headers.Add("Allow", "GET,HEAD,POST,OPTIONS");
            return Ok();
        }
    }
}
