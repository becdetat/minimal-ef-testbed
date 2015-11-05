using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using MinimalEF.Commands.Common;
using MinimalEF.Domain;
using MinimalEF.Domain.Actions;

namespace MinimalEF.Commands.Portfolios
{
    public class AddItemCommand : ILeafCliCommand
    {
        private readonly IDbContext _context;
        private readonly GetPortfolio _getPortfolio;
        private readonly AddAssetToPortfolio _addAssetToPortfolio;

        public AddItemCommand(IDbContext context, GetPortfolio getPortfolio, AddAssetToPortfolio addAssetToPortfolio)
        {
            _context = context;
            _getPortfolio = getPortfolio;
            _addAssetToPortfolio = addAssetToPortfolio;
        }

        public string Description => "\tadditem\t\tAdd an item to a portfolio";
        public Type ParentCommandType => typeof (PortfoliosCommand);
        public bool CanHandle(string command) => command.IsRoughly("additem");

        public Task Execute()
        {
            var portfolio = _getPortfolio.Get();
            if (portfolio == null)
            {
                return SayWeAreCancelling();
            }

            var security = GetSecurity();
            if (security == null)
            {
                return SayWeAreCancelling();
            }

            Cmd.WriteLine("Units (leave blank to cancel)");
            var units = Cmd.Prompt();
            decimal unitsValue;
            if (string.IsNullOrEmpty(units))
            {
              return  SayWeAreCancelling();
            }
            if (!decimal.TryParse(units, out unitsValue))
            {
                Cmd.WriteErrorLine("Price is invalid");
                return SayWeAreCancelling();
            }

            // 1. DIY
            //var item = new PortfolioItem(portfolio, security, unitsValue);
            //_context.PortfolioItems.Add(item);

            // 2. aggregate root
            //portfolio.AddItem(_context, security, unitsValue);

            // 3. service
            _addAssetToPortfolio.Add(portfolio, security, unitsValue);

            _context.SaveChanges();

            return Task.CompletedTask;
        }

        Security GetSecurity()
        {
            Cmd.WriteLine("Security code (leave blank to cancel)");
            var code = Cmd.Prompt();
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }

            var query =
                from security in _context.Securities
                where security.Code.ToLower() == code.ToLower()
                select security;

            return query.SingleOrDefault();
        }

        Task SayWeAreCancelling()
        {
            Cmd.WriteInfoLine("Cancelling creating a portfolio");
            return Task.CompletedTask;
        }
    }
}