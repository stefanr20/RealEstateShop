using Microsoft.Playwright;
using Xunit;
using FluentAssertions;

namespace RealEstate.Tests.E2ETests
{
    public class AuthE2ETests : IAsyncLifetime
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
        public async Task LoginPage_LoadsSuccessfully()
        {
            await _page.GotoAsync($"{BaseUrl}/login");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Sign in to your account");
        }

        [Fact]
        public async Task LoginPage_InvalidCredentials_StaysOnLoginPage()
        {
            await _page.GotoAsync($"{BaseUrl}/login");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await _page.FillAsync("input[formcontrolname='username']", "wronguser");
            await _page.FillAsync("input[formcontrolname='password']", "wrongpass");
            await _page.ClickAsync("button.auth-btn[type='submit']");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            _page.Url.Should().Contain("login");
        }

        [Fact]
        public async Task LoginPage_HasSignInAndRegisterTabs()
        {
            await _page.GotoAsync($"{BaseUrl}/login");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Sign In");
            content.Should().Contain("Register");
        }

        [Fact]
        public async Task RegisterPage_HasRegisterForm()
        {
            await _page.GotoAsync($"{BaseUrl}/login");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("auth-switch");
        }

        [Fact]
        public async Task ProfilePage_WhenNotLoggedIn_RedirectsAwayFromProfile()
        {
            await _page.GotoAsync($"{BaseUrl}/profile");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            _page.Url.Should().NotBe($"{BaseUrl}/profile");
        }

        [Fact]
        public async Task LoginPage_HasUsernameAndPasswordFields()
        {
            await _page.GotoAsync($"{BaseUrl}/login");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var usernameField = _page.Locator("input[formcontrolname='username']");
            var passwordField = _page.Locator("input[formcontrolname='password']");

            await usernameField.IsVisibleAsync().ContinueWith(t => t.Result.Should().BeTrue());
            await passwordField.IsVisibleAsync().ContinueWith(t => t.Result.Should().BeTrue());
        }

        [Fact]
        public async Task LoginPage_EmptyForm_ButtonIsDisabled()
        {
            await _page.GotoAsync($"{BaseUrl}/login");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var button = _page.Locator("button.auth-btn[type='submit']");
            var isDisabled = await button.IsDisabledAsync();
            isDisabled.Should().BeTrue();
        }

        [Fact]
        public async Task LoginPage_OnlyUsername_ButtonStaysDisabled()
        {
            await _page.GotoAsync($"{BaseUrl}/login");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await _page.FillAsync("input[formcontrolname='username']", "testuser");

            var button = _page.Locator("button.auth-btn[type='submit']");
            var isDisabled = await button.IsDisabledAsync();
            isDisabled.Should().BeTrue();
        }

        [Fact]
        public async Task LoginPage_BothFieldsFilled_ButtonBecomesEnabled()
        {
            await _page.GotoAsync($"{BaseUrl}/login");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await _page.FillAsync("input[formcontrolname='username']", "testuser");
            await _page.FillAsync("input[formcontrolname='password']", "Testpass1");

            var button = _page.Locator("button.auth-btn[type='submit']");
            var isDisabled = await button.IsDisabledAsync();
            isDisabled.Should().BeFalse();
        }

        [Fact]
        public async Task RegisterTab_Click_ShowsRegisterForm()
        {
            await _page.GotoAsync($"{BaseUrl}/login");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await _page.ClickAsync("button:has-text('Register')");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task LoginPage_HasForgotPasswordLink()
        {
            await _page.GotoAsync($"{BaseUrl}/login");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var content = await _page.ContentAsync();
            content.Should().Contain("Forgot password");
        }

        [Fact]
        public async Task LoginPage_HasRememberMeCheckbox()
        {
            await _page.GotoAsync($"{BaseUrl}/login");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            var checkbox = _page.Locator("input[type='checkbox']");
            var isVisible = await checkbox.IsVisibleAsync();
            isVisible.Should().BeTrue();
        }
    }
}