
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.HTTP;

namespace PlatformService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class PlatformController : ControllerBase
{
    private readonly IPlatformRepo repo;
    private readonly IMapper mapper;
    private readonly ICommandDataClient commandDataClient;

    public PlatformController(IPlatformRepo repo, IMapper mapper, ICommandDataClient commandDataClient)
    {
        this.repo = repo;
        this.mapper = mapper;
        this.commandDataClient = commandDataClient;
    }
    [HttpGet]
    public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms()
    {
        Console.WriteLine("--> Getting platforms");
        var platformItems = repo.GetAllPlatforms();

        // Map to IEnumerable of PlatformReadDtos from platformItems models to 

        return Ok(mapper.Map<IEnumerable<PlatformReadDto>>(platformItems));
    }
    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<PlatformReadDto> GetPlatformById(int id)
    {
        var platformItem = repo.GetPlatformById(id);

        if (platformItem != null)
        {

            return Ok(mapper.Map<PlatformReadDto>(platformItem));
        }

        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
    {
        var platformModel = mapper.Map<Platform>(platformCreateDto);

        repo.CreatePlatform(platformModel);
        repo.Save();

        var platformReadDto = mapper.Map<PlatformReadDto>(platformModel);

        try
        {
            await commandDataClient.SendPlatformToCommand(platformReadDto);
        }
        catch(Exception ex)
        {
            System.Console.WriteLine($"Could not send synchronously.{ex.Message}");
        }

        return CreatedAtRoute(nameof(GetPlatformById), new {id=platformReadDto.Id},platformReadDto);
    }


}
