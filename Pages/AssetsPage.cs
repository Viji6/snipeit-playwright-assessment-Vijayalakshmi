//using System;
using System.Text.RegularExpressions;
using Microsoft.Playwright;
using SnipeIt.Playwright.Assessment.Configuration;
using SnipeIt.Playwright.Assessment.Models;

namespace SnipeIt.Playwright.Assessment.Pages;

public sealed class AssetsPage
{
    private readonly IPage _page;
    private readonly ILocator _assetTagInput;
    private readonly ILocator _serialNumberInput;
    private readonly ILocator _modelDropdown;
    private readonly ILocator _statusDropdown;
    private readonly ILocator _notesInput;
    private readonly ILocator _saveButton;
    private readonly ILocator _assetsTable;
    private readonly ILocator _searchInput;

    public AssetsPage(IPage page)
    {
        _page = page;
        _assetTagInput = page.Locator("#asset_tag");
        _serialNumberInput = page.Locator("#serial_1");
        _modelDropdown = page.Locator("#select2-model_select_id-container");
        _statusDropdown = page.Locator("#select2-status_select_id-container");
        _notesInput = page.Locator("#notes");
        _saveButton = page.Locator("#submit_button");
        _assetsTable = page.Locator("#assetsListingTable");
        _searchInput = page.Locator("input.search-input");
    }

    private ILocator OpenSelect2SearchInput =>
        _page.Locator(".select2-container--open input.select2-search__field");

    public async Task NavigateToCreateAsync()
    {
        await _page.GotoAsync(
            $"{TestSettings.BaseUrl}/hardware/create",
            new PageGotoOptions
            {
                WaitUntil = WaitUntilState.DOMContentLoaded
            });

        await _assetTagInput.WaitForAsync();
    }

    public async Task NavigateToListAsync()
{
    await _page.GotoAsync(
        $"{TestSettings.BaseUrl}/hardware",
        new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        });

    await _assetsTable.WaitForAsync(
        new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible
        });
}

    public async Task CreateAssetAsync(AssetData asset)
    {
        ArgumentNullException.ThrowIfNull(asset);

        await _assetTagInput.FillAsync(asset.AssetTag);
        await _serialNumberInput.FillAsync(asset.SerialNumber);

        await SelectOptionAsync(_modelDropdown, asset.ModelName);
        await SelectOptionAsync(_statusDropdown, asset.Status);

        await _notesInput.FillAsync(asset.Notes);

        await Task.WhenAll(
            _page.WaitForURLAsync(
                new Regex(@".*/hardware/?(?:\?.*)?$")),
            _saveButton.ClickAsync());

        await _assetsTable.WaitForAsync();
    }

    public async Task SearchForAssetAsync(string assetTag)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(assetTag);

        await _searchInput.FillAsync(assetTag);
        await _searchInput.PressAsync("Enter");

        await GetAssetRow(assetTag).WaitForAsync(
            new LocatorWaitForOptions
            {
                State = WaitForSelectorState.Visible
            });
    }

    public async Task OpenCheckoutAsync(string assetTag)
    {
        var checkoutLink = GetAssetRow(assetTag).GetByRole(
            AriaRole.Link,
            new LocatorGetByRoleOptions
            {
                Name = "Checkout",
                Exact = true
            });

        await checkoutLink.ClickAsync();

        await _page.Locator(
            "#select2-assigned_user_select-container").WaitForAsync();
    }

    public async Task OpenAssetDetailsAsync(string assetTag)
    {
        var assetLink = GetAssetRow(assetTag).GetByRole(
            AriaRole.Link,
            new LocatorGetByRoleOptions
            {
                Name = assetTag,
                Exact = true
            });

        await assetLink.ClickAsync();

        await _page
    .Locator("#details")
    .GetByText(
        "Asset Tag",
        new LocatorGetByTextOptions
        {
            Exact = true
        })
    .WaitForAsync(
        new LocatorWaitForOptions
        {
            State = WaitForSelectorState.Visible
        });
    }

    private ILocator GetAssetRow(string assetTag)
    {
        return _assetsTable
            .Locator("tbody tr")
            .Filter(new LocatorFilterOptions
            {
                HasText = assetTag
            })
            .First;
    }

    private async Task SelectOptionAsync(
        ILocator dropdown,
        string optionText)
    {
        await dropdown.ClickAsync();
        await OpenSelect2SearchInput.FillAsync(optionText);

        var matchingOption = _page
            .Locator(".select2-results__option")
            .Filter(new LocatorFilterOptions
            {
                HasText = optionText
            })
            .First;

        await matchingOption.ClickAsync();
    }
}