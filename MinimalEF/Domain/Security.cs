using System;

namespace MinimalEF.Domain
{
    public class Security
    {
        private Security() { }

        public Security(string code, decimal currentPrice)
        {
            this.Id = Guid.NewGuid();
            this.Code = code;
            this.CurrentPrice = currentPrice;
        }

        public Guid Id { get; private set; }
        public string Code { get; private set; }
        public decimal CurrentPrice { get; private set; }

        public void SetCurrentPrice(decimal newPrice)
        {
            this.CurrentPrice = newPrice;
        }
    }
}