using Microsoft.Playwright;
using SnipeIt.Playwright.Assessment.Models;
using static Microsoft.Playwright.Assertions;

namespace SnipeIt.Playwright.Assessment.Pages;

public sealed class AssetDetailsPage
{
    private readonly ILocator _detailsContent;
    private readonly ILocator _historyTab;
    private readonly ILocator _historyContent;

    public AssetDetailsPage(IPage page)
    {
        _detailsContent = page.Locator("section.content");

        _historyTab = page.GetByRole(
            AriaRole.Link,
            new PageGetByRoleOptions
            {
                Name = "History"
            });

        _historyContent = page.Locator("#history");
    }

    public async Task AssertDetailsAsync(
        AssetData asset,
        string assignedUserName)
    {
        ArgumentNullException.ThrowIfNull(asset);
        ArgumentException.ThrowIfNullOrWhiteSpace(assignedUserName);

        await Expect(_detailsContent).ToBeVisibleAsync();

        await Expect(
            GetDetailText(asset.AssetTag),
            "Asset tag should match the newly created asset.")
            .ToBeVisibleAsync();

        await Expect(
            GetDetailText(asset.SerialNumber),
            "Serial number should match the created asset.")
            .ToBeVisibleAsync();

        await Expect(
            GetDetailText(asset.ModelName),
            "Model should be Macbook Pro 13.")
            .ToBeVisibleAsync();

       var statusSummary = _detailsContent
        .Locator("div.well")
        .Filter(new LocatorFilterOptions
        {
        HasText = asset.Status
        })
        .First;

        await Expect(
        statusSummary,
        "Status should be Ready to Deploy.")
        .ToContainTextAsync(asset.Status);

        await Expect(
            GetDetailText("Deployed"),
            "Asset should be checked out.")
            .ToBeVisibleAsync();

        await Expect(
            GetDetailText(assignedUserName),
            "Asset should be checked out to the selected user.")
            .ToBeVisibleAsync();

        await Expect(
            GetDetailText(asset.Notes),
            "Notes should match the created asset.")
            .ToBeVisibleAsync();
    }

    public async Task AssertHistoryAsync(
        AssetData asset,
        string assignedUserName)
    {
        await _historyTab.ClickAsync();

        await Expect(_historyContent).ToBeVisibleAsync();

        await Expect(_historyContent)
            .ToContainTextAsync("create new");

        await Expect(_historyContent)
            .ToContainTextAsync("checkout");

        await Expect(_historyContent)
            .ToContainTextAsync(asset.AssetTag);

        await Expect(_historyContent)
            .ToContainTextAsync(assignedUserName);
    }

    private ILocator GetDetailText(string text)
    {
        return _detailsContent.GetByText(
            text,
            new LocatorGetByTextOptions
            {
                Exact = true
            }).First;
    }
}