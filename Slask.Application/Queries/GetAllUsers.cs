using Slask.Application.Queries.Interfaces;
using Slask.Application.Utilities;
using Slask.Dto;
using Slask.Persistence.Services;
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
            return _userService.GetUsers()
                .Select(user => DomainToDtoConverters.ConvertToUserDto(user))
                .ToList();
        }
    }
}
