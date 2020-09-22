namespace Slask.Application.Queries.Interfaces
{
    public interface QueryHandlerInterface<QueryType, ResultType> where QueryType : QueryInterface<ResultType>
    {
        ResultType Handle(QueryType query);
    }
}
