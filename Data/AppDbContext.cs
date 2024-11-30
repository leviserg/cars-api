using cars_api.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace cars_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { 
            Database.EnsureCreated();
        }
        
        public DbSet<Car> Cars { get; set; }
        public DbSet<Brand> Brands { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<Car>()
                .HasIndex(c => new { c.Id, c.BrandId });

            modelBuilder.Entity<Car>()
                .HasOne(c => c.Brand)
                .WithMany(b => b.Cars)
                .HasForeignKey(c => c.BrandId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
