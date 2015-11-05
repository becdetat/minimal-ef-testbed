using System;

namespace MinimalEF.Commands.Securities
{
    public class SecuritiesCommand : IBranchCliCommand
    {
        public string Description
        {
            get { return "\tsecurities\tManage securities"; }
        }

        public Type ParentCommandType => null;

        public bool CanHandle(string command)
        {
            return command.IsRoughly("securities") || command.IsRoughly("s");
        }
    }
}