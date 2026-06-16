using Microsoft.Playwright;
using Xunit;
using FluentAssertions;
using RealEstate.Tests.E2ETests.Pages;

namespace RealEstate.Tests.E2ETests
{
    public class ContactE2ETests : IAsyncLifetime
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;
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
            _contactPage = new ContactPage(_page);
        }

        public async Task DisposeAsync()
        {
            await _browser.DisposeAsync();
            _playwright.Dispose();
        }

        [Fact]
        public async Task ContactPage_LoadsSuccessfully()
        {
            await _contactPage.GoTo();
            await _contactPage.WaitForLoad();

            var title = await _contactPage.GetTitle();
            title.Should().Contain("Contact");
        }

        [Fact]
        public async Task ContactPage_HasSendMessageForm()
        {
            await _contactPage.GoTo();
            await _contactPage.WaitForLoad();

            var content = await _contactPage.GetContent();
            content.Should().Contain("Send Us a Message");
        }

        [Fact]
        public async Task ContactPage_HasFullNameField()
        {
            await _contactPage.GoTo();
            await _contactPage.WaitForLoad();

            var content = await _contactPage.GetContent();
            content.Should().Contain("John Doe");
        }

        [Fact]
        public async Task ContactPage_HasEmailField()
        {
            await _contactPage.GoTo();
            await _contactPage.WaitForLoad();

            var content = await _contactPage.GetContent();
            content.Should().Contain("john@example.com");
        }

        [Fact]
        public async Task ContactPage_HasPhoneField()
        {
            await _contactPage.GoTo();
            await _contactPage.WaitForLoad();

            var content = await _contactPage.GetContent();
            content.Should().Contain("+389 70 000 000");
        }

        [Fact]
        public async Task ContactPage_HasSubjectField()
        {
            await _contactPage.GoTo();
            await _contactPage.WaitForLoad();

            var content = await _contactPage.GetContent();
            content.Should().Contain("Property inquiry");
        }

        [Fact]
        public async Task ContactPage_HasMessageField()
        {
            await _contactPage.GoTo();
            await _contactPage.WaitForLoad();

            var content = await _contactPage.GetContent();
            content.Should().Contain("Tell us how we can help");
        }

        [Fact]
        public async Task ContactPage_HasSendButton()
        {
            await _contactPage.GoTo();
            await _contactPage.WaitForLoad();

            var isVisible = await _contactPage.IsSendMessageButtonVisible();
            isVisible.Should().BeTrue();
        }

        [Fact]
        public async Task ContactPage_HasOfficeAddress()
        {
            await _contactPage.GoTo();
            await _contactPage.WaitForLoad();

            var content = await _contactPage.GetContent();
            content.Should().Contain("Our Office");
        }

        [Fact]
        public async Task ContactPage_HasPhoneInfo()
        {
            await _contactPage.GoTo();
            await _contactPage.WaitForLoad();

            var content = await _contactPage.GetContent();
            content.Should().Contain("Phone");
        }

        [Fact]
        public async Task ContactPage_HasEmailInfo()
        {
            await _contactPage.GoTo();
            await _contactPage.WaitForLoad();

            var content = await _contactPage.GetContent();
            content.Should().Contain("veloraestate.com");
        }

        [Fact]
        public async Task ContactPage_FormFillsCorrectly()
        {
            await _contactPage.GoTo();
            await _contactPage.WaitForLoad();

            await _contactPage.FillFullName("Test User");
            await _contactPage.FillEmail("test@test.com");
            await _contactPage.FillSubject("Test Subject");

            var nameValue = await _contactPage.GetInputValue("John Doe");
            var emailValue = await _contactPage.GetInputValue("john@example.com");

            nameValue.Should().Be("Test User");
            emailValue.Should().Be("test@test.com");
        }
    }
}