namespace Slask.Domain.ObjectState
{
    public interface ObjectStateInterface
    {
        public ObjectStateEnum ObjectState { get; }

        public void MarkForDeletion();
        public void MarkAsModified();
        public void ResetObjectState();
    }
}
