using Microsoft.EntityFrameworkCore;
using PlatformService.Models;

namespace PlatformService.Data;

public class ApplicationDBContext : DbContext
{
    public DbSet<Platform> Platforms{ get; set; }

    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> opt) : base(opt)
    {
        
    }
}
