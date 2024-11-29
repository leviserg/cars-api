using cars_api.Models;
using Microsoft.EntityFrameworkCore;

namespace cars_api.Data
{
    public class DbSeed
    {
        private readonly AppDbContext _dbContext;

        public DbSeed(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SeedCars()
        {
            _dbContext.Cars.AddRange(
                new Car { Name = "Corolla", Year = 2020, BrandId = 1 },
                new Car { Name = "Mustang", Year = 1963, BrandId = 2 },
                new Car { Name = "Corvette", Year = 1963, BrandId = 3 },
                new Car { Name = "Maxima", Year = 2005, BrandId = 4 }
            );
            _dbContext.SaveChanges();
        }

        public void SeedBrands()
        {
            _dbContext.Brands.AddRange(
                new Brand { Name = "Toyota" },
                new Brand { Name = "Ford"},
                new Brand { Name = "Chevrolet" },
                new Brand { Name = "Nissan" }
            );
            _dbContext.SaveChanges();
        }
    }
}
