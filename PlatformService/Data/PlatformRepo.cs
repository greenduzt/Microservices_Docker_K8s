using PlatformService.Models;

namespace PlatformService.Data;

public class PlatformRepo : IPlatformRepo
{
    private readonly ApplicationDBContext _dbContext;

    public PlatformRepo(ApplicationDBContext applicationDBContext)
    {
        _dbContext = applicationDBContext;
    }

    public void CreatePlatform(Platform platofm)
    {
        if (platofm == null)
        {
            throw new ArgumentException(nameof(platofm));
        }

        _dbContext.Platforms.Add(platofm);
    }

    public void DeletePlatform(int id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Platform> GetAllPlatforms()
    {
        return _dbContext.Platforms.ToList();
    }

    public Platform GetPlatformById(int id)
    {
        return _dbContext.Platforms.FirstOrDefault(x=>x.Id == id);
    }

    public bool Save()
    {
        return (_dbContext.SaveChanges() > 0);
    }
}
