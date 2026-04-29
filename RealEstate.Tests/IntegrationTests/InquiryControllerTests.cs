using Xunit;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.Data.Context;
using RealEstate.Domain.Entities;

namespace RealEstate.Tests.IntegrationTests
{
    public class InquiryControllerTests : IClassFixture<WebAppFactory>
    {
        private readonly HttpClient _client;
        private readonly WebAppFactory _factory;

        public InquiryControllerTests(WebAppFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            SeedData();
        }

        private void SeedData()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RealEstateDbContext>();
            if (context.Inquiries.Any()) return;

            context.Inquiries.AddRange(
                new Inquiry
                {
                    Name = "Stefan",
                    Email = "stefan@test.com",
                    Phone = "070000000",
                    Message = "Interested in property"
                },
                new Inquiry
                {
                    Name = "Ana",
                    Email = "ana@test.com",
                    Phone = "071000000",
                    Message = "Question about price"
                }
            );
            context.SaveChanges();
        }

        private async Task<string> GetAdminToken()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RealEstateDbContext>();
            if (!context.Users.Any(u => u.Username == "admin"))
            {
                context.Users.Add(new User
                {
                    Username = "admin",
                    Email = "admin@test.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = "admin"
                });
                context.SaveChanges();
            }

            var loginDto = new { username = "admin", password = "admin123" };
            var response = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            var content = await response.Content.ReadAsStringAsync();
            return System.Text.Json.JsonDocument.Parse(content)
                .RootElement.GetProperty("token").GetString();
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithInquiries()
        {
            // Act
            var response = await _client.GetAsync("/api/inquiry");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Stefan");
        }

        [Fact]
        public async Task Create_ValidInquiry_ReturnsOk()
        {
            // Arrange
            var inquiry = new
            {
                name = "Marko",
                email = "marko@test.com",
                phone = "072000000",
                message = "I want to buy this property"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/inquiry", inquiry);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetMyInquiries_WithoutToken_ReturnsUnauthorized()
        {
            // Act
            var response = await _client.GetAsync("/api/inquiry/my");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetMyInquiries_WithValidToken_ReturnsOk()
        {
            // Arrange
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RealEstateDbContext>();
            if (!context.Users.Any(u => u.Username == "inquiryuser"))
            {
                context.Users.Add(new User
                {
                    Username = "inquiryuser",
                    Email = "stefan@test.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("pass123"),
                    Role = "user"
                });
                context.SaveChanges();
            }

            var loginDto = new { username = "inquiryuser", password = "pass123" };
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            var loginContent = await loginResponse.Content.ReadAsStringAsync();
            var token = System.Text.Json.JsonDocument.Parse(loginContent)
                .RootElement.GetProperty("token").GetString();

            var request = new HttpRequestMessage(HttpMethod.Get, "/api/inquiry/my");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Reply_WithoutToken_ReturnsUnauthorized()
        {
            // Arrange
            var dto = new { reply = "Thank you for your inquiry!" };

            // Act
            var response = await _client.PutAsJsonAsync("/api/inquiry/1/reply", dto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Reply_WithAdminToken_ReturnsOk()
        {
            // Arrange
            var token = await GetAdminToken();
            var dto = new { reply = "Thank you for your inquiry!" };

            var request = new HttpRequestMessage(HttpMethod.Put, "/api/inquiry/1/reply");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            request.Content = JsonContent.Create(dto);

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
