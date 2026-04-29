using Microsoft.Playwright;
using Xunit;
using FluentAssertions;

namespace RealEstate.Tests.E2ETests
{
    public class InquiryE2ETests : IAsyncLifetime
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;
        private const string BaseUrl = "http://localhost:4200";

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
        public async Task HomePage_HasNewsletterSection()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("BECOME A MEMBER");
        }

        [Fact]
        public async Task HomePage_NewsletterHasEmailInput()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Email");
        }

        [Fact]
        public async Task HomePage_NewsletterHasSubscribeButton()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("LET'S BE EXCLUSIVE");
        }

        [Fact]
        public async Task ContactPage_LoadsSuccessfully()
        {
            await _page.GotoAsync($"{BaseUrl}/contact");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task AboutPage_LoadsSuccessfully()
        {
            await _page.GotoAsync($"{BaseUrl}/about");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Footer_HasNavigationLinks()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Properties");
            content.Should().Contain("About Us");
            content.Should().Contain("Contact");
        }

        [Fact]
        public async Task Footer_HasLegalLinks()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Terms");
            content.Should().Contain("Privacy");
        }

        [Fact]
        public async Task Footer_HasContactInfo()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Skopje");
        }
    }
}