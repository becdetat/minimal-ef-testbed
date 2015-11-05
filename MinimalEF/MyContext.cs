using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using MinimalEF.Domain;

namespace MinimalEF
{
    public interface IDbContext
    {
        IDbSet<Security> Securities { get; }
        IDbSet<Portfolio> Portfolios { get; set; }
        IDbSet<PortfolioItem> PortfolioItems { get; set; }
        int SaveChanges();
    }

    public class MyContext : DbContext, IDbContext
    {
        public MyContext(string connectionString)
            : base(connectionString)
        {
        }

        public IDbSet<Security> Securities { get; set; }
        public IDbSet<Portfolio> Portfolios { get; set; } 
        public IDbSet<PortfolioItem> PortfolioItems { get; set; }
    }
}