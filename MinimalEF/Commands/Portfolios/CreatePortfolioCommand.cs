using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinimalEF.Domain;

namespace MinimalEF.Commands.Portfolios
{
    public class CreatePortfolioCommand : ILeafCliCommand
    {
        private readonly IDbContext _context;

        public CreatePortfolioCommand(IDbContext context)
        {
            _context = context;
        }

        public string Description => "\tcreate,new\tCreate a new portfolio";
        public Type ParentCommandType => typeof (PortfoliosCommand);
        public bool CanHandle(string command) => command.IsRoughly("create", "new");

        public Task Execute()
        {
            Cmd.WriteLine("Name (leave blank to cancel)");
            var name = Cmd.Prompt();
            if (string.IsNullOrEmpty(name))
            {
                return SayWeAreCancelling();
            }

            var portfolio = new Portfolio(name);

            _context.Portfolios.Add(portfolio);

            _context.SaveChanges();

            return Task.CompletedTask;
        }

        Task SayWeAreCancelling()
        {
            Cmd.WriteInfoLine("Cancelling creating a portfolio");
            return Task.CompletedTask;
        }
    }
}
