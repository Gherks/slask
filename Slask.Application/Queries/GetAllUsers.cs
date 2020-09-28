using CSharpFunctionalExtensions;
using Slask.Application.Interfaces.Persistence;
using Slask.Application.Queries.Interfaces;
using Slask.Application.Utilities;
using Slask.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Application.Querys
{
    public sealed class GetAllUsers : QueryInterface<IEnumerable<UserDto>>
    {
    }

    public sealed class GetAllUsersHander : QueryHandlerInterface<GetAllUsers, IEnumerable<UserDto>>
    {
        private readonly UserRepositoryInterface _userRepository;

        public GetAllUsersHander(UserRepositoryInterface userRepository)
        {
            _userRepository = userRepository;
        }

        public Result<IEnumerable<UserDto>> Handle(GetAllUsers query)
        {
            return Result.Success(_userRepository.GetUsers()
                .Select(user => DomainToDtoConverters.ConvertToUserDto(user)));
        }
    }
}
