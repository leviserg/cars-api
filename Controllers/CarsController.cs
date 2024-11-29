using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cars_api.Data;
using cars_api.Models;
using cars_api.Extensions;
using System.Linq.Expressions;

namespace cars_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CarsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Cars
        [HttpGet]
        public async Task<IActionResult> GetCars(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "name",
            [FromQuery] string sortOrder = "asc"
            )
        {
            // Ensure valid pagination parameters
            page = page < 1 ? 1 : page;
            pageSize = pageSize < 1 ? 10 : pageSize;

            // Base query
            var query = _context.Cars.Include(c => c.Brand).ApplyCarSorting(sortBy, sortOrder);

            // Apply pagination
            var cars = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new Car
                    {
                        Id = c.Id,
                        Brand = c.Brand,
                        Name = c.Name,
                        Year = c.Year,
                        LastChangedAtUtc = c.LastChangedAtUtc
                    }
                )
                .ToListAsync();

            var totalCount = await query.CountAsync();

            // Return response with metadata
            return Ok(new
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Cars = cars
            });
        }

        // GET: api/Cars/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> GetCar(long id)
        {
            var car = await _context.Cars.
                Include(c => c.Brand).
                Where(c => c.Id == id).
                Select(c => new
                Car
                {
                    Id = c.Id,
                    Name = c.Name,
                    Year = c.Year,
                    LastChangedAtUtc = c.LastChangedAtUtc,
                    Brand = c.Brand
                }).FirstOrDefaultAsync();

            if (car == null)
            {
                return NotFound();
            }

            return Ok(car);
        }

        // PUT: api/Cars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Car>> PutCar(long id, Car car)
        {
            if (!await BrandExists(car))
            {
                return BadRequest($"Brand with ID {car.BrandId} does not exist.");
            }

            if (id != car.Id)
            {
                return BadRequest();
            }

            _context.Entry(car).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return AcceptedAtAction("GetCar", new { id = car.Id }, car);
        }

        // POST: api/Cars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Car>> PostCar(Car car)
        {
            if (!await BrandExists(car))
            {
                return BadRequest($"Brand with ID {car.BrandId} does not exist.");
            }

            var carToSave = new Car
            {
                Name = car.Name,
                Year = car.Year,
                BrandId = car.BrandId,
                LastChangedAtUtc = DateTime.UtcNow
            };

            _context.Cars.Add(carToSave);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCar", new { id = car.Id }, car);
        }

        // DELETE: api/Cars/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(long id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarExists(long id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }

        private async Task<bool> BrandExists(Car car) {
            return await _context.Brands.AnyAsync(b => b.Id == car.BrandId);
        }
    }
}
