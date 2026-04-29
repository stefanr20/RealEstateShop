using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Data.Context;
using RealEstate.Domain.Entities;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace RealEstate.Tests.IntegrationTests
{
    public class PropertyControllerTests : IClassFixture<WebAppFactory>
    {
        private readonly HttpClient _client;
        private readonly WebAppFactory _factory;

        public PropertyControllerTests(WebAppFactory factory)
        {
            _factory = factory;
            _factory.SeedDatabase();
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetProperties_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetAsync("/api/property");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetPropertyDetails_ValidId_ReturnsSuccess()
        {
            var id = _factory.GetFirstPropertyId();
            var response = await _client.GetAsync($"/api/property/{id}");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetPropertyDetails_InvalidId_ReturnsNotFound()
        {
            var response = await _client.GetAsync("/api/property/9999");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetProperties_ReturnsJsonContentType()
        {
            var response = await _client.GetAsync("/api/property");
            response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
        }

        [Fact]
        public async Task GetPropertySearch_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetAsync("/api/property/search?query=skopje");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetPropertyPaged_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetAsync("/api/property/paged?page=1&pageSize=9");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetPropertyPaged_InvalidPage_ReturnsBadRequest()
        {
            var response = await _client.GetAsync("/api/property/paged?page=0&pageSize=9");
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetHome_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetAsync("/");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateProperty_ValidData_ReturnsCreated()
        {
            // Arrange
            var dto = new
            {
                title = "New Test Property",
                description = "A great property",
                price = "€200,000",
                photo = "photo.jpg",
                bedrooms = 3,
                bathrooms = 2,
                area = 120,
                type = "Apartment",
                heatingType = "Central",
                isFeatured = false,
                hasGarage = false,
                hasElevator = true,
                hasBalcony = true,
                hasPool = false,
                hasInternet = true,
                isFurnished = false,
                hasAirConditioning = true,
                hasSecurity = false,
                address = new { city = "Skopje", street = "Test St", country = "MK" }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/property", dto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
        }

        [Fact]
        public async Task DeleteProperty_ValidId_ReturnsOk()
        {
            // Arrange - seed a property to delete
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RealEstateDbContext>();
            var property = new Property
            {
                Title = "To Delete",
                Description = "Delete me",
                Price = "€50,000",
                Photo = "photo.jpg",
                Type = "Apartment",
                HeatingType = "Central",
                Bedrooms = 1,
                Bathrooms = 1,
                Area = 50,
                Address = new Address { City = "Skopje", Street = "Str 1", Country = "MK" }
            };
            context.Properties.Add(property);
            context.SaveChanges();

            // Act
            var response = await _client.DeleteAsync($"/api/property/{property.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteProperty_InvalidId_ReturnsNotFound()
        {
            // Act
            var response = await _client.DeleteAsync("/api/property/9999");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task EditProperty_ValidData_ReturnsOk()
        {
            // Arrange
            var id = _factory.GetFirstPropertyId();
            var dto = new
            {
                id = id,
                title = "Updated Property",
                description = "Updated description",
                price = "€180,000",
                photo = "photo.jpg",
                bedrooms = 2,
                bathrooms = 1,
                area = 90,
                type = "Apartment",
                heatingType = "Central",
                isFeatured = true,
                hasGarage = false,
                hasElevator = false,
                hasBalcony = true,
                hasPool = false,
                hasInternet = true,
                isFurnished = true,
                hasAirConditioning = false,
                hasSecurity = false,
                address = new { city = "Skopje", street = "Updated St", country = "MK" }
            };

            // Act
            var response = await _client.PutAsJsonAsync("/api/property", dto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}