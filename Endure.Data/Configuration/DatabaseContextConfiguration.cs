using Endure.Data.Configuration.Interceptors;
using Endure.Data.Seeding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Endure.Data.Configuration;

public static class DatabaseContextConfiguration
{
    /// <summary>
    /// Configures the databasecontext, and registers the services that i may use / depend on.
    /// </summary>
    public static IServiceCollection ConfigureDatabaseContext(this IServiceCollection services, string npgConnectionString)
    {
        services.AddSingleton<BaseModelInterceptor>();

        return services.AddDbContext<DatabaseContext>((sp, context) =>
            context.UseNpgsql(npgConnectionString)
                .AddInterceptors(sp.GetRequiredService<BaseModelInterceptor>())
                .UseSeeding((dbContext, _) =>
                {
                    var context = (DatabaseContext)dbContext;

                    context
                        .SeedWarehouse()
                        .SaveChanges();
                })
                .UseAsyncSeeding(async (dbContext, _, ctx) =>
                {
                    var context = (DatabaseContext)dbContext;

                    await context.SeedWarehouseAsync();

                    await context.SaveChangesAsync(ctx);
                })
        );
    }
}
