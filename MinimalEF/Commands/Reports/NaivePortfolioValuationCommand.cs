using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinimalEF.Commands.Common;

namespace MinimalEF.Commands.Reports
{
    public class NaivePortfolioValuationCommand : ILeafCliCommand
    {
        private readonly GetPortfolio _getPortfolio;
        private readonly IDbContext _context;

        public NaivePortfolioValuationCommand(GetPortfolio getPortfolio, IDbContext context)
        {
            _getPortfolio = getPortfolio;
            _context = context;
        }

        public string Description => "\tpv1\t\tNaive portfolio valuation";
        public Type ParentCommandType => typeof (ReportsCommand);
        public bool CanHandle(string command) => command.IsRoughly("pv1");

        public Task Execute()
        {
            var p = _getPortfolio.Get();
            if (p == null) return SayWeAreCancelling();
            var portfolioId = p.Id;

            var query =
                from portfolio in _context.Portfolios
                where portfolio.Id == portfolioId
                let items =
                    from item in _context.PortfolioItems
                    where item.PortfolioId == portfolio.Id
                    let security = _context.Securities.FirstOrDefault(s => s.Id == item.SecurityId)
                    select new
                    {
                        security.Code,
                        Value = item.Units*security.CurrentPrice
                    }
                select items;

            Cmd.Dump(query.Single());

            return Task.CompletedTask;
        }

        Task SayWeAreCancelling()
        {
            Cmd.WriteInfoLine("Cancelling creating a portfolio");
            return Task.CompletedTask;
        }
    }
}
