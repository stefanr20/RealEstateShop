using Microsoft.Playwright;
using Xunit;
using FluentAssertions;

namespace RealEstate.Tests.E2ETests
{
    public class AboutE2ETests : IAsyncLifetime
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;
        private const string AboutUrl = "http://localhost:4200/about";

        public async Task InitializeAsync()
        {
            _playwright = await Playwright.CreateAsync();
            _browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false,
                SlowMo = 300
            });
            _page = await _browser.NewPageAsync(new BrowserNewPageOptions
            {
                IgnoreHTTPSErrors = true
            });
        }

        public async Task DisposeAsync()
        {
            await _browser.DisposeAsync();
            _playwright.Dispose();
        }

        [Fact]
        public async Task AboutPage_LoadsSuccessfully()
        {
            await _page.GotoAsync(AboutUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var title = await _page.TitleAsync();
            title.Should().Contain("About");
        }

        [Fact]
        public async Task AboutPage_HasHeroSection()
        {
            await _page.GotoAsync(AboutUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Macedonia");
        }

        [Fact]
        public async Task AboutPage_HasOurStorySection()
        {
            await _page.GotoAsync(AboutUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Our Story");
        }

        [Fact]
        public async Task AboutPage_HasTeamSection()
        {
            await _page.GotoAsync(AboutUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Our Team");
        }

        [Fact]
        public async Task AboutPage_HasTeamMembers()
        {
            await _page.GotoAsync(AboutUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Stefan Ristevski");
        }

        [Fact]
        public async Task AboutPage_HasStatistics()
        {
            await _page.GotoAsync(AboutUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Properties Listed");
        }

        [Fact]
        public async Task AboutPage_HasHappyClients()
        {
            await _page.GotoAsync(AboutUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Happy Clients");
        }

        [Fact]
        public async Task AboutPage_HasCitiesCovered()
        {
            await _page.GotoAsync(AboutUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Cities Covered");
        }

        [Fact]
        public async Task AboutPage_HasAverageRating()
        {
            await _page.GotoAsync(AboutUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Average Rating");
        }

        [Fact]
        public async Task AboutPage_HasFoundedYear()
        {
            await _page.GotoAsync(AboutUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("2020");
        }
    }
}
