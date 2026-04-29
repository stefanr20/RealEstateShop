using Microsoft.Playwright;
using Xunit;
using FluentAssertions;

namespace RealEstate.Tests.E2ETests
{
    public class ContactE2ETests : IAsyncLifetime
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;
        private const string BaseUrl = "http://localhost:4200";
        private const string ContactUrl = "http://localhost:4200/contact";

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
        public async Task ContactPage_LoadsSuccessfully()
        {
            await _page.GotoAsync(ContactUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var title = await _page.TitleAsync();
            title.Should().Contain("Contact");
        }

        [Fact]
        public async Task ContactPage_HasSendMessageForm()
        {
            await _page.GotoAsync(ContactUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Send Us a Message");
        }

        [Fact]
        public async Task ContactPage_HasFullNameField()
        {
            await _page.GotoAsync(ContactUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("John Doe");
        }

        [Fact]
        public async Task ContactPage_HasEmailField()
        {
            await _page.GotoAsync(ContactUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("john@example.com");
        }

        [Fact]
        public async Task ContactPage_HasPhoneField()
        {
            await _page.GotoAsync(ContactUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("+389 70 000 000");
        }

        [Fact]
        public async Task ContactPage_HasSubjectField()
        {
            await _page.GotoAsync(ContactUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Property inquiry");
        }

        [Fact]
        public async Task ContactPage_HasMessageField()
        {
            await _page.GotoAsync(ContactUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Tell us how we can help");
        }

        [Fact]
        public async Task ContactPage_HasSendButton()
        {
            await _page.GotoAsync(ContactUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var button = _page.Locator("button:has-text('Send Message')");
            var isVisible = await button.IsVisibleAsync();
            isVisible.Should().BeTrue();
        }

        [Fact]
        public async Task ContactPage_HasOfficeAddress()
        {
            await _page.GotoAsync(ContactUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Our Office");
        }

        [Fact]
        public async Task ContactPage_HasPhoneInfo()
        {
            await _page.GotoAsync(ContactUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Phone");
        }

        [Fact]
        public async Task ContactPage_HasEmailInfo()
        {
            await _page.GotoAsync(ContactUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("veloraestate.com");
        }

        [Fact]
        public async Task ContactPage_FormFillsCorrectly()
        {
            await _page.GotoAsync(ContactUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await _page.FillAsync("input[placeholder='John Doe']", "Test User");
            await _page.FillAsync("input[placeholder='john@example.com']", "test@test.com");
            await _page.FillAsync("input[placeholder='Property inquiry...']", "Test Subject");

            var nameValue = await _page.InputValueAsync("input[placeholder='John Doe']");
            var emailValue = await _page.InputValueAsync("input[placeholder='john@example.com']");

            nameValue.Should().Be("Test User");
            emailValue.Should().Be("test@test.com");
        }
    }
}
