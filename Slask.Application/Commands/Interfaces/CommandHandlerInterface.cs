using CSharpFunctionalExtensions;

namespace Slask.Application.Commands.Interfaces
{
    public interface CommandHandlerInterface<CommandType> where CommandType : CommandInterface
    {
        Result Handle(CommandType command);
    }
}
