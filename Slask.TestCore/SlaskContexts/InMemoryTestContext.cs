using Slask.UnitTests;

namespace Slask.TestCore.SlaskContexts
{
    public class InMemoryTestContext : TestContextBase
    {
        public InMemoryTestContext()
            : base(InMemoryContextCreator.Create())
        {
        }
    }
}
