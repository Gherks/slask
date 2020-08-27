using Slask.Domain.Utilities;

namespace Slask.Domain.ObjectState
{
    public abstract class ObjectStateBase : ObjectStateInterface
    {
        public ObjectStateEnum ObjectState { get; private set; }

        public ObjectStateBase()
        {
            ObjectState = ObjectStateEnum.Added;
        }

        public virtual void MarkForDeletion()
        {
            ObjectState = ObjectStateEnum.Deleted;
        }

        public void MarkAsModified()
        {
            if (ObjectState == ObjectStateEnum.Unchanged)
            {
                ObjectState = ObjectStateEnum.Modified;
            }
        }

        public void ResetObjectState()
        {
            ObjectState = ObjectStateEnum.Unchanged;
        }
    }
}
