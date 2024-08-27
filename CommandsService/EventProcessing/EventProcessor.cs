using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;

namespace CommandsService.EventProcessing;

public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _serviceScropFactory;
    private readonly IMapper _mapper;

    public EventProcessor(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
    {
        _serviceScropFactory = serviceScopeFactory;
        _mapper = mapper;
    }
    
    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);
        switch (eventType)  
        {
            case EventType.PlatformPublished:
                break;
            default:
                break;

        }
    }

    private EventType DetermineEvent(string notificationMessage)
    {
        System.Console.WriteLine("--> Dtermining event");

        var eventType =  JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

        switch (eventType.Event)
        {
            case "Platform_Published" : 
                System.Console.WriteLine("Platform published event detected");
                return EventType.PlatformPublished;
            default : 
                System.Console.WriteLine("--> Could not determine the event type");
                return EventType.Undertermined;
        }

    }

    private void AddPlatform(string platformPublishedMessage)
    {
        using(var scope = _serviceScropFactory.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();

            var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

            try
            {
                var plat = _mapper.Map<Platform>(platformPublishedDto);
                if(!repo.ExternalPlatformExists(plat.ExternalId))
                {
                    repo.CreatePlatform(plat);
                    repo.SaveChanges();
                }
                else
                {
                    System.Console.WriteLine("--> Platform already exists");
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine($"--> Could not add platform DB {ex.Message}");
            }
        }
    }
}

enum EventType
{
    PlatformPublished,
    Undertermined
}