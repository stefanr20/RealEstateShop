using Xunit;
using FluentAssertions;
using System.Net;
using System.Net.Http.Json;

namespace RealEstate.Tests.IntegrationTests
{
    public class SubscriberControllerTests : IClassFixture<WebAppFactory>
    {
        private readonly HttpClient _client;
        private readonly WebAppFactory _factory;

        public SubscriberControllerTests(WebAppFactory factory)
        {
            _factory = factory;
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Subscribe_ValidEmail_ReturnsOk()
        {
            // Arrange
            var dto = new { email = "newsubscriber@test.com" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/subscriber", dto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Subscribed successfully");
        }

        [Fact]
        public async Task Subscribe_DuplicateEmail_ReturnsBadRequest()
        {
            // Arrange
            var dto = new { email = "duplicate@test.com" };
            await _client.PostAsJsonAsync("/api/subscriber", dto);

            // Act - subscribe again with same email
            var response = await _client.PostAsJsonAsync("/api/subscriber", dto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Already subscribed");
        }

        [Fact]
        public async Task Subscribe_EmptyEmail_ReturnsBadRequest()
        {
            // Arrange
            var dto = new { email = "" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/subscriber", dto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Email is required");
        }

        [Fact]
        public async Task Subscribe_MultipleUniqueEmails_AllReturnOk()
        {
            // Arrange
            var emails = new[] { "user1@test.com", "user2@test.com", "user3@test.com" };

            // Act & Assert
            foreach (var email in emails)
            {
                var response = await _client.PostAsJsonAsync("/api/subscriber", new { email });
                response.StatusCode.Should().Be(HttpStatusCode.OK);
            }
        }
    }
}