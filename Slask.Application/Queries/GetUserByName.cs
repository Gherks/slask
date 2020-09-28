using CSharpFunctionalExtensions;
using Slask.Application.Interfaces.Persistence;
using Slask.Application.Queries.Interfaces;
using Slask.Application.Utilities;
using Slask.Domain;
using Slask.Dto;

namespace Slask.Application.Querys
{
    public sealed class GetUserByName : QueryInterface<UserDto>
    {
        public string UserName { get; }
    }

    public sealed class GetUserByNameHandler : QueryHandlerInterface<GetUserByName, UserDto>
    {
        private readonly UserRepositoryInterface _userRepository;

        public GetUserByNameHandler(UserRepositoryInterface userRepository)
        {
            _userRepository = userRepository;
        }

        public Result<UserDto> Handle(GetUserByName query)
        {
            User user = _userRepository.GetUserByName(query.UserName);

            if (user == null)
            {
                return Result.Failure<UserDto>($"Could not find user ({ query.UserName })");
            }

            UserDto userDto = DomainToDtoConverters.ConvertToUserDto(user);

            return Result.Success(userDto);
        }
    }
}
