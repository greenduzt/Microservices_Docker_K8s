using CommandsService.Models;
using CommandsService.SyncDataServices;

namespace CommandsService.Data;

public static class PrepDb
{
    public static void PrepPopulation(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
            if (grpcClient == null)
            {
                throw new Exception("GRPC client is not available.");
            }

            var platforms = grpcClient.ReturnAllPlatforms();
            if (platforms == null || !platforms.Any())
            {
                Console.WriteLine("No platforms retrieved from GRPC service.");
                return;
            }

            SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>(), platforms);
        }
    }
    
    private static void SeedData(ICommandRepo repo, IEnumerable<Platform> platforms)
    {
        if (repo == null)
        {
            throw new Exception("Command repository is not available.");
        }

        Console.WriteLine("Seeding new platforms...");

        foreach (var plat in platforms)
        {
            if (!repo.ExternalPlatformExists(plat.ExternalId))
            {
                repo.CreatePlatform(plat);
            }
            repo.SaveChanges();
        }
    }
}
