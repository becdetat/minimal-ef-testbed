using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalEF
{
    public interface ICliCommand
    {
        string Description { get; }
        Type ParentCommandType { get; }
        bool CanHandle(string command);
    }

    public interface IBranchCliCommand : ICliCommand
    {
    }

    public interface ILeafCliCommand : ICliCommand
    {
        Task Execute();
    }

    public static class CliAppLoop
    {
        public static void StartAppLoop(ICliCommand[] commands, ILeafCliCommand quitCommand)
        {
            Cmd.WriteHeader("Ready");

            Cmd.WriteLine("'?' for help");

            Type currentCommandType = null;

            do
            {
                var path = GetCommandInheritanceChain(currentCommandType, commands)
                    .Reverse()
                    .Aggregate(string.Empty, (current, commandType) => string.Format("{0}.{1}", current, commandType.Name.Replace("Command", string.Empty)));
                if (path.StartsWith("."))
                {
                    path = path.Substring(1);
                }
                var prompt = Cmd.Prompt(path);

                var type = currentCommandType;
                var childCommands = commands.Where(x => x.ParentCommandType == type)
                    .Union(new[] { quitCommand });

                if (prompt.IsRoughly("?", "help"))
                {
                    ShowHelp(childCommands, currentCommandType != null);
                    continue;
                }
                if (prompt.IsRoughly(".."))
                {
                    if (currentCommandType != null)
                    {
                        var currentCommand = commands.Single(x => x.GetType() == currentCommandType);
                        currentCommandType = commands.Where(x => x.GetType() == currentCommand.ParentCommandType)
                            .Select(x => x.GetType())
                            .SingleOrDefault();
                    }
                    continue;
                }

                var command = childCommands.SingleOrDefault(x => x.CanHandle(prompt));

                if (command == null)
                {
                    Cmd.WriteWarningLine("Command not recognised");
                    continue;
                }

                if (command is ILeafCliCommand)
                {
                    try
                    {
                        (command as ILeafCliCommand).Execute().Wait();
                    }
                    catch (Exception ex)
                    {
                        Cmd.WriteException(ex);
                    }
                }
                else
                {
                    currentCommandType = command.GetType();
                }
            } while (true);
            // ReSharper disable once FunctionNeverReturns
        }

        private static IEnumerable<Type> GetCommandInheritanceChain(Type currentType, ICliCommand[] commands)
        {
            while (true)
            {
                if (currentType == null)
                {
                    yield break;
                }

                yield return currentType;

                var currentCommand = commands.Single(x => x.GetType() == currentType);
                currentType = currentCommand.ParentCommandType;
            }
        }

        private static void ShowHelp(IEnumerable<ICliCommand> commands, bool atRoot)
        {
            Cmd.WriteLine("Commands:");
            if (!atRoot)
            {
                Cmd.WriteLine("\t..\t\tMove up a level");
            }
            foreach (var command in commands)
            {
                Cmd.WriteLine(command.Description);
            }
        }
    }
}