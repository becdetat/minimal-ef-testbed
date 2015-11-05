using System;
using System.Data.SqlClient;
using System.Reflection;
using DbUp;

namespace MinimalEF
{
    public class UpTheDb
    {
        private readonly string _connectionString;

        public UpTheDb(string connectionString)
        {
            _connectionString = connectionString;
        }

         private string MasterConnectionString
        {
            get
            {
                var builder = new SqlConnectionStringBuilder(_connectionString) {InitialCatalog = "master"};
                return builder.ConnectionString;
            }
        }

        private string DatabaseName
        {
            get
            {
                var builder = new SqlConnectionStringBuilder(_connectionString);
                return builder.InitialCatalog;
            }
        }

        public bool DatabaseExists()
        {
            Cmd.WriteInfoLine("Checking for database");
            using (var connection = new SqlConnection(MasterConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"SELECT COUNT(*) FROM [sys].[databases] WHERE [Name] = '{DatabaseName}'";

                    return (int) command.ExecuteScalar() > 0;
                }
            }
        }

        public void CreateDatabase()
        {
            Cmd.WriteInfoLine("Creating database");
            using (var connection = new SqlConnection(MasterConnectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"CREATE DATABASE [{DatabaseName}]";

                    command.ExecuteNonQuery();
                }
            }
        }

        public bool PerformMigrations()
        {
            Cmd.WriteInfoLine("Performing migrations");

            var result = DeployChanges
                .To.SqlDatabase(_connectionString)
                .WithScriptsAndCodeEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build()
                .PerformUpgrade();

            return result.Successful;
        }
    }
}