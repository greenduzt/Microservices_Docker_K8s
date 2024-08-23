using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers;

[Route("api/c/platforms{platformId}/[controller]")]
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
            return NotFound(); // Http 401
        }

    }


}