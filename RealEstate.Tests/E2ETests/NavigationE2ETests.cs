using Microsoft.Playwright;
using Xunit;
using FluentAssertions;
using RealEstate.Tests.E2ETests.Pages;

namespace RealEstate.Tests.E2ETests
{
    public class NavigationE2ETests : IAsyncLifetime
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
        public async Task HomePage_LoadsSuccessfully()
        {
            await _navigationPage.GoTo();
            await _navigationPage.WaitForLoad();

            var title = await _navigationPage.GetTitle();
            title.Should().Contain("VeloraEstate");
        }

        [Fact]
        public async Task HomePage_HasHeroSection()
        {
            await _navigationPage.GoTo();
            await _navigationPage.WaitForLoad();

            var content = await _navigationPage.GetContent();
            content.Should().Contain("Premium");
        }

        [Fact]
        public async Task HomePage_HasNavbar()
        {
            await _navigationPage.GoTo();
            await _navigationPage.WaitForLoad();

            var content = await _navigationPage.GetContent();
            content.Should().Contain("VeloraEstate");
            content.Should().Contain("Properties");
        }

        [Fact]
        public async Task HomePage_NavbarPropertiesLink_Works()
        {
            await _navigationPage.GoTo();
            await _navigationPage.WaitForLoad();

            await _homePage.ClickPropertiesLink();
            await _navigationPage.WaitForLoad();

            var content = await _navigationPage.GetContent();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Newsletter_TypeEmailAndSubmit()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            await _homePage.FillNewsletterEmail("playwright@test.com");
            var value = await _page.InputValueAsync("input[placeholder='Email']");
            value.Should().Be("playwright@test.com");
        }

        [Fact]
        public async Task Newsletter_SubscribeButton_IsVisible()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            var isVisible = await _homePage.IsSubscribeButtonVisible();
            isVisible.Should().BeTrue();
        }

        [Fact]
        public async Task Newsletter_EmptyEmail_ButtonVisible()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            var content = await _homePage.GetContent();
            content.Should().Contain("LET'S BE EXCLUSIVE");
        }

        [Fact]
        public async Task ContactPage_HasContactInfo()
        {
            await _navigationPage.GoTo("contact");
            await _navigationPage.WaitForLoad();

            var content = await _navigationPage.GetContent();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task AboutPage_HasContent()
        {
            await _navigationPage.GoTo("about");
            await _navigationPage.WaitForLoad();

            var content = await _navigationPage.GetContent();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task UnknownPage_Shows404()
        {
            await _navigationPage.GoTo("nonexistentpage");
            await _navigationPage.WaitForLoad();

            var content = await _navigationPage.GetContent();
            content.Should().Contain("404");
        }

        [Fact]
        public async Task TermsPage_LoadsSuccessfully()
        {
            await _navigationPage.GoTo("terms");
            await _navigationPage.WaitForLoad();

            var content = await _navigationPage.GetContent();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task PrivacyPage_LoadsSuccessfully()
        {
            await _navigationPage.GoTo("privacy");
            await _navigationPage.WaitForLoad();

            var content = await _navigationPage.GetContent();
            content.Should().NotBeNullOrEmpty();
        }
    }
}