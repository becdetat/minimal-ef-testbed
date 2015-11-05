using System;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalEF.Commands.Portfolios
{
    public class ListPortfoliosCommand : ILeafCliCommand
    {
        private readonly IDbContext _context;

        public ListPortfoliosCommand(IDbContext context)
        {
            _context = context;
        }

        public string Description => "\tlist,l\t\tList portfolios";
        public Type ParentCommandType => typeof (PortfoliosCommand);
        public bool CanHandle(string command) => command.IsRoughly("list", "l");

        public Task Execute()
        {
            var portfolios = _context.Portfolios.ToArray();

            Cmd.Dump(portfolios.Select(x => new {x.Name}));

            return Task.CompletedTask;
        }
    }
}