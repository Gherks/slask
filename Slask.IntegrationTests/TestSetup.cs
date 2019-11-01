using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Slask.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Slask.IntegrationTests
{
    public class TestSetup : IntegrationTestSlaskContextCreator, IDisposable
    {
        public TestSetup()
        {
            DestroyDatabase();
            CreateDatabase();
        }

        public void Dispose()
        {
            DestroyDatabase();
        }

        public void CreateDatabase()
        {
            ExecuteSqlCommand(Master, $@"
                CREATE DATABASE [SlaskTest]
                ON (NAME = 'SlaskTest',
                FILENAME = '{Filename}')");

            using (SlaskContext slaskTestContext = CreateContext(beginTransaction: false))
            {
                slaskTestContext.Database.Migrate();
                SlaskTestDatabaseSeeder.Seed(slaskTestContext);
                slaskTestContext.SaveChanges();
            }
        }

        public void DestroyDatabase()
        {
            var filenames = ExecuteSqlQuery(Master,
                @"SELECT [physical_name] FROM [sys].[master_files]
                WHERE [database_id] = DB_ID('SlaskTest')",
                row => (string)row["physical_name"]);

            if (filenames.Any())
            {
                ExecuteSqlCommand(Master,
                    @"ALTER DATABASE [SlaskTest] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                    EXEC sp_detach_db 'SlaskTest'");

                filenames.ForEach(File.Delete);
            }
        }

        private static void ExecuteSqlCommand(SqlConnectionStringBuilder connectionStringBuilder, string commandText)
        {
            using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = commandText;
                    command.ExecuteNonQuery();
                }
            }
        }

        private static List<T> ExecuteSqlQuery<T>(
            SqlConnectionStringBuilder connectionStringBuilder,
            string queryText,
            Func<SqlDataReader, T> read)
        {
            var result = new List<T>();
            using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = queryText;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            result.Add(read(reader));
                        }
                    }
                }
            }
            return result;
        }

        private static SqlConnectionStringBuilder Master =>
            new SqlConnectionStringBuilder
            {
                DataSource = @"(LocalDB)\MSSQLLocalDB",
                InitialCatalog = "master",
                IntegratedSecurity = true
            };

        private static string Filename => Path.Combine(
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
            "SlaskTestDB.mdf");
    }
}
