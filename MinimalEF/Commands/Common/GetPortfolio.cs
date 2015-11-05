using System.Linq;
using MinimalEF.Domain;

namespace MinimalEF.Commands.Common
{
    public class GetPortfolio
    {
        private readonly IDbContext _context;

        public GetPortfolio(IDbContext context)
        {
            _context = context;
        }

        public Portfolio Get()
        {
            Cmd.WriteLine("Portfolio name (leave blank to cancel)");
            var name = Cmd.Prompt();
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            var query =
                from portfolio in _context.Portfolios
                where portfolio.Name.ToLower() == name.ToLower()
                select portfolio;

            return query.SingleOrDefault();
        }
    }
}