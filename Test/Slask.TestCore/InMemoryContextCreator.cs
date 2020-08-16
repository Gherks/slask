using Microsoft.EntityFrameworkCore;
using Slask.Persistence;
using System;

namespace Slask.TestCore
{
    public static class InMemoryContextCreator
    {
        public static SlaskContext Create()
        {
            return new SlaskContext(new DbContextOptionsBuilder()
                .UseLoggerFactory(SlaskContext.DebugLoggerFactory)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options);
        }
    }
}