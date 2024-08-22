using PlatformService.Models;

namespace PlatformService.Data;

public interface IPlatformRepo
{
    bool Save();
    IEnumerable<Platform> GetAllPlatforms();

    Platform GetPlatformById(int id);

    void CreatePlatform(Platform platofm);

    void DeletePlatform(int id);
}
