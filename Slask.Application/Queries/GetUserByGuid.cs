using Slask.Application.Queries.Interfaces;
using Slask.Application.Utilities;
using Slask.Domain;
using Slask.Dto;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Querys
{
    public sealed class GetUserByGuid : QueryInterface<UserDto>
    {
        public Guid UserId { get; }
    }

    public sealed class GetUserByGuidHandler : QueryHandlerInterface<GetUserByGuid, UserDto>
    {
        private readonly UserServiceInterface _userService;

        public GetUserByGuidHandler(UserServiceInterface userService)
        {
            _userService = userService;
        }

        public UserDto Handle(GetUserByGuid query)
        {
            User user = _userService.GetUserById(query.UserId);

            return DomainToDtoConverters.ConvertToUserDto(user);
        }
    }
}
