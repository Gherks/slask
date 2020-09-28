using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Interfaces.Persistence;
using Slask.Domain;

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
        private readonly UserRepositoryInterface _userRepository;

        public CreateUserHandler(UserRepositoryInterface userRepository)
        {
            _userRepository = userRepository;
        }

        public Result Handle(CreateUser command)
        {
            User user = _userRepository.CreateUser(command.Name);

            if (user == null)
            {
                return Result.Failure($"Could not create user ({ command.Name })");
            }

            _userRepository.Save();
            return Result.Success();
        }
    }
}
