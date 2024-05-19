using Microsoft.EntityFrameworkCore;
using WebAPICba.Models;

namespace WebAPICba.Data
{
    public class ExchangeRateContext : DbContext
    {
        public ExchangeRateContext(DbContextOptions<ExchangeRateContext> options) : base(options) { }

        public DbSet<ExchangeRate> ExchangeRates { get; set; }
    }
}
