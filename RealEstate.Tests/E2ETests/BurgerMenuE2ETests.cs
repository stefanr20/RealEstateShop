using Microsoft.Playwright;
using Xunit;
using FluentAssertions;
using RealEstate.Tests.E2ETests.Pages;

namespace RealEstate.Tests.E2ETests
{
    public class BurgerMenuE2ETests : IAsyncLifetime
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;
        private NavigationPage _navigationPage;
        private HomePage _homePage;

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
            _navigationPage = new NavigationPage(_page);
            _homePage = new HomePage(_page);
        }

        public async Task DisposeAsync()
        {
            await _browser.DisposeAsync();
            _playwright.Dispose();
        }

        [Fact]
        public async Task BurgerMenu_Opens_WhenClicked()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            await _homePage.OpenBurgerMenu();
            await _navigationPage.WaitForTimeout(500);

            var isOpen = await _navigationPage.IsBurgerMenuOpen();
            isOpen.Should().BeTrue();
        }

        [Fact]
        public async Task BurgerMenu_HasPropertiesLink()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            await _homePage.OpenBurgerMenu();
            await _navigationPage.WaitForTimeout(500);

            var content = await _navigationPage.GetContent();
            content.Should().Contain("Properties");
        }

        [Fact]
        public async Task BurgerMenu_HasAboutUsLink()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            await _homePage.OpenBurgerMenu();
            await _navigationPage.WaitForTimeout(500);

            var content = await _navigationPage.GetContent();
            content.Should().Contain("About Us");
        }

        [Fact]
        public async Task BurgerMenu_HasContactLink()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            await _homePage.OpenBurgerMenu();
            await _navigationPage.WaitForTimeout(500);

            var content = await _navigationPage.GetContent();
            content.Should().Contain("Contact");
        }

        [Fact]
        public async Task BurgerMenu_HasMyProfileLink()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            await _homePage.OpenBurgerMenu();
            await _navigationPage.WaitForTimeout(500);

            var content = await _navigationPage.GetContent();
            content.Should().Contain("VeloraEstate");
        }

        [Fact]
        public async Task BurgerMenu_HasLanguageSwitcher()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            await _homePage.OpenBurgerMenu();
            await _navigationPage.WaitForTimeout(500);

            var content = await _navigationPage.GetContent();
            content.Should().Contain("EN");
            content.Should().Contain("MK");
        }

        [Fact]
        public async Task BurgerMenu_HasSocialLinks()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            await _homePage.OpenBurgerMenu();
            await _navigationPage.WaitForTimeout(500);

            var isVisible = await _navigationPage.IsSocialLinksVisible();
            isVisible.Should().BeTrue();
        }

        [Fact]
        public async Task BurgerMenu_Closes_WhenXClicked()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            await _homePage.OpenBurgerMenu();
            await _navigationPage.WaitForTimeout(500);
            await _homePage.CloseBurgerMenu();
            await _navigationPage.WaitForTimeout(500);

            var menuPanel = _page.Locator(".menu-panel.open");
            var count = await menuPanel.CountAsync();
            count.Should().Be(0);
        }
    }
}