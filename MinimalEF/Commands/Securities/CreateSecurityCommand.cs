using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinimalEF.Domain;

namespace MinimalEF.Commands.Securities
{
    public class CreateSecurityCommand : ILeafCliCommand
    {
        private readonly IDbContext _context;

        public CreateSecurityCommand(IDbContext context)
        {
            _context = context;
        }

        public string Description => "\tcreate\tCreate a new security";
        public Type ParentCommandType => typeof (SecuritiesCommand);
        public bool CanHandle(string command) => command.IsRoughly("create", "new");

        public Task Execute()
        {
            Cmd.WriteLine("Code (leave blank to cancel)");
            var code = Cmd.Prompt();
            if (string.IsNullOrEmpty(code))
            {
                return SayWeAreCancelling();
            }

            Cmd.WriteLine("Current price (leave blank to cancel)");
            var priceValue = Cmd.Prompt();
            decimal currentPrice;
            if (string.IsNullOrEmpty(priceValue))
            {
              return  SayWeAreCancelling();
            }
            if (!decimal.TryParse(priceValue, out currentPrice))
            {
                Cmd.WriteErrorLine("Price is invalid");
                return SayWeAreCancelling();
            }

            var security = new Security(code, currentPrice);

            _context.Securities.Add(security);

            _context.SaveChanges();

            return Task.CompletedTask;
        }

        Task SayWeAreCancelling()
        {
            Cmd.WriteInfoLine("Cancelling creating a security");
            return Task.CompletedTask;
        }
    }
}
