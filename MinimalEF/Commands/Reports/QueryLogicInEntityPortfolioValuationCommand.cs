using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinimalEF.Commands.Common;

namespace MinimalEF.Commands.Reports
{
    public class QueryLogicInEntityPortfolioValuationCommand : ILeafCliCommand
    {
        private readonly GetPortfolio _getPortfolio;
        private readonly IDbContext _context;

        public QueryLogicInEntityPortfolioValuationCommand(GetPortfolio getPortfolio, IDbContext context)
        {
            _getPortfolio = getPortfolio;
            _context = context;
        }

        public string Description => "\tpv2\t\tQuery logic in entity PV";
        public Type ParentCommandType => typeof(ReportsCommand);
        public bool CanHandle(string command) => command.IsRoughly("pv2");

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
                        item,
                        security
                    }
                select items;
            var results =
                from item in query.Single().ToArray()
                select new
                {
                    item.security.Code,
                    Value1 = item.item.Value1(item.security.CurrentPrice),
                    Value2 = item.item.Value2(item.security)
                };

            Cmd.Dump(results);

            return Task.CompletedTask;
        }

        Task SayWeAreCancelling()
        {
            Cmd.WriteInfoLine("Cancelling creating a portfolio");
            return Task.CompletedTask;
        }
    }
}
