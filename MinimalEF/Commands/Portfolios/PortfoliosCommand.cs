using System;

namespace MinimalEF.Commands.Portfolios
{
    public class PortfoliosCommand : IBranchCliCommand
    {
        public string Description => "\tportfolios,p\tManage portfolios";
        public Type ParentCommandType => null;
        public bool CanHandle(string command) => command.IsRoughly("portfolios", "p");
    }
}