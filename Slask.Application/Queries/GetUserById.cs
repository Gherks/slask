using Slask.Application.Queries.Interfaces;
using Slask.Application.Utilities;
using Slask.Domain;
using Slask.Dto;
using Slask.Persistence.Services;
using System;

namespace Slask.Application.Querys
{
    public sealed class GetUserById : QueryInterface<UserDto>
    {
        public Guid UserId { get; }
    }

    public sealed class GetUserByIdHandler : QueryHandlerInterface<GetUserById, UserDto>
    {
        private readonly UserServiceInterface _userService;

        public GetUserByIdHandler(UserServiceInterface userService)
        {
            _userService = userService;
        }

        public UserDto Handle(GetUserById query)
        {
            User user = _userService.GetUserById(query.UserId);

            return DomainToDtoConverters.ConvertToUserDto(user);
        }
    }
}
