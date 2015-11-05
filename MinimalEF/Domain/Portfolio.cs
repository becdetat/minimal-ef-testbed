using System;

namespace MinimalEF.Domain
{
    public class Portfolio
    {
        private Portfolio()
        {
        }

        public Portfolio(string name)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }

        public PortfolioItem AddItem(IDbContext context, Security security, decimal units)
        {
            var item = new PortfolioItem(this, security, units);

            context.PortfolioItems.Add(item);

            return item;
        }
    }
}