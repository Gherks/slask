using Slask.Persistance;

namespace Slask.TestCore
{
    public abstract class SlaskContextCreatorInterface
    {
        public abstract SlaskContext CreateContext();
        public abstract SlaskContext CreateContext(bool beginTransaction = false);
    }
}