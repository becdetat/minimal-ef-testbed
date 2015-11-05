namespace MinimalEF.Domain.Actions
{
    public class AddAssetToPortfolio
    {
        private readonly IDbContext _context;

        public AddAssetToPortfolio(IDbContext context)
        {
            _context = context;
        }

        public void Add(Portfolio portfolio, Security security, decimal units)
        {
            var item = new PortfolioItem(portfolio, security, units);

            _context.PortfolioItems.Add(item);
        }
    }
}