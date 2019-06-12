using Microsoft.EntityFrameworkCore;
using Slask.Data;
using System.Data.SqlClient;

namespace Slask.IntegrationTests
{
    public class IntegrationTestBase
    {
        protected static SlaskContext CreateSlaskTestContext(bool beginTransaction = true)
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            builder.UseSqlServer(SlaskTestConnectionString.ConnectionString);

            SlaskContext slaskTestContext = new SlaskContext(builder.Options);

            if (beginTransaction)
            {
                slaskTestContext.Database.BeginTransaction();
            }

            return slaskTestContext;
        }

        private static SqlConnectionStringBuilder SlaskTestConnectionString =>
            new SqlConnectionStringBuilder
            {
                DataSource = @"(LocalDB)\MSSQLLocalDB",
                InitialCatalog = "SlaskTest",
                IntegratedSecurity = true
            };
    }
}
