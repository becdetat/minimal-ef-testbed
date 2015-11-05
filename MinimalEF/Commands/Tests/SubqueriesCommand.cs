using System;
using System.Linq;
using System.Threading.Tasks;

namespace MinimalEF.Commands.Tests
{
    public class SubqueriesCommand : ILeafCliCommand
    {
        private readonly IDbContext _context;

        public SubqueriesCommand(IDbContext context)
        {
            _context = context;
        }

        public string Description => "\tsubqueries\tSubqueries";
        public Type ParentCommandType => typeof (TestsCommand);
        public bool CanHandle(string command) => command.IsRoughly("subqueries");

        public Task Execute()
        {
            var query =
                from portfolio in _context.Portfolios
                where portfolio.Name == "Ben Scott"
                let items =
                    from item in _context.PortfolioItems
                    where item.PortfolioId == portfolio.Id
                    // Fails: "The methods 'Single' and 'SingleOrDefault' can only be used as a final query operation. Consider using the method 'FirstOrDefault' in this instance instead."
                    //let security = _context.Securities.Single(x => x.Id == item.SecurityId)
                    let security = _context.Securities.FirstOrDefault(x => x.Id == item.SecurityId)
                    select security.Code
                select new
                {
                    portfolio,
                    items
                };
            var result = query.Single();

            Cmd.DumpCollection("Security codes held by portfolio", result.items);

            return Task.CompletedTask;
        }
    }
}