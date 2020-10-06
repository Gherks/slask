using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Queries.Interfaces;
using System;

namespace Slask.Application.Utilities
{
    public sealed class CommandQueryDispatcher
    {
        private readonly IServiceProvider _provider;

        public CommandQueryDispatcher(IServiceProvider provider)
        {
            _provider = provider;
        }

        public Result Dispatch(CommandInterface command)
        {
            Type type = typeof(CommandHandlerInterface<>);
            Type[] typeArgs = { command.GetType() };
            Type commandHandlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _provider.GetService(commandHandlerType);
            Result result = handler.Handle((dynamic)command);

            return result;
        }

        public Result<ReturnType> Dispatch<ReturnType>(QueryInterface<ReturnType> query)
        {
            Type type = typeof(QueryHandlerInterface<,>);
            Type[] typeArgs = { query.GetType(), typeof(ReturnType) };
            Type queryHandlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _provider.GetService(queryHandlerType);
            Result<ReturnType> result = handler.Handle((dynamic)query);

            return result;
        }
    }
}
