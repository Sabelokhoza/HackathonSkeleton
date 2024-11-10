using Microsoft.EntityFrameworkCore;

namespace HackathonAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
                
        }
    }
}
