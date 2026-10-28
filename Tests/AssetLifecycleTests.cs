using SnipeIt.Playwright.Assessment.Configuration;
using SnipeIt.Playwright.Assessment.Models;
using SnipeIt.Playwright.Assessment.Pages;

namespace SnipeIt.Playwright.Assessment.Tests;

[TestFixture]
[NonParallelizable]
public sealed class AssetLifecycleTests : TestBase {
    [Test]
    [Description(
        "Creates a Macbook Pro 13 asset, checks it out, and validates its details and history.")]
    public async Task Create_Checkout_And_Validate_Asset_Lifecycle() {
        var asset = AssetData.Create();
        Log($"Starting test | Model: {asset.ModelName} | " + $"Asset Tag: {asset.AssetTag} | " +
            $"Serial Number: {asset.SerialNumber}");

        var loginPage = new LoginPage(Page);
        var assetsPage = new AssetsPage(Page);
        var checkoutPage = new CheckoutPage(Page);
        var assetDetailsPage = new AssetDetailsPage(Page);

        await loginPage.NavigateAsync();
        await loginPage.LoginAsync(TestSettings.Username, TestSettings.Password);
        Log("Login completed successfully.");

        await assetsPage.NavigateToCreateAsync();
        await assetsPage.CreateAssetAsync(asset);
        Log($"Asset created successfully | " + $"Asset Tag: {asset.AssetTag} | " +
            $"Serial Number: {asset.SerialNumber}");

        await assetsPage.SearchForAssetAsync(asset.AssetTag);
        Log($"Asset found in asset list | Asset Tag: {asset.AssetTag}");
        await assetsPage.OpenCheckoutAsync(asset.AssetTag);

        var assignedUserName = await checkoutPage.CheckoutToRandomUserAsync();
        Log($"Asset checked out successfully | " + $"Asset Tag: {asset.AssetTag} | " +
            $"Assigned User: {assignedUserName}");

        await assetsPage.NavigateToListAsync();
        await assetsPage.SearchForAssetAsync(asset.AssetTag);
        await assetsPage.OpenAssetDetailsAsync(asset.AssetTag);

        await assetDetailsPage.AssertDetailsAsync(asset, assignedUserName);
        Log($"Asset details validated successfully | Asset Tag: {asset.AssetTag}");
        await CaptureEvidenceScreenshotAsync($"asset-details-{asset.AssetTag}.png");

        await assetDetailsPage.AssertHistoryAsync(asset, assignedUserName);
        Log($"Asset history validated successfully | " + $"Asset Tag: {asset.AssetTag} | " +
            $"Assigned User: {assignedUserName}");

        await CaptureEvidenceScreenshotAsync($"asset-history-{asset.AssetTag}.png");

        Log($"Test completed successfully | Asset Tag: {asset.AssetTag}");
    }
}