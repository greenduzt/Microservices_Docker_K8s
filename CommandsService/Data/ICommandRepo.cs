using CommandsService.Models;

namespace CommandsService.Data;

public interface ICommandRepo
{
    // Platforms
    bool SaveChanges();
    IEnumerable<Platform> GetAllPlatforms();
    void CreatePlatform(Platform platform);
    bool PlatFormExists(int PlatformId);
    
    // Commands
    IEnumerable<Command> GetCommandsForPlatform(int PlatformId);
    Command GetCommand(int platformId, int commandId);
    void CreateCommand(int platformId, Command command);
}