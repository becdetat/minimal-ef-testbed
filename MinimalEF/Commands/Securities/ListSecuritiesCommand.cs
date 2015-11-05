using System;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalEF.Commands.Securities
{
    public class ListSecuritiesCommand : ILeafCliCommand
    {
        private readonly IDbContext _context;

        public ListSecuritiesCommand(IDbContext context)
        {
            _context = context;
        }

        public string Description => "\tlist,l\t\tList securities";
        public Type ParentCommandType => typeof (SecuritiesCommand);
        public bool CanHandle(string command) => command.IsRoughly("list", "l");

        public Task Execute()
        {
            var securities = _context.Securities.ToArray();

            Cmd.Dump(securities.Select(x => new
            {
                x.Code,
                x.CurrentPrice
            }));

            return Task.CompletedTask;
        }
    }
}