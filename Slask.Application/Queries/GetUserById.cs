using AutoMapper;
using CSharpFunctionalExtensions;
using Slask.Application.Interfaces.Persistence;
using Slask.Application.Queries.Interfaces;
using Slask.Domain;
using Slask.Dto;
using System;

namespace Slask.Application.Querys
{
    public sealed class GetUserById : QueryInterface<UserDto>
    {
        public Guid UserId { get; }

        public GetUserById(Guid userId)
        {
            UserId = userId;
        }
    }

    public sealed class GetUserByIdHandler : QueryHandlerInterface<GetUserById, UserDto>
    {
        private readonly UserRepositoryInterface _userRepository;
        private readonly IMapper _mapper;

        public GetUserByIdHandler(UserRepositoryInterface userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public Result<UserDto> Handle(GetUserById query)
        {
            User user = _userRepository.GetUserById(query.UserId);

            if (user == null)
            {
                return Result.Failure<UserDto>($"Could not find user ({ query.UserId })");
            }

            return Result.Success(_mapper.Map<UserDto>(user));
        }
    }
}
