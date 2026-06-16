using Microsoft.Playwright;

namespace RealEstate.Tests.E2ETests.Pages
{
    public class PropertyDetailsPage
    {
        private readonly IPage _page;
        private const string BaseUrl = "http://localhost:4200";

        public PropertyDetailsPage(IPage page) => _page = page;

        public async Task GoTo(int propertyId) =>
            await _page.GotoAsync($"{BaseUrl}/property/{propertyId}");

        public async Task WaitForLoad() =>
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        public async Task<string> GetTitle() =>
            await _page.TitleAsync();

        public async Task<string> GetContent() =>
            await _page.ContentAsync();

        public async Task<bool> IsSendInquiryButtonVisible() =>
            await _page.Locator("button:has-text('Send Inquiry')").IsVisibleAsync();

        public async Task ClickBackToListings() =>
            await _page.ClickAsync("text=Back to listings");

        public async Task<string> GetUrl() =>
            _page.Url;
    }
}
