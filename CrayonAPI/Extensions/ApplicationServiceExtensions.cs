using CrayonAPI.Data;
using CrayonAPI.Interfaces;
using CrayonAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace CrayonAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddServicesToContainer(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddDbContext<CrayonDbContext>(opt =>
                opt.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerService, CustomerService>();

            services.AddScoped<ICCPService, CCPService>();

            services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();

            return services;
        }
    }
}
