using System.Text;
using System.Text.Json;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.HTTP;

public class HttpCommandDataClient : ICommandDataClient
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration configuration;

    public HttpCommandDataClient(HttpClient httpClient, IConfiguration configuration)
    {
        this.httpClient = httpClient;
        this.configuration = configuration;
    }

    public async Task SendPlatformToCommand(PlatformReadDto platformReadDto)
    {
        var httpContent = new StringContent(
            JsonSerializer.Serialize(platformReadDto),Encoding.UTF8,"application/json");
    
        var response = await httpClient.PostAsync($"{configuration["CommandService"]}",httpContent);

        if(response.IsSuccessStatusCode)
        {
            System.Console.WriteLine("--> Sync POST to CommandService was succsessfull");
        }
        else
        {
            System.Console.WriteLine("Failed");
        }
    }
}
