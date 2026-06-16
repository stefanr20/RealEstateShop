using Microsoft.Playwright;

namespace RealEstate.Tests.E2ETests.Pages
{
    public class ContactPage
    {
        private readonly IPage _page;
        private const string BaseUrl = "http://localhost:4200";

        public ContactPage(IPage page) => _page = page;

        public async Task GoTo() =>
            await _page.GotoAsync($"{BaseUrl}/contact");

        public async Task WaitForLoad() =>
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        public async Task<string> GetTitle() =>
            await _page.TitleAsync();

        public async Task<string> GetContent() =>
            await _page.ContentAsync();

        public async Task FillFullName(string name) =>
            await _page.FillAsync("input[placeholder='John Doe']", name);

        public async Task FillEmail(string email) =>
            await _page.FillAsync("input[placeholder='john@example.com']", email);

        public async Task FillSubject(string subject) =>
            await _page.FillAsync("input[placeholder='Property inquiry...']", subject);

        public async Task<bool> IsSendMessageButtonVisible() =>
            await _page.Locator("button:has-text('Send Message')").IsVisibleAsync();

        public async Task<string> GetInputValue(string placeholder) =>
            await _page.InputValueAsync($"input[placeholder='{placeholder}']");
    }
}
