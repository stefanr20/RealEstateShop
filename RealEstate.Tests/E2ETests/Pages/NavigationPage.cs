using Microsoft.Playwright;

namespace RealEstate.Tests.E2ETests.Pages
{
    public class NavigationPage
    {
        private readonly IPage _page;
        private const string BaseUrl = "http://localhost:4200";

        public NavigationPage(IPage page) => _page = page;

        public async Task GoTo(string path = "") =>
            await _page.GotoAsync($"{BaseUrl}/{path}");

        public async Task WaitForLoad() =>
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        public async Task<string> GetUrl() =>
            _page.Url;

        public async Task<string> GetContent() =>
            await _page.ContentAsync();

        public async Task<string> GetTitle() =>
            await _page.TitleAsync();

        public async Task OpenBurgerMenu() =>
            await _page.ClickAsync("button.burger-btn");

        public async Task CloseBurgerMenu() =>
            await _page.ClickAsync("button.menu-close");

        public async Task<bool> IsBurgerMenuOpen() =>
            await _page.Locator(".menu-panel").IsVisibleAsync();

        public async Task<bool> IsSocialLinksVisible() =>
            await _page.Locator(".menu-social").IsVisibleAsync();

        public async Task WaitForTimeout(int ms) =>
            await _page.WaitForTimeoutAsync(ms);
    }
}
