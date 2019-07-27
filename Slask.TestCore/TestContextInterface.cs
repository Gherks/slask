using Moq;
using Slask.Common;
using Slask.Persistence;
using System;

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
