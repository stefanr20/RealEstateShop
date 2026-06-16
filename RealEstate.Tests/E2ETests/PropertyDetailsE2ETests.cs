using Microsoft.Playwright;
using Xunit;
using FluentAssertions;
using RealEstate.Tests.E2ETests.Pages;

namespace RealEstate.Tests.E2ETests
{
    public class PropertyDetailsE2ETests : IAsyncLifetime
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;
        private PropertyDetailsPage _propertyDetailsPage;

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
            _propertyDetailsPage = new PropertyDetailsPage(_page);
        }

        public async Task DisposeAsync()
        {
            await _browser.DisposeAsync();
            _playwright.Dispose();
        }

        [Fact]
        public async Task PropertyDetails_LoadsSuccessfully()
        {
            await _propertyDetailsPage.GoTo(3);
            await _propertyDetailsPage.WaitForLoad();

            var title = await _propertyDetailsPage.GetTitle();
            title.Should().Contain("Property");
        }

        [Fact]
        public async Task PropertyDetails_ShowsPrice()
        {
            await _propertyDetailsPage.GoTo(3);
            await _page.WaitForTimeoutAsync(4000);

            var content = await _propertyDetailsPage.GetContent();
            content.Should().Contain("€");
        }

        [Fact]
        public async Task PropertyDetails_ShowsPropertyType()
        {
            await _propertyDetailsPage.GoTo(3);
            await _page.WaitForTimeoutAsync(4000);

            var content = await _propertyDetailsPage.GetContent();
            content.Should().Contain("Property Details");
        }

        [Fact]
        public async Task PropertyDetails_ShowsBedroomsAndBathrooms()
        {
            await _propertyDetailsPage.GoTo(3);
            await _propertyDetailsPage.WaitForLoad();

            var content = await _propertyDetailsPage.GetContent();
            content.Should().Contain("Bedrooms");
            content.Should().Contain("Bathrooms");
        }

        [Fact]
        public async Task PropertyDetails_ShowsAddress()
        {
            await _propertyDetailsPage.GoTo(3);
            await _propertyDetailsPage.WaitForLoad();

            var content = await _propertyDetailsPage.GetContent();
            content.Should().Contain("Skopje");
        }

        [Fact]
        public async Task PropertyDetails_ShowsAmenities()
        {
            await _propertyDetailsPage.GoTo(3);
            await _page.WaitForTimeoutAsync(4000);

            var content = await _propertyDetailsPage.GetContent();
            content.Should().Contain("Back to listings");
        }

        [Fact]
        public async Task PropertyDetails_ShowsAboutSection()
        {
            await _propertyDetailsPage.GoTo(3);
            await _propertyDetailsPage.WaitForLoad();

            var content = await _propertyDetailsPage.GetContent();
            content.Should().Contain("About This Property");
        }

        [Fact]
        public async Task PropertyDetails_ShowsLocationMap()
        {
            await _propertyDetailsPage.GoTo(3);
            await _propertyDetailsPage.WaitForLoad();

            var content = await _propertyDetailsPage.GetContent();
            content.Should().Contain("Location");
        }

        [Fact]
        public async Task PropertyDetails_HasInquiryForm()
        {
            await _propertyDetailsPage.GoTo(3);
            await _propertyDetailsPage.WaitForLoad();

            var content = await _propertyDetailsPage.GetContent();
            content.Should().Contain("Interested in this property?");
        }

        [Fact]
        public async Task PropertyDetails_InquiryForm_HasRequiredFields()
        {
            await _propertyDetailsPage.GoTo(3);
            await _page.WaitForTimeoutAsync(4000);

            var content = await _propertyDetailsPage.GetContent();
            content.Should().Contain("Send Inquiry");
        }

        [Fact]
        public async Task PropertyDetails_InquiryForm_HasSendButton()
        {
            await _propertyDetailsPage.GoTo(3);
            await _propertyDetailsPage.WaitForLoad();

            var isVisible = await _propertyDetailsPage.IsSendInquiryButtonVisible();
            isVisible.Should().BeTrue();
        }

        [Fact]
        public async Task PropertyDetails_HasBackToListingsButton()
        {
            await _propertyDetailsPage.GoTo(3);
            await _propertyDetailsPage.WaitForLoad();

            var content = await _propertyDetailsPage.GetContent();
            content.Should().Contain("Back to listings");
        }

        [Fact]
        public async Task PropertyDetails_BackToListings_Navigates()
        {
            await _propertyDetailsPage.GoTo(3);
            await _propertyDetailsPage.WaitForLoad();

            await _propertyDetailsPage.ClickBackToListings();
            await _propertyDetailsPage.WaitForLoad();

            var url = await _propertyDetailsPage.GetUrl();
            url.Should().NotContain("property/3");
        }

        [Fact]
        public async Task PropertyDetails_HasSaveButton()
        {
            await _propertyDetailsPage.GoTo(3);
            await _propertyDetailsPage.WaitForLoad();

            var content = await _propertyDetailsPage.GetContent();
            content.Should().Contain("Save");
        }

        [Fact]
        public async Task PropertyDetails_HasShareButton()
        {
            await _propertyDetailsPage.GoTo(3);
            await _propertyDetailsPage.WaitForLoad();

            var content = await _propertyDetailsPage.GetContent();
            content.Should().Contain("Share");
        }
    }
}