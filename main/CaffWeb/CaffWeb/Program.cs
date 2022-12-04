using CaffDal;
using CaffDal.Identity;
using Microsoft.EntityFrameworkCore;

namespace CaffWeb
{
    public class Program
    {
        public static async Task Main(string[] args) =>
#if DEBUG
            (await
            CreateHostBuilder(args)
                .Build()
                .MigrateDataBaseAsync<CaffDbContext>())
                .Run();
#else
            CreateHostBuilder(args).Build().Run();
#endif


        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }

    public static class HostDataExtensions
    {
        public static async Task<IHost> MigrateDataBaseAsync<TContext>(this IHost host)
            where TContext : DbContext
        {
            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var context = serviceProvider.GetRequiredService<TContext>();
                context.Database.Migrate();

                var roleSeeder = serviceProvider.GetRequiredService<RoleSeedService>();
                await roleSeeder.SeedRoleAsync();

                var userSeeder = serviceProvider.GetRequiredService<UserSeedService>();
                await userSeeder.SeedUserAsync();
            }

            return host;
        }
    }
}
