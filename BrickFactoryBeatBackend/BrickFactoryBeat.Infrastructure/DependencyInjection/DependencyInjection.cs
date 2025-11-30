using BrickFactoryBeat.Application.Services;
using BrickFactoryBeat.Domain.Equipment;
using BrickFactoryBeat.Domain.Orders;
using BrickFactoryBeat.Domain.StateHistory;
using BrickFactoryBeat.Infrastructure.Persistence;
using BrickFactoryBeat.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BrickFactoryBeat.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Registers all Infrastructure and Application dependencies - to be used with the webapi project.
        /// </summary>
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            
            // Register repositories (Infrastructure implementations)
            services.AddScoped<IEquipmentRepository, EquipmentRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IStateHistoryRepository, StateHistoryRepository>();

            // Register Application services
            services.AddScoped<IEquipmentService, EquipmentService>();
            
            
            // TODO consider optimization if time
            
            return services;
        }
    }
}