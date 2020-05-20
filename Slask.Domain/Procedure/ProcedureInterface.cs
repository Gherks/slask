namespace Slask.Domain.Procedure
{
    public interface ProcedureInterface<Type, ParentType>
    {
        bool Set(Type inValue, out Type outValue, ParentType parent);
    }
}
