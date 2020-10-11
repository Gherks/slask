using AutoMapper;
using CSharpFunctionalExtensions;
using Slask.Application.Interfaces.Persistence;
using Slask.Application.Queries.Interfaces;
using Slask.Domain;
using Slask.Dto;

namespace Slask.Application.Querys
{
    public sealed class GetUserByName : QueryInterface<UserDto>
    {
        public string UserName { get; }

        public GetUserByName(string userName)
        {
            UserName = userName;
        }
    }

    public sealed class GetUserByNameHandler : QueryHandlerInterface<GetUserByName, UserDto>
    {
        private readonly UserRepositoryInterface _userRepository;
        private readonly IMapper _mapper;

        public GetUserByNameHandler(UserRepositoryInterface userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public Result<UserDto> Handle(GetUserByName query)
        {
            User user = _userRepository.GetUserByName(query.UserName);

            if (user == null)
            {
                return Result.Failure<UserDto>($"Could not find user ({ query.UserName })");
            }

            return Result.Success(_mapper.Map<UserDto>(user));
        }
    }
}
