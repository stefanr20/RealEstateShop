using Microsoft.Playwright;

namespace RealEstate.Tests.E2ETests.Pages
{
    public class HomePage
    {
        private readonly IPage _page;
        private const string BaseUrl = "http://localhost:4200";

        public HomePage(IPage page) => _page = page;

        public async Task GoTo() =>
            await _page.GotoAsync(BaseUrl);

        public async Task WaitForLoad() =>
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        public async Task<string> GetTitle() =>
            await _page.TitleAsync();

        public async Task<string> GetContent() =>
            await _page.ContentAsync();

        public async Task<bool> IsSearchButtonVisible() =>
            await _page.Locator("button:has-text('Search')").IsVisibleAsync();

        public async Task<bool> IsSubscribeButtonVisible() =>
            await _page.Locator("button:has-text(\"LET'S BE EXCLUSIVE\")").IsVisibleAsync();

        public async Task FillSearchBox(string query) =>
            await _page.FillAsync("input[placeholder*='Search by city']", query);

        public async Task ClickSearch() =>
            await _page.ClickAsync("button:has-text('Search')");

        public async Task FillNewsletterEmail(string email) =>
            await _page.FillAsync("input[placeholder='Email']", email);

        public async Task ClickViewDetails() =>
            await _page.Locator("text=View Details").First.ClickAsync();

        public async Task ClickPropertiesLink() =>
            await _page.ClickAsync("a:has-text('Properties')");

        public async Task OpenBurgerMenu() =>
            await _page.ClickAsync("button.burger-btn");

        public async Task CloseBurgerMenu() =>
            await _page.ClickAsync("button.menu-close");
    }
}
