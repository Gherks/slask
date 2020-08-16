using Microsoft.EntityFrameworkCore;
using Slask.Persistence;
using System;

namespace Slask.TestCore
{
    public static class InMemoryContextCreator
    {
        public static SlaskContext Create(string givenDatabaseName = "")
        {
            string databaseName = Guid.NewGuid().ToString();
            bool givenDatabaseNotEmpty = givenDatabaseName.Length > 0;
                
            if (givenDatabaseNotEmpty)
            {
                databaseName = givenDatabaseName;
            }

            return new SlaskContext(new DbContextOptionsBuilder()
                .UseLoggerFactory(SlaskContext.DebugLoggerFactory)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
                .UseInMemoryDatabase(databaseName: databaseName)
                .Options);
        }
    }
}