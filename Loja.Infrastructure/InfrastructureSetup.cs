using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Loja.Domain.Interfaces;
using Loja.Infrastructure.Data;
using Loja.Infrastructure.Data.Repositories;
using Loja.Infrastructure.EventPublisher;
using Loja.Infrastructure.Repositories;

namespace Loja.Infrastructure
{
    public static class InfrastructureSetup
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));

            services.AddScoped<ISaleRepository, SaleRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();

            services.AddScoped<IEventPublisher, LogEventPublisher>();

            return services;
        }
    }
}
