using System;

namespace MinimalEF.Commands.Reports
{
    public class ReportsCommand : IBranchCliCommand
    {
        public string Description => "\treports\t\tReports";
        public Type ParentCommandType => null;
        public bool CanHandle(string command) => command.IsRoughly("reports");
    }
}