namespace Slask.Domain.ObjectState
{
    public interface ObjectStateInterface
    {
        public ObjectStateEnum ObjectState { get; }

        public void ResetObjectState();
        public void MarkForDeletion();
    }
}
