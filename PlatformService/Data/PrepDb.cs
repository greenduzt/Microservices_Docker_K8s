using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder applicationBuilder, bool isProd)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            SeedData(serviceScope.ServiceProvider.GetService<ApplicationDBContext>(), isProd);
        }
    }

    private static void SeedData(ApplicationDBContext appDbContext, bool isProd)
    {

        if(isProd)
        {
            System.Console.WriteLine("--> Attempting to apply migrations...");
            
            try 
            {
                appDbContext.Database.Migrate();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"--> Could not run migrations: {ex.Message}");
            }
        }

        if (!appDbContext.Platforms.Any())
        {
            Console.WriteLine("--> Seeding data");

            appDbContext.AddRange(new Platform { Name = "Dot Net", Publisher = "Microsoft", Cost="Free" },
                                  new Platform { Name = "SQL Server Express", Publisher = "Microsoft", Cost = "Free" },
                                  new Platform { Name = "Kubernetes", Publisher = "Cloud Native Computing Foundation", Cost = "Free" }
                );
            appDbContext.SaveChanges();
        }
        else
        {
            Console.WriteLine("--> We already have data");
        }
    }
}
