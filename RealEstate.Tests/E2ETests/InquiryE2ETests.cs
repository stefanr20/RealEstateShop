using Microsoft.Playwright;
using Xunit;
using FluentAssertions;
using RealEstate.Tests.E2ETests.Pages;

namespace RealEstate.Tests.E2ETests
{
    public class InquiryE2ETests : IAsyncLifetime
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;
        private HomePage _homePage;
        private AboutPage _aboutPage;
        private ContactPage _contactPage;

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
            _homePage = new HomePage(_page);
            _aboutPage = new AboutPage(_page);
            _contactPage = new ContactPage(_page);
        }

        public async Task DisposeAsync()
        {
            await _browser.DisposeAsync();
            _playwright.Dispose();
        }

        [Fact]
        public async Task HomePage_HasNewsletterSection()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            var content = await _homePage.GetContent();
            content.Should().Contain("BECOME A MEMBER");
        }

        [Fact]
        public async Task HomePage_NewsletterHasEmailInput()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            var content = await _homePage.GetContent();
            content.Should().Contain("Email");
        }

        [Fact]
        public async Task HomePage_NewsletterHasSubscribeButton()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            var isVisible = await _homePage.IsSubscribeButtonVisible();
            isVisible.Should().BeTrue();
        }

        [Fact]
        public async Task ContactPage_LoadsSuccessfully()
        {
            await _contactPage.GoTo();
            await _contactPage.WaitForLoad();

            var content = await _contactPage.GetContent();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task AboutPage_LoadsSuccessfully()
        {
            await _aboutPage.GoTo();
            await _aboutPage.WaitForLoad();

            var content = await _aboutPage.GetContent();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Footer_HasNavigationLinks()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            var content = await _homePage.GetContent();
            content.Should().Contain("Properties");
            content.Should().Contain("About Us");
            content.Should().Contain("Contact");
        }

        [Fact]
        public async Task Footer_HasLegalLinks()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            var content = await _homePage.GetContent();
            content.Should().Contain("Terms");
            content.Should().Contain("Privacy");
        }

        [Fact]
        public async Task Footer_HasContactInfo()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            var content = await _homePage.GetContent();
            content.Should().Contain("Skopje");
        }
    }
}