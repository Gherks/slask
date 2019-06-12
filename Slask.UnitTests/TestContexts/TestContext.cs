using Microsoft.EntityFrameworkCore;
using Slask.Data;
using System;

namespace Slask.UnitTests.TestContexts
{
    public abstract class TestContext
    {
        public SlaskContext SlaskContext { get; }

        protected TestContext()
        {
            SlaskContext = new SlaskContext(new DbContextOptionsBuilder()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options);
        }
    }
}
