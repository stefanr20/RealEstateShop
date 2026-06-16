using Microsoft.Playwright;

namespace RealEstate.Tests.E2ETests.Pages
{
    public class AboutPage
    {
        private readonly IPage _page;
        private const string BaseUrl = "http://localhost:4200";

        public AboutPage(IPage page) => _page = page;

        public async Task GoTo() =>
            await _page.GotoAsync($"{BaseUrl}/about");

        public async Task WaitForLoad() =>
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        public async Task<string> GetTitle() =>
            await _page.TitleAsync();

        public async Task<string> GetContent() =>
            await _page.ContentAsync();
    }
}
