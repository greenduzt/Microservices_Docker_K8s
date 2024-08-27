using CommandsService.Models;

namespace CommandsService.Data;

public class CommandRepo : ICommandRepo
{
    private readonly AppDbContext _appDbContext;

    public CommandRepo(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public void CreateCommand(int platformId, Command command)
    {
        if(command == null)
        {
            throw new ArgumentNullException(nameof(command));
           
        }

        command.PlatformId = platformId;
        _appDbContext.Commands.Add(command);
    }

    public void CreatePlatform(Platform platform)
    {
        if(platform == null)
        {
            throw new ArgumentNullException(nameof(platform));
        }
        
        _appDbContext.Platforms.Add(platform);
        
    }

    public bool ExternalPlatformExists(int externalPlatformId)
    {
        return _appDbContext.Platforms.Any(p => p.Id == externalPlatformId);

    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
        return _appDbContext.Platforms.ToList();
    }

    public Command GetCommand(int platformId, int commandId)
    {
        return _appDbContext.Commands
                .Where(c => c.PlatformId == platformId && c.Id == commandId).FirstOrDefault();  
    }

    public IEnumerable<Command> GetCommandsForPlatform(int PlatformId)
    {
        return _appDbContext.Commands
                .Where(x => x.PlatformId == PlatformId)
                .OrderBy(x => x.Platform.Name);
    }

    public bool PlatFormExists(int PlatformId)
    {
        return _appDbContext.Platforms.Any(p => p.Id == PlatformId);
    }

    public bool SaveChanges()
    {
        return(_appDbContext.SaveChanges() >= 0);
    }
}