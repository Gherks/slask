using AutoMapper;
using CSharpFunctionalExtensions;
using Slask.Application.Interfaces.Persistence;
using Slask.Application.Queries.Interfaces;
using Slask.Domain;
using Slask.Dto;
using System;

namespace Slask.Application.Querys
{
    public sealed class GetUser : QueryInterface<UserDto>
    {
        public string UserIdentifier { get; }

        public GetUser(string userIdentifier)
        {
            UserIdentifier = userIdentifier;
        }
    }

    public sealed class GetUserHandler : QueryHandlerInterface<GetUser, UserDto>
    {
        private readonly UserRepositoryInterface _userRepository;
        private readonly IMapper _mapper;

        public GetUserHandler(UserRepositoryInterface userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public Result<UserDto> Handle(GetUser query)
        {
            User user;

            if (Guid.TryParse(query.UserIdentifier, out Guid userId))
            {
                user = _userRepository.GetUserById(userId);
            }
            else
            {
                user = _userRepository.GetUserByName(query.UserIdentifier);
            }

            if (user == null)
            {
                return Result.Failure<UserDto>($"Could not find user ({ query.UserIdentifier })");
            }

            return Result.Success(_mapper.Map<UserDto>(user));
        }
    }
}
