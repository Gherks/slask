using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Slask.Persistence;
using Slask.TestCore;

namespace Slask.IntegrationTests
{
    public class IntegrationTestSlaskContextCreator : SlaskContextCreatorInterface
    {
        public override SlaskContext CreateContext()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            builder.UseSqlServer(SlaskTestConnectionString.ConnectionString);

            return new SlaskContext(builder.Options);
        }

        public override SlaskContext CreateContext(bool beginTransaction = false)
        {
            SlaskContext slaskTestContext = CreateContext();

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
