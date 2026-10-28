using System;
using System.Text.RegularExpressions;
using Microsoft.Playwright;

namespace SnipeIt.Playwright.Assessment.Pages;

public sealed class CheckoutPage
{
    private static readonly Regex UserOptionPattern =
        new(@".+\(.+\)\s*-\s*#\d+");

    private readonly IPage _page;
    private readonly ILocator _userDropdown;
    private readonly ILocator _checkoutButton;

    public CheckoutPage(IPage page)
    {
        _page = page;
        _userDropdown = page.Locator(
            "#select2-assigned_user_select-container");
        _checkoutButton = page.Locator("#submit_button");
    }

    private ILocator OpenSelect2SearchInput =>
        _page.Locator(".select2-container--open input.select2-search__field");

    public async Task<string> CheckoutToRandomUserAsync()
    {
        await _userDropdown.ClickAsync();
        await OpenSelect2SearchInput.FillAsync("a");

        var userOptions = _page
            .Locator(".select2-results__option")
            .Filter(new LocatorFilterOptions
            {
                HasTextRegex = UserOptionPattern
            });

        await userOptions.First.WaitForAsync();

        var optionCount = await userOptions.CountAsync();
        var selectedOption = userOptions.Nth(
            Random.Shared.Next(optionCount));

        var optionText = (await selectedOption.InnerTextAsync()).Trim();

        await selectedOption.ClickAsync();

        await Task.WhenAll(
            _page.WaitForURLAsync(
                new Regex(@".*/hardware(?:\?.*|/\d+(?:\?.*)?)?$")),
            _checkoutButton.ClickAsync());

        return GetUserDisplayName(optionText);
    }

    private static string GetUserDisplayName(string optionText)
    {
        var usernameStartIndex = optionText.IndexOf(
            " (",
            StringComparison.Ordinal);

        return usernameStartIndex > 0
            ? optionText[..usernameStartIndex].Trim()
            : optionText;
    }
}