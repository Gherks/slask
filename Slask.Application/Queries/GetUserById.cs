using Slask.Application.Interfaces.Persistence;
using Slask.Application.Queries.Interfaces;
using Slask.Application.Utilities;
using Slask.Domain;
using Slask.Dto;
using System;

namespace Slask.Application.Querys
{
    public sealed class GetUserById : QueryInterface<UserDto>
    {
        public Guid UserId { get; }
    }

    public sealed class GetUserByIdHandler : QueryHandlerInterface<GetUserById, UserDto>
    {
        private readonly UserRepositoryInterface _userRepository;

        public GetUserByIdHandler(UserRepositoryInterface userRepository)
        {
            _userRepository = userRepository;
        }

        public UserDto Handle(GetUserById query)
        {
            User user = _userRepository.GetUserById(query.UserId);

            return DomainToDtoConverters.ConvertToUserDto(user);
        }
    }
}
