using Xunit;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using RealEstate.Data.Context;
using RealEstate.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace RealEstate.Tests.IntegrationTests
{
    public class AuthControllerTests : IClassFixture<WebAppFactory>
    {
        private readonly HttpClient _client;
        private readonly WebAppFactory _factory;

        public AuthControllerTests(WebAppFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
            SeedUser();
        }

        private void SeedUser()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RealEstateDbContext>();
            if (context.Users.Any()) return;

            context.Users.Add(new User
            {
                Username = "testuser",
                Email = "testuser@test.com",
                Password = BCrypt.Net.BCrypt.HashPassword("password123"),
                Role = "user"
            });
            context.SaveChanges();
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkWithToken()
        {
            // Arrange
            var dto = new { username = "testuser", password = "password123" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", dto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("token");
        }

        [Fact]
        public async Task Login_InvalidPassword_ReturnsUnauthorized()
        {
            // Arrange
            var dto = new { username = "testuser", password = "wrongpassword" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", dto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Login_NonExistentUser_ReturnsUnauthorized()
        {
            // Arrange
            var dto = new { username = "nobody", password = "password123" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/login", dto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Register_NewUser_ReturnsOk()
        {
            // Arrange
            var dto = new { username = "newuser", email = "newuser@test.com", password = "pass123" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/register", dto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Account created successfully");
        }

        [Fact]
        public async Task Register_ExistingUsername_ReturnsBadRequest()
        {
            // Arrange
            var dto = new { username = "testuser", email = "other@test.com", password = "pass123" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/auth/register", dto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Username already exists");
        }

        [Fact]
        public async Task GetUsers_ReturnsOkWithUsers()
        {
            // Act
            var response = await _client.GetAsync("/api/auth/users");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("testuser");
        }

        [Fact]
        public async Task GetProfile_WithoutToken_ReturnsUnauthorized()
        {
            // Act
            var response = await _client.GetAsync("/api/auth/profile");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetProfile_WithValidToken_ReturnsOk()
        {
            // Arrange - login first to get token
            var loginDto = new { username = "testuser", password = "password123" };
            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", loginDto);
            var loginContent = await loginResponse.Content.ReadAsStringAsync();
            var token = System.Text.Json.JsonDocument.Parse(loginContent)
                .RootElement.GetProperty("token").GetString();

            // Act - use token to access profile
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/auth/profile");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("testuser");
        }
    }
}