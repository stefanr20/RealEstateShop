using Microsoft.Playwright;
using Xunit;
using FluentAssertions;
using RealEstate.Tests.E2ETests.Pages;

namespace RealEstate.Tests.E2ETests
{
    public class AboutE2ETests : IAsyncLifetime
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;
        private AboutPage _aboutPage;

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
            _aboutPage = new AboutPage(_page);
        }

        public async Task DisposeAsync()
        {
            await _browser.DisposeAsync();
            _playwright.Dispose();
        }

        [Fact]
        public async Task AboutPage_LoadsSuccessfully()
        {
            await _aboutPage.GoTo();
            await _aboutPage.WaitForLoad();

            var title = await _aboutPage.GetTitle();
            title.Should().Contain("About");
        }

        [Fact]
        public async Task AboutPage_HasHeroSection()
        {
            await _aboutPage.GoTo();
            await _aboutPage.WaitForLoad();

            var content = await _aboutPage.GetContent();
            content.Should().Contain("Premium");
        }

        [Fact]
        public async Task AboutPage_HasOurStorySection()
        {
            await _aboutPage.GoTo();
            await _aboutPage.WaitForLoad();

            var content = await _aboutPage.GetContent();
            content.Should().Contain("Our Story");
        }

        [Fact]
        public async Task AboutPage_HasTeamSection()
        {
            await _aboutPage.GoTo();
            await _aboutPage.WaitForLoad();

            var content = await _aboutPage.GetContent();
            content.Should().Contain("Our Team");
        }

        [Fact]
        public async Task AboutPage_HasTeamMembers()
        {
            await _aboutPage.GoTo();
            await _aboutPage.WaitForLoad();

            var content = await _aboutPage.GetContent();
            content.Should().Contain("Stefan Ristevski");
        }

        [Fact]
        public async Task AboutPage_HasStatistics()
        {
            await _aboutPage.GoTo();
            await _aboutPage.WaitForLoad();

            var content = await _aboutPage.GetContent();
            content.Should().Contain("Properties Listed");
        }

        [Fact]
        public async Task AboutPage_HasHappyClients()
        {
            await _aboutPage.GoTo();
            await _aboutPage.WaitForLoad();

            var content = await _aboutPage.GetContent();
            content.Should().Contain("Happy Clients");
        }

        [Fact]
        public async Task AboutPage_HasCitiesCovered()
        {
            await _aboutPage.GoTo();
            await _aboutPage.WaitForLoad();

            var content = await _aboutPage.GetContent();
            content.Should().Contain("Cities Covered");
        }

        [Fact]
        public async Task AboutPage_HasAverageRating()
        {
            await _aboutPage.GoTo();
            await _aboutPage.WaitForLoad();

            var content = await _aboutPage.GetContent();
            content.Should().Contain("Average Rating");
        }

        [Fact]
        public async Task AboutPage_HasFoundedYear()
        {
            await _aboutPage.GoTo();
            await _aboutPage.WaitForLoad();

            var content = await _aboutPage.GetContent();
            content.Should().Contain("2020");
        }
    }
}