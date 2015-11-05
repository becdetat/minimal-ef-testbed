using System;

namespace MinimalEF.Domain
{
    public class PortfolioItem
    {
        private PortfolioItem()
        {
        }

        public PortfolioItem(Portfolio portfolio, Security security, decimal units)
        {
            this.Id = Guid.NewGuid();
            this.PortfolioId = portfolio.Id;
            this.SecurityId = security.Id;
            this.Units = units;
        }

        public Guid Id { get; private set; }
        public Guid PortfolioId { get; private set; }
        public Guid SecurityId { get; private set; }
        public decimal Units { get; private set; }

        public decimal Value1(decimal currentPrice) => this.Units*currentPrice;
        public decimal Value2(Security security) => this.Units*security.CurrentPrice;
    }
}