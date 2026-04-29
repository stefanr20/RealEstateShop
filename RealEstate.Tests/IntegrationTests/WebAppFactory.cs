using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using RealEstate.Data.Context;
using RealEstate.Domain.Entities;

namespace RealEstate.Tests.IntegrationTests
{
    public class WebAppFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string>
                {
                    ["Jwt:Key"] = "ThisIsATestSecretKeyThatIsLongEnough123!",
                    ["Jwt:Issuer"] = "TestIssuer",
                    ["Jwt:Audience"] = "TestAudience",
                    ["ConnectionStrings:DefaultConnection"] = "DataSource=:memory:"
                });
            });

            builder.ConfigureServices(services =>
            {
                // Remove ALL services that contain DbContext or database provider references
                var toRemove = services.Where(d =>
                    d.ServiceType.FullName != null && (
                        d.ServiceType.FullName.Contains("RealEstateDbContext") ||
                        d.ServiceType.FullName.Contains("DbContextOptions") ||
                        d.ServiceType.FullName.Contains("IDbContextOptions") ||
                        d.ServiceType.FullName.Contains("SqlServer") ||
                        d.ServiceType.FullName.Contains("DbContext")
                    )
                ).ToList();

                foreach (var d in toRemove)
                    services.Remove(d);

                services.AddDbContext<RealEstateDbContext>(options =>
                    options.UseInMemoryDatabase("IntegrationTestDb"));
            });

            builder.UseEnvironment("Development");
        }

        public void SeedDatabase()
        {
            using var scope = Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RealEstateDbContext>();
            if (context.Properties.Any()) return;

            context.Properties.AddRange(
                new Property
                {
                    Title = "Luxury Apartment",
                    Description = "Nice apartment",
                    Price = "€150,000",
                    Photo = "photo.jpg",
                    Type = "Apartment",
                    HeatingType = "Central",
                    Bedrooms = 2,
                    Bathrooms = 1,
                    Area = 80,
                    Address = new Address { City = "Skopje", Street = "Str 1", Country = "MK" }
                },
                new Property
                {
                    Title = "Modern Villa",
                    Description = "Big villa",
                    Price = "€320,000",
                    Photo = "photo.jpg",
                    Type = "House",
                    HeatingType = "Central",
                    Bedrooms = 4,
                    Bathrooms = 2,
                    Area = 200,
                    Address = new Address { City = "Ohrid", Street = "Str 2", Country = "MK" }
                }
            );
            context.SaveChanges();
        }

        public int GetFirstPropertyId()
        {
            using var scope = Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RealEstateDbContext>();
            return context.Properties.First().Id;
        }
    }
}