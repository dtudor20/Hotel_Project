using Hotel.Application.QueryHandlers;
using Hotel.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Application.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Register DbContext
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<HotelDbContext>(options =>
            options.UseSqlServer(connectionString, 
                sqlOptions => sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "dbo")));

        // Register query handlers
        services.AddScoped<GetAllRoomsQueryHandler>();

        return services;
    }
}
