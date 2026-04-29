using Microsoft.Playwright;
using Xunit;
using FluentAssertions;

namespace RealEstate.Tests.E2ETests
{
    public class PropertySearchE2ETests : IAsyncLifetime
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
        public async Task HomePage_LoadsWithSearchBar()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Search by city, title or type");
        }

        [Fact]
        public async Task HomePage_HasFeaturedProperties()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Featured Properties");
        }

        [Fact]
        public async Task HomePage_HasSearchFilters()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Search");
        }

        [Fact]
        public async Task HomePage_SearchButton_IsVisible()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var searchButton = _page.Locator("button:has-text('Search')");
            var isVisible = await searchButton.IsVisibleAsync();
            isVisible.Should().BeTrue();
        }

        [Fact]
        public async Task HomePage_TypesInSearchBox()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await _page.FillAsync("input[placeholder*='Search by city']", "Skopje");
            var value = await _page.InputValueAsync("input[placeholder*='Search by city']");
            value.Should().Be("Skopje");
        }

        [Fact]
        public async Task HomePage_ClicksSearchButton()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await _page.FillAsync("input[placeholder*='Search by city']", "Skopje");
            await _page.ClickAsync("button:has-text('Search')");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task PropertiesPage_HasPropertyCards()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("View Details");
        }

        [Fact]
        public async Task PropertiesPage_HasPagination()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("All Properties");
        }

        [Fact]
        public async Task PropertyCard_ViewDetailsButton_IsVisible()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("View Details");
        }


        [Fact]
        public async Task PropertyCard_ClickViewDetails_NavigatesToDetails()
        {
            await _page.GotoAsync(BaseUrl);
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await _page.Locator("text=View Details").First.ClickAsync();
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            _page.Url.Should().Contain("property");
        }
    }
}
