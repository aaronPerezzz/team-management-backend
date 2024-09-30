using Microsoft.EntityFrameworkCore;

namespace team_management_backend
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
