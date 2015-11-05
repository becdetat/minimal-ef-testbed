using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalEF.Commands.Securities
{
    public class RandomisePricesCommand : ILeafCliCommand
    {
        private readonly IDbContext _context;

        public RandomisePricesCommand(IDbContext context)
        {
            _context = context;
        }

        public string Description => "\trndprices\tRandomise prices";
        public Type ParentCommandType => typeof (SecuritiesCommand);
        public bool CanHandle(string command) => command.IsRoughly("rndprices");

        public Task Execute()
        {
            var securities = _context.Securities.ToArray();
            var random = new Random();

            foreach (var security in securities)
            {
                var newPrice = (decimal) random.Next(100, 1000)/100;
                Cmd.WriteLine($"Changing {security.Code} from {security.CurrentPrice:c} to {newPrice:c}");
                security.SetCurrentPrice(newPrice);
            }

            _context.SaveChanges();

            return Task.CompletedTask;
        }
    }
}
