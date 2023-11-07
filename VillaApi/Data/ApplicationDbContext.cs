using Microsoft.EntityFrameworkCore;
using VillaApi.Models;

namespace VillaApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Villa> Villas { get; set; }

        public DbSet<Driver> Drivers { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa()
                {
                    Id = 1,
                    Name = "Test1",
                    Price = 4,
                    Area = 3000,
                },
                new Villa()
                {
                    Id = 2,
                    Name = "Test2",
                    Price = 5,
                    Area = 13000,
                },
                new Villa()
                {
                    Id = 3,
                    Name = "Test3",
                    Price = 2,
                    Area = 7000,
                },
                new Villa()
                {
                    Id = 4,
                    Name = "Test4",
                    Price = 1,
                    Area = 1000,
                });
                
                


        }
    }
}
