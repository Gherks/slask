﻿namespace Slask.Domain.ObjectState
{
    public abstract class ObjectStateBase : ObjectStateInterface
    {
        public ObjectStateEnum ObjectState { get; protected set; }

        public ObjectStateBase()
        {
            ObjectState = ObjectStateEnum.Added;
        }

        public void MarkForDeletion()
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
