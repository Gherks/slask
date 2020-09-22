using CSharpFunctionalExtensions;
using Slask.Application.Commands.Interfaces;
using Slask.Application.Queries.Interfaces;
using System;

namespace Slask.Application
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

        public ResultType Dispatch<ResultType>(QueryInterface<ResultType> query)
        {
            Type type = typeof(QueryHandlerInterface<,>);
            Type[] typeArgs = { query.GetType(), typeof(ResultType) };
            Type queryHandlerType = type.MakeGenericType(typeArgs);

            dynamic handler = _provider.GetService(queryHandlerType);
            ResultType result = handler.Handle((dynamic)query);

            return result;
        }
    }
}
