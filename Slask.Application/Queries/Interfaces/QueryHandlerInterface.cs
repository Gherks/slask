using CSharpFunctionalExtensions;

namespace Slask.Application.Queries.Interfaces
{
    public interface QueryHandlerInterface<QueryType, ReturnType> where QueryType : QueryInterface<ReturnType>
    {
        Result<ReturnType> Handle(QueryType query);
    }
}
