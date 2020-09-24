using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Slask.Application.Decorators
{
    public sealed class AuditLoggingDecorator<CommandType> : CommandHandlerInterface<CommandType>
        where CommandType : CommandInterface
    {
        private readonly CommandHandlerInterface<CommandType> _handler;

        public AuditLoggingDecorator(CommandHandlerInterface<CommandType> handler)
        {
            _handler = handler;
        }

        public Result Handle(CommandType command)
        {
            Console.WriteLine($"Command of type { command.GetType().Name } called.");

            return _handler.Handle(command);
        }
    }

    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class AuditLoggingAttribute : Attribute
    {
        public AuditLoggingAttribute()
        {
        }
    }
}
