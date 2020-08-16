using Microsoft.EntityFrameworkCore;
using Slask.Persistence;
using System;

namespace Slask.TestCore
{
    public static class InMemoryContextCreator
    {
        public static SlaskContext Create(string specifiedDatabaseName = "")
        {
            string givenDatabaseName = Guid.NewGuid().ToString();

            bool specifiedDatabaseNameNotEmpty = specifiedDatabaseName.Length > 0;                
            if (specifiedDatabaseNameNotEmpty)
            {
                givenDatabaseName = specifiedDatabaseName;
            }

            return new SlaskContext(new DbContextOptionsBuilder()
                .UseLoggerFactory(SlaskContext.DebugLoggerFactory)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .UseInMemoryDatabase(databaseName: givenDatabaseName)
                .Options);
        }
    }
}