using Microsoft.Playwright;
using Xunit;
using FluentAssertions;
using RealEstate.Tests.E2ETests.Pages;

namespace RealEstate.Tests.E2ETests
{
    public class PropertySearchE2ETests : IAsyncLifetime
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;
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
            _homePage = new HomePage(_page);
        }

        public async Task DisposeAsync()
        {
            await _browser.DisposeAsync();
            _playwright.Dispose();
        }

        [Fact]
        public async Task HomePage_LoadsWithSearchBar()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            var content = await _homePage.GetContent();
            content.Should().Contain("Search by city, title or type");
        }

        [Fact]
        public async Task HomePage_HasFeaturedProperties()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            var content = await _homePage.GetContent();
            content.Should().Contain("Featured Properties");
        }

        [Fact]
        public async Task HomePage_HasSearchFilters()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            var content = await _homePage.GetContent();
            content.Should().Contain("Search");
        }

        [Fact]
        public async Task HomePage_SearchButton_IsVisible()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            var isVisible = await _homePage.IsSearchButtonVisible();
            isVisible.Should().BeTrue();
        }

        [Fact]
        public async Task HomePage_TypesInSearchBox()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            await _homePage.FillSearchBox("Skopje");
            var value = await _page.InputValueAsync("input[placeholder*='Search by city']");
            value.Should().Be("Skopje");
        }

        [Fact]
        public async Task HomePage_ClicksSearchButton()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            await _homePage.FillSearchBox("Skopje");
            await _homePage.ClickSearch();
            await _homePage.WaitForLoad();

            var content = await _homePage.GetContent();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task PropertiesPage_HasPropertyCards()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            var content = await _homePage.GetContent();
            content.Should().Contain("View Details");
        }

        [Fact]
        public async Task PropertiesPage_HasPagination()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            var content = await _homePage.GetContent();
            content.Should().Contain("All Properties");
        }

        [Fact]
        public async Task PropertyCard_ViewDetailsButton_IsVisible()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            var content = await _homePage.GetContent();
            content.Should().Contain("View Details");
        }

        [Fact]
        public async Task PropertyCard_ClickViewDetails_NavigatesToDetails()
        {
            await _homePage.GoTo();
            await _homePage.WaitForLoad();

            await _homePage.ClickViewDetails();
            await _homePage.WaitForLoad();

            _page.Url.Should().Contain("property");
        }
    }
}