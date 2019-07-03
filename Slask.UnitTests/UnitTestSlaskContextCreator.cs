using Microsoft.EntityFrameworkCore;
using Slask.Persistance;
using Slask.TestCore;
using System;

namespace Slask.UnitTests
{
    public class UnitTestSlaskContextCreator : SlaskContextCreatorInterface
    {
        public override SlaskContext CreateContext()
        {
            return new SlaskContext(new DbContextOptionsBuilder()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options);
        }

        public override SlaskContext CreateContext(bool beginTransaction = false)
        {
            return CreateContext();
        }
    }
}