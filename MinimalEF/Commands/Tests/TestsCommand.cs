using System;

namespace MinimalEF.Commands.Tests
{
    public class TestsCommand : IBranchCliCommand
    {
        public string Description => "\ttest\t\tRandom tests";
        public Type ParentCommandType => null;
        public bool CanHandle(string command) => command.IsRoughly("test");
    }
}