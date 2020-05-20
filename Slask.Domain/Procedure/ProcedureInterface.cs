namespace Slask.Domain.Procedure
{
    public interface ProcedureInterface<Type, ParentType>
    {
        bool NewValueValid(Type inValue, out Type outValue, ParentType parent);
        void ApplyPostAssignmentOperations(ParentType parent);
    }
}
