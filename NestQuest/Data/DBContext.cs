using Microsoft.EntityFrameworkCore;
using NestQuest.Data.Models;

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
        public DbSet<Models.Properties> Properties { get; set; }
        public DbSet<Models.Favorites> Favorites { get; set; }
        public DbSet<Models.Bookings> Bookings { get; set; }
        public DbSet<Models.Reportings> Reportings { get; set; }
        public DbSet<Models.Reviews> Reviews { get; set; }
        public DbSet<Models.Utilities> Utilities { get; set; }
        public DbSet<Models.Beds> Beds { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bookings>()
                .HasKey(b => new { b.BookingTime, b.Property_Id, b.Start_Date});

            modelBuilder.Entity<Reportings>()
               .HasKey(b => new { b.Guest_Id, b.Property_Id, b.Start_Date });

            modelBuilder.Entity<Favorites>()
               .HasKey(b => new { b.Guest_Id, b.Property_Id});
        }
    }
}
