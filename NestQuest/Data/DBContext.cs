using Microsoft.EntityFrameworkCore;

namespace NestQuest.Data
{
    public class DBContext: DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {

        }

        public DbSet<Models.Users> Users { get; set; }
        public DbSet<Models.Hosts> Host { get; set; }
        public DbSet<Models.Guest> Guest { get; set; }
    }
}
