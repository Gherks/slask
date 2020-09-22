using Slask.Application.Queries.Interfaces;
using Slask.Domain;
using Slask.Dto;
using Slask.Persistence.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Application.Querys
{
    public sealed class GetAllUsers : QueryInterface<IEnumerable<UserDto>>
    {
    }

    public sealed class GetAllUsersHander : QueryHandlerInterface<GetAllUsers, IEnumerable<UserDto>>
    {
        private readonly UserServiceInterface _userService;

        public GetAllUsersHander(UserServiceInterface userService)
        {
            _userService = userService;
        }

        public IEnumerable<UserDto> Handle(GetAllUsers query)
        {
            return _userService.GetUsers().Select(user => ConvertToUserDto(user)).ToList();
        }

        private UserDto ConvertToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name
            };
        }
    }
}
