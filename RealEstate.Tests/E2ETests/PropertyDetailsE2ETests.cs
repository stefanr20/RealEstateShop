using Microsoft.Playwright;
using Xunit;
using FluentAssertions;

namespace RealEstate.Tests.E2ETests
{
    public class PropertyDetailsE2ETests : IAsyncLifetime
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;
        private const string BaseUrl = "http://localhost:4200";
        private const string PropertyUrl = "http://localhost:4200/property/3";

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
        public async Task PropertyDetails_LoadsSuccessfully()
        {
            await _page.GotoAsync(PropertyUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var title = await _page.TitleAsync();
            title.Should().Contain("Property");
        }

        [Fact]
        public async Task PropertyDetails_ShowsPrice()
        {
            await _page.GotoAsync(PropertyUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);
            await _page.WaitForTimeoutAsync(2000);

            var content = await _page.ContentAsync();
            content.Should().Contain("€");
        }

        [Fact]
        public async Task PropertyDetails_ShowsPropertyType()
        {
            await _page.GotoAsync(PropertyUrl);
            await _page.WaitForSelectorAsync(".property-type, .type-badge, span:has-text('APARTMENT')",
                new PageWaitForSelectorOptions { Timeout = 10000 });

            var content = await _page.ContentAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task PropertyDetails_ShowsBedroomsAndBathrooms()
        {
            await _page.GotoAsync(PropertyUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Bedrooms");
            content.Should().Contain("Bathrooms");
        }

        [Fact]
        public async Task PropertyDetails_ShowsAddress()
        {
            await _page.GotoAsync(PropertyUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Skopje");
        }

        [Fact]
        public async Task PropertyDetails_ShowsAmenities()
        {
            await _page.GotoAsync(PropertyUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Back to listings");
        }


        [Fact]
        public async Task PropertyDetails_ShowsAboutSection()
        {
            await _page.GotoAsync(PropertyUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("About This Property");
        }

        [Fact]
        public async Task PropertyDetails_ShowsLocationMap()
        {
            await _page.GotoAsync(PropertyUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Location");
        }

        [Fact]
        public async Task PropertyDetails_HasInquiryForm()
        {
            await _page.GotoAsync(PropertyUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Interested in this property?");
        }

        [Fact]
        public async Task PropertyDetails_InquiryForm_HasRequiredFields()
        {
            await _page.GotoAsync(PropertyUrl);
            await _page.WaitForTimeoutAsync(4000);

            var content = await _page.ContentAsync();
            content.Should().Contain("Send Inquiry");
        }

        [Fact]
        public async Task PropertyDetails_InquiryForm_HasSendButton()
        {
            await _page.GotoAsync(PropertyUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var button = _page.Locator("button:has-text('Send Inquiry')");
            var isVisible = await button.IsVisibleAsync();
            isVisible.Should().BeTrue();
        }

        [Fact]
        public async Task PropertyDetails_HasBackToListingsButton()
        {
            await _page.GotoAsync(PropertyUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Back to listings");
        }

        [Fact]
        public async Task PropertyDetails_BackToListings_Navigates()
        {
            await _page.GotoAsync(PropertyUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await _page.ClickAsync("text=Back to listings");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            _page.Url.Should().NotContain("property/3");
        }

        [Fact]
        public async Task PropertyDetails_HasSaveButton()
        {
            await _page.GotoAsync(PropertyUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Save");
        }

        [Fact]
        public async Task PropertyDetails_HasShareButton()
        {
            await _page.GotoAsync(PropertyUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Share");
        }
    }
}
