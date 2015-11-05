using System;
using System.Threading.Tasks;

namespace MinimalEF.Commands
{
    public class QuitCommand : ILeafCliCommand
    {
        public bool CanHandle(string command)
        {
            return command.IsRoughly("q", "quit", "exit");
        }

        public Task Execute()
        {
            Cmd.WriteLine("Closing");
            Environment.Exit(0);

            return Task.FromResult(0);
        }

        public string Description
        {
            get { return "\tq, quit, exit\tQuit"; }
        }

        public Type ParentCommandType
        {
            get { return null; }
        }
    }
}