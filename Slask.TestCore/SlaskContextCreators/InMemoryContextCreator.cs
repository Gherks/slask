using Microsoft.EntityFrameworkCore;
using Slask.Persistence;
using System;

namespace Slask.UnitTests
{
    public class InMemoryContextCreator
    {
        public static SlaskContext Create()
        {
            return new SlaskContext(new DbContextOptionsBuilder()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options);
        }
    }
}