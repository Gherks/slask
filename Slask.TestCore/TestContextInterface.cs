using Slask.Persistence;

namespace Slask.TestCore
{
    public abstract class TestContextBase
    {
        protected SlaskContext SlaskContext { get; }

        protected TestContextBase(SlaskContext slaskContext)
        {
            SlaskContext = slaskContext;
        }
    }
}
