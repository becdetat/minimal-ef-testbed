using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalEF.Commands.Tests
{
    public class SecondQueryCommand : ILeafCliCommand
    {
        private readonly IDbContext _context;

        public SecondQueryCommand(IDbContext context)
        {
            _context = context;
        }

        public string Description => "\tsecondquery\tSecond query";
        public Type ParentCommandType => typeof(TestsCommand);
        public bool CanHandle(string command) => command.IsRoughly("secondquery");

        public Task Execute()
        {
            var query =
                from portfolio in _context.Portfolios
                where portfolio.Name == "Ben Scott"
                let items =
                    from item in _context.PortfolioItems
                    where item.PortfolioId == portfolio.Id
                    select item
                select new {portfolio, items};

            var result = query.Single();

            // later..

            var specifiedItem = result.items.ToArray().First();

            var security = _context.Securities.Single(x => x.Id == specifiedItem.SecurityId);
            Cmd.WriteSubheader($"Entire security");
            Cmd.Dump(security);

            var justTheCode = _context.Securities
                .Where(x => x.Id == specifiedItem.SecurityId)
                .Select(x => x.Code)
                .Single();
            Cmd.WriteSubheader("Just the code");
            Cmd.WriteLine(justTheCode);

            return Task.CompletedTask;
        }
    }
}
