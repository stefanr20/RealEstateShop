using Microsoft.Playwright;
using Xunit;
using FluentAssertions;
using RealEstate.Tests.E2ETests.Pages;

namespace RealEstate.Tests.E2ETests
{
    public class AuthE2ETests : IAsyncLifetime
    {
        private IPlaywright _playwright;
        private IBrowser _browser;
        private IPage _page;
        private LoginPage _loginPage;

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
            _loginPage = new LoginPage(_page);
        }

        public async Task DisposeAsync()
        {
            await _browser.DisposeAsync();
            _playwright.Dispose();
        }

        [Fact]
        public async Task LoginPage_LoadsSuccessfully()
        {
            await _loginPage.GoTo();
            await _loginPage.WaitForLoad();

            var content = await _loginPage.GetContent();
            content.Should().Contain("Sign in to your account");
        }

        [Fact]
        public async Task LoginPage_InvalidCredentials_StaysOnLoginPage()
        {
            await _loginPage.GoTo();
            await _loginPage.WaitForLoad();

            await _loginPage.FillUsername("wronguser");
            await _loginPage.FillPassword("wrongpass");
            await _loginPage.ClickSignIn();
            await _loginPage.WaitForLoad();

            var url = await _loginPage.GetUrl();
            url.Should().Contain("login");
        }

        [Fact]
        public async Task LoginPage_EmptyForm_ButtonIsDisabled()
        {
            await _loginPage.GoTo();
            await _loginPage.WaitForLoad();

            var isDisabled = await _loginPage.IsButtonDisabled();
            isDisabled.Should().BeTrue();
        }

        [Fact]
        public async Task LoginPage_OnlyUsername_ButtonStaysDisabled()
        {
            await _loginPage.GoTo();
            await _loginPage.WaitForLoad();

            await _loginPage.FillUsername("testuser");

            var isDisabled = await _loginPage.IsButtonDisabled();
            isDisabled.Should().BeTrue();
        }

        [Fact]
        public async Task LoginPage_BothFieldsFilled_ButtonBecomesEnabled()
        {
            await _loginPage.GoTo();
            await _loginPage.WaitForLoad();

            await _loginPage.FillUsername("testuser");
            await _loginPage.FillPassword("Testpass1");

            var isDisabled = await _loginPage.IsButtonDisabled();
            isDisabled.Should().BeFalse();
        }

        [Fact]
        public async Task LoginPage_HasSignInAndRegisterTabs()
        {
            await _loginPage.GoTo();
            await _loginPage.WaitForLoad();

            var content = await _loginPage.GetContent();
            content.Should().Contain("Sign In");
            content.Should().Contain("Register");
        }

        [Fact]
        public async Task LoginPage_HasUsernameAndPasswordFields()
        {
            await _loginPage.GoTo();
            await _loginPage.WaitForLoad();

            var content = await _loginPage.GetContent();
            content.Should().Contain("your username");
        }

        [Fact]
        public async Task LoginPage_HasForgotPasswordLink()
        {
            await _loginPage.GoTo();
            await _loginPage.WaitForLoad();

            var content = await _loginPage.GetContent();
            content.Should().Contain("Forgot password");
        }

        [Fact]
        public async Task LoginPage_HasRememberMeCheckbox()
        {
            await _loginPage.GoTo();
            await _loginPage.WaitForLoad();

            var checkbox = _page.Locator("input[type='checkbox']");
            var isVisible = await checkbox.IsVisibleAsync();
            isVisible.Should().BeTrue();
        }

        [Fact]
        public async Task RegisterPage_HasRegisterForm()
        {
            await _loginPage.GoTo();
            await _loginPage.WaitForLoad();

            var content = await _loginPage.GetContent();
            content.Should().Contain("auth-switch");
        }

        [Fact]
        public async Task RegisterTab_Click_ShowsRegisterForm()
        {
            await _loginPage.GoTo();
            await _loginPage.WaitForLoad();

            await _loginPage.ClickRegisterTab();
            await _loginPage.WaitForLoad();

            var content = await _loginPage.GetContent();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task ProfilePage_WhenNotLoggedIn_RedirectsAwayFromProfile()
        {
            await _page.GotoAsync("http://localhost:4200/profile");
            await _page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            _page.Url.Should().NotBe("http://localhost:4200/profile");
        }
    }
}