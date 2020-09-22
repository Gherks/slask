using Slask.Application.Queries.Interfaces;
using Slask.Domain;
using Slask.Dto;
using Slask.Persistence.Services;

namespace Slask.Application.Querys
{
    public sealed class GetUserByName : QueryInterface<UserDto>
    {
        public string UserName { get; }
    }

    public sealed class GetUserByNameHandler : QueryHandlerInterface<GetUserByName, UserDto>
    {
        private readonly UserServiceInterface _userService;

        public GetUserByNameHandler(UserServiceInterface userService)
        {
            _userService = userService;
        }

        public UserDto Handle(GetUserByName query)
        {
            User user = _userService.GetUserByName(query.UserName);

            return ConvertToUserDto(user);
        }

        private UserDto ConvertToUserDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                Name = user.Name
            };
        }
    }
}
