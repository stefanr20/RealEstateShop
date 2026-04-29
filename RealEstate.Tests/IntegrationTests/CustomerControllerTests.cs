using Xunit;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Data.Context;
using RealEstate.Domain.Entities;

namespace RealEstate.Tests.IntegrationTests
{
    public class CustomerControllerTests : IClassFixture<WebAppFactory>
    {
        private readonly HttpClient _client;
        private readonly WebAppFactory _factory;

        public CustomerControllerTests(WebAppFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            SeedData();
        }

        private void SeedData()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RealEstateDbContext>();
            if (context.Customers.Any()) return;

            context.Customers.AddRange(
                new Customer { FirstName = "Stefan", LastName = "Ristevski", Email = "stefan@test.com", Phone = "070000000" },
                new Customer { FirstName = "Ana", LastName = "Stoeva", Email = "ana@test.com", Phone = "071000000" }
            );
            context.SaveChanges();
        }

        private int GetFirstCustomerId()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RealEstateDbContext>();
            return context.Customers.First().Id;
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithCustomers()
        {
            // Act
            var response = await _client.GetAsync("/api/customer");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GetById_ValidId_ReturnsOk()
        {
            // Arrange
            var id = GetFirstCustomerId();

            // Act
            var response = await _client.GetAsync($"/api/customer/{id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetById_InvalidId_ReturnsNotFound()
        {
            // Act
            var response = await _client.GetAsync("/api/customer/9999");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Create_ValidCustomer_ReturnsOk()
        {
            // Arrange
            var customer = new
            {
                firstName = "Marko",
                lastName = "Popov",
                email = "marko@test.com",
                phone = "072000000"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/customer", customer);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Edit_ValidCustomer_ReturnsOk()
        {
            // Arrange
            var id = GetFirstCustomerId();
            var customer = new
            {
                id = id,
                firstName = "UpdatedName",
                lastName = "UpdatedLastName",
                email = "updated@test.com",
                phone = "075000000"
            };

            // Act
            var response = await _client.PutAsJsonAsync("/api/customer", customer);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_ValidId_ReturnsOk()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RealEstateDbContext>();
            var customer = new Customer { FirstName = "ToDelete", LastName = "User", Email = "delete@test.com", Phone = "079000000" };
            context.Customers.Add(customer);
            context.SaveChanges();

            // Act
            var response = await _client.DeleteAsync($"/api/customer/{customer.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}