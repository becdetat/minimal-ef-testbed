using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using MinimalEF.Commands;

namespace MinimalEF
{
    internal class Program
    {
        private const string ConnectionString = @"Data Source=.\sqlexpress;Initial Catalog=MinimalEF;Integrated Security=True";

        private static void Main(string[] args)
        {
            try
            {
                Cmd.WriteHeader("Initialising...");
                var upTheDb = new UpTheDb(ConnectionString);
                if (!upTheDb.DatabaseExists()) upTheDb.CreateDatabase();
                if (!upTheDb.PerformMigrations())
                {
                    Cmd.Pause();
                    return;
                }
            }
            catch (Exception ex)
            {
                Cmd.WriteException(ex);
                Cmd.Pause();
                throw;
            }

            var container = IoC.HaveYouAnyIoC(ConnectionString);
            var commands = container
                .Resolve<IEnumerable<ICliCommand>>()
                .ToArray();
            var quitCommand = commands.OfType<QuitCommand>().Single();

            CliAppLoop.StartAppLoop(commands, quitCommand);
        }
    }
}