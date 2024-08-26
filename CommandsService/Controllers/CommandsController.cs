using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/c/platforms/{platformId}/[controller]")]
[ApiController]
public class CommandsController : Controller
{
    private readonly ICommandRepo commandRepo;  
    private readonly IMapper mapper;

    public CommandsController(ICommandRepo commandRepo, IMapper mapper)
    {
        this.commandRepo = commandRepo;
        this.mapper = mapper;
    }

    [HttpGet]
    public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
    {
        System.Console.WriteLine($"--> Hit GetCommandsForPlatform: {platformId}");  

        if(!commandRepo.PlatFormExists(platformId))
        {
            return NotFound(); // Http 404
        }
        
        var commands = commandRepo.GetCommandsForPlatform(platformId);

        return Ok(mapper.Map<IEnumerable<CommandReadDto>>(commands));
    }

    [HttpGet("{commandId}", Name = "GetCommandForPlatform")]

    public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
    {
        System.Console.WriteLine($"--> Hit GetCommandForPlatform: {platformId} | {commandId}");

        if(!commandRepo.PlatFormExists(platformId))
        {
            return NotFound();
        }

        var command = commandRepo.GetCommand(platformId,commandId);
        
        if(command == null )
        {
            return NotFound();
        }

        return Ok(mapper.Map<CommandReadDto>(command));
    }

    [HttpPost] 
    public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandDto)
    {
        System.Console.WriteLine($"--> Hit CreateCommandForPlatform: {platformId}");  

        if(!commandRepo.PlatFormExists(platformId))
        {
            return NotFound(); // Http 404
        }

        var command = mapper.Map<Command>(commandDto);

        commandRepo.CreateCommand(platformId, command);
        commandRepo.SaveChanges();

        var commandReadDto = mapper.Map<CommandReadDto>(command);

        return CreatedAtRoute(nameof(GetCommandForPlatform), new { platformId, commandId  = commandReadDto.Id},commandReadDto);
    }
}