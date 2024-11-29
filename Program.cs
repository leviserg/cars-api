using cars_api.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(
        configuration.GetConnectionString("SqlServer"), 
        dboptions =>
        {
            dboptions.EnableRetryOnFailure(3, TimeSpan.FromSeconds(3), null);
        }
    );
});


builder.Services.AddScoped<DbSeed>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        dbContext.Database.EnsureCreated();

        var seeder = scope.ServiceProvider.GetRequiredService<DbSeed>();
        if (!dbContext.Brands.Any())
        {
            seeder.SeedBrands();
        }

        if (!dbContext.Cars.Any())
        {
            seeder.SeedCars();
        }
    }

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
