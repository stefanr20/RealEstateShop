using Microsoft.Playwright;
using Xunit;
using FluentAssertions;

namespace RealEstate.Tests.E2ETests
{
    public class NavigationE2ETests : IAsyncLifetime
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

        // HOME PAGE
        [Fact]
        public async Task HomePage_LoadsSuccessfully()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var title = await _page.TitleAsync();
            title.Should().Contain("VeloraEstate");
        }

        [Fact]
        public async Task HomePage_HasHeroSection()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Premium");
        }

        [Fact]
        public async Task HomePage_HasNavbar()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("VeloraEstate");
            content.Should().Contain("Properties");
        }

        [Fact]
        public async Task HomePage_NavbarPropertiesLink_Works()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await _page.ClickAsync("a:has-text('Properties')");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().NotBeNullOrEmpty();
        }

        // SUBSCRIBER
        [Fact]
        public async Task Newsletter_TypeEmailAndSubmit()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await _page.FillAsync("input[placeholder='Email']", "playwright@test.com");
            var value = await _page.InputValueAsync("input[placeholder='Email']");
            value.Should().Be("playwright@test.com");
        }

        [Fact]
        public async Task Newsletter_SubscribeButton_IsVisible()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var button = _page.Locator("button:has-text(\"LET'S BE EXCLUSIVE\")");
            var isVisible = await button.IsVisibleAsync();
            isVisible.Should().BeTrue();
        }

        [Fact]
        public async Task Newsletter_EmptyEmail_ButtonVisible()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("LET'S BE EXCLUSIVE");
        }

        // CONTACT PAGE
        [Fact]
        public async Task ContactPage_HasContactInfo()
        {
            await _page.GotoAsync($"{BaseUrl}/contact");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().NotBeNullOrEmpty();
        }

        // ABOUT PAGE
        [Fact]
        public async Task AboutPage_HasContent()
        {
            await _page.GotoAsync($"{BaseUrl}/about");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().NotBeNullOrEmpty();
        }

        // 404 PAGE
        [Fact]
        public async Task UnknownPage_Shows404()
        {
            await _page.GotoAsync($"{BaseUrl}/nonexistentpage");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("404");
        }

        // TERMS AND PRIVACY
        [Fact]
        public async Task TermsPage_LoadsSuccessfully()
        {
            await _page.GotoAsync($"{BaseUrl}/terms");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task PrivacyPage_LoadsSuccessfully()
        {
            await _page.GotoAsync($"{BaseUrl}/privacy");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().NotBeNullOrEmpty();
        }
    }
}
