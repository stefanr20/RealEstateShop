using Microsoft.Playwright;

namespace RealEstate.Tests.E2ETests.Pages
{
    public class LoginPage
    {
        private readonly IPage _page;
        private const string BaseUrl = "http://localhost:4200";

        public LoginPage(IPage page) => _page = page;

        public async Task GoTo() =>
            await _page.GotoAsync($"{BaseUrl}/login");

        public async Task FillUsername(string username) =>
            await _page.FillAsync("input[formcontrolname='username']", username);

        public async Task FillPassword(string password) =>
            await _page.FillAsync("input[formcontrolname='password']", password);

        public async Task ClickSignIn() =>
            await _page.ClickAsync("button.auth-btn[type='submit']");

        public async Task WaitForLoad() =>
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        public async Task<bool> IsButtonDisabled() =>
            await _page.Locator("button.auth-btn[type='submit']").IsDisabledAsync();

        public async Task<string> GetUrl() =>
            _page.Url;

        public async Task<string> GetContent() =>
            await _page.ContentAsync();

        public async Task ClickRegisterTab() =>
            await _page.ClickAsync("button:has-text('Register')");

        public async Task Login(string username, string password)
        {
            await GoTo();
            await WaitForLoad();
            await FillUsername(username);
            await FillPassword(password);
            await _page.WaitForSelectorAsync("button.auth-btn:not([disabled])");
            await ClickSignIn();
            await _page.WaitForTimeoutAsync(3000);
        }
    }
}
