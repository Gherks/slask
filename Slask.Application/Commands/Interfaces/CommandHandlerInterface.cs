using CSharpFunctionalExtensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Slask.Application.Commands.Interfaces
{
    public interface CommandHandlerInterface<CommandType> where CommandType : CommandInterface
    {
        Result Handle(CommandType command);
    }
}
