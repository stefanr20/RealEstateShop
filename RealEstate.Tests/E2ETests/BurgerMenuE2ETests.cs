using Microsoft.Playwright;
using Xunit;
using FluentAssertions;

namespace RealEstate.Tests.E2ETests
{
    public class BurgerMenuE2ETests : IAsyncLifetime
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
        public async Task BurgerMenu_Opens_WhenClicked()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await _page.ClickAsync("button.burger-btn");
            await _page.WaitForTimeoutAsync(500);

            var menuPanel = _page.Locator(".menu-panel");
            var isVisible = await menuPanel.IsVisibleAsync();
            isVisible.Should().BeTrue();
        }

        [Fact]
        public async Task BurgerMenu_HasPropertiesLink()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await _page.ClickAsync("button.burger-btn");
            await _page.WaitForTimeoutAsync(500);

            var content = await _page.ContentAsync();
            content.Should().Contain("Properties");
        }

        [Fact]
        public async Task BurgerMenu_HasAboutUsLink()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await _page.ClickAsync("button.burger-btn");
            await _page.WaitForTimeoutAsync(500);

            var content = await _page.ContentAsync();
            content.Should().Contain("About Us");
        }

        [Fact]
        public async Task BurgerMenu_HasContactLink()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await _page.ClickAsync("button.burger-btn");
            await _page.WaitForTimeoutAsync(500);

            var content = await _page.ContentAsync();
            content.Should().Contain("Contact");
        }

        [Fact]
        public async Task BurgerMenu_HasMyProfileLink()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await _page.ClickAsync("button.burger-btn");
            await _page.WaitForTimeoutAsync(500);

            var content = await _page.ContentAsync();
            content.Should().Contain("VeloraEstate");
        }

        [Fact]
        public async Task BurgerMenu_HasLanguageSwitcher()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await _page.ClickAsync("button.burger-btn");
            await _page.WaitForTimeoutAsync(500);

            var content = await _page.ContentAsync();
            content.Should().Contain("EN");
            content.Should().Contain("MK");
        }

        [Fact]
        public async Task BurgerMenu_HasSocialLinks()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await _page.ClickAsync("button.burger-btn");
            await _page.WaitForTimeoutAsync(500);

            var socialLinks = _page.Locator(".menu-social");
            var isVisible = await socialLinks.IsVisibleAsync();
            isVisible.Should().BeTrue();
        }

        [Fact]
        public async Task BurgerMenu_Closes_WhenXClicked()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await _page.ClickAsync("button.burger-btn");
            await _page.WaitForTimeoutAsync(500);
            await _page.ClickAsync("button.menu-close");
            await _page.WaitForTimeoutAsync(500);

            var menuPanel = _page.Locator(".menu-panel.open");
            var count = await menuPanel.CountAsync();
            count.Should().Be(0);
        }
    }
}
