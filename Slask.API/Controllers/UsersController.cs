using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Mvc;
using Slask.Application.Querys;
using Slask.Application.Utilities;
using Slask.Dto;
using System;
using System.Collections.Generic;

namespace Slask.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly CommandQueryDispatcher _commandQueryDispatcher;

        public UsersController(CommandQueryDispatcher commandQueryDispatcher)
        {
            _commandQueryDispatcher = commandQueryDispatcher;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            GetAllUsers query = new GetAllUsers();
            Result<IEnumerable<UserDto>> result = _commandQueryDispatcher.Dispatch(query);

            if (result.IsFailure)
            {
                if (result.Value == null)
                {
                    return StatusCode(500);
                }

                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }

        [HttpGet("{userId}")]
        public IActionResult GetUser(Guid userId)
        {
            GetUserById query = new GetUserById(userId);
            Result<UserDto> result = _commandQueryDispatcher.Dispatch(query);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }

        //[HttpPost("{userName}")]
        //public IActionResult Post(UserCreationDto user)
        //{
        //    if(user == null)
        //    {
        //        return BadRequest();
        //    }

        //    CreateUser command = new CreateUser(userName);
        //    Result result = _commandQueryDispatcher.Dispatch(command);

        //    if (result.IsFailure)
        //    {
        //        return NotFound(result.Error);
        //    }

        //    return Ok();
        //}
    }
}
