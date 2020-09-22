using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Domain;
using Slask.Persistence.Services;

namespace Slask.Application.Commands
{
    public sealed class CreateUser : CommandInterface
    {
        public string Name { get; }

        public CreateUser(string name)
        {
            Name = name;
        }
    }

    public sealed class CreateUserHandler : CommandHandlerInterface<CreateUser>
    {
        private readonly UserServiceInterface _userService;

        public CreateUserHandler(UserServiceInterface userService)
        {
            _userService = userService;
        }

        public Result Handle(CreateUser command)
        {
            User user = _userService.CreateUser(command.Name);

            if (user == null)
            {
                return Result.Failure($"Could not create user ({ command.Name })");
            }

            _userService.Save();
            return Result.Success();
        }
    }
}
