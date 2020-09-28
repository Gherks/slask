using CSharpFunctionalExtensions;
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

        public Result<UserDto> Handle(GetUserById query)
        {
            User user = _userRepository.GetUserById(query.UserId);

            if (user == null)
            {
                return Result.Failure<UserDto>($"Could not find user ({ query.UserId })");
            }

            UserDto userDto = DomainToDtoConverters.ConvertToUserDto(user);

            return Result.Success(userDto);
        }
    }
}
