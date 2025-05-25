using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace ECommerce.Infrastructure.Persistence
{
    public class OrderDbContextFactory : IDesignTimeDbContextFactory<OrderDbContext>
    {
        public OrderDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<OrderDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection") 
                ?? "Host=postgres;Database=ecommerce;Username=postgres;Password=postgres";

            optionsBuilder.UseNpgsql(connectionString);

            return new OrderDbContext(optionsBuilder.Options);
        }
    }
} 