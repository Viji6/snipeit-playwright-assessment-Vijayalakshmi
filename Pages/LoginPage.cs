using Microsoft.Playwright;
using SnipeIt.Playwright.Assessment.Configuration;

namespace SnipeIt.Playwright.Assessment.Pages;

public sealed class LoginPage
{
    private readonly IPage _page;
    private readonly ILocator _usernameInput;
    private readonly ILocator _passwordInput;
    private readonly ILocator _loginButton;
    private readonly ILocator _assetLookupInput;

    public LoginPage(IPage page)
    {
        _page = page;
        _usernameInput = page.Locator("#username");
        _passwordInput = page.Locator("#password-field");
        _loginButton = page.GetByRole(
            AriaRole.Button,
            new PageGetByRoleOptions { Name = "Login" });
       _assetLookupInput = page.GetByPlaceholder("Lookup by Asset Tag");;
    }

    public async Task NavigateAsync()
    {
        await _page.GotoAsync(
            $"{TestSettings.BaseUrl}/login",
            new PageGotoOptions
            {
                WaitUntil = WaitUntilState.DOMContentLoaded
            });

        await _usernameInput.WaitForAsync();
    }

    public async Task LoginAsync(string username, string password)
    {
        await _usernameInput.FillAsync(username);
        await _passwordInput.FillAsync(password);
        await _loginButton.ClickAsync();

        await _assetLookupInput.WaitForAsync(
            new LocatorWaitForOptions { State = WaitForSelectorState.Visible });;
    }
}