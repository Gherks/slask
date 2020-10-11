using AutoMapper;
using CSharpFunctionalExtensions;
using Slask.Application.Interfaces.Persistence;
using Slask.Application.Queries.Interfaces;
using Slask.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Slask.Application.Querys
{
    public sealed class GetAllUsers : QueryInterface<IEnumerable<UserDto>>
    {
    }

    public sealed class GetAllUsersHandler : QueryHandlerInterface<GetAllUsers, IEnumerable<UserDto>>
    {
        private readonly UserRepositoryInterface _userRepository;
        private readonly IMapper _mapper;

        public GetAllUsersHandler(UserRepositoryInterface userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public Result<IEnumerable<UserDto>> Handle(GetAllUsers query)
        {
            try
            {
                return Result.Success(_userRepository.GetUsers()
                    .Select(user => _mapper.Map<UserDto>(user)));
            }
            catch (Exception exception)
            {
                return Result.Failure<IEnumerable<UserDto>>(exception.Message);
            }
        }
    }
}
