using Microsoft.Playwright;
using Microsoft.Playwright.NUnit;
using NUnit.Framework.Interfaces;

namespace SnipeIt.Playwright.Assessment.Tests;

public abstract class TestBase : PageTest
{
    public override BrowserNewContextOptions ContextOptions()
    {
        return new BrowserNewContextOptions
        {
            ViewportSize = new ViewportSize
            {
                Width = 1440,
                Height = 900
            }
        };
    }

    [SetUp]
    public void ConfigureTimeouts()
    {
        Page.SetDefaultTimeout(15_000);
        Assertions.SetDefaultExpectTimeout(15_000);
    }

    [TearDown]
    public async Task CaptureFailureScreenshotAsync()
    {
        if (TestContext.CurrentContext.Result.Outcome.Status != TestStatus.Failed)
        {
            return;
        }

        try
        {
            var outputDirectory = Path.Combine(
                TestContext.CurrentContext.WorkDirectory,
                "test-results");

            Directory.CreateDirectory(outputDirectory);

            var screenshotPath = Path.Combine(
                outputDirectory,
                "failure.png");

            await Page.ScreenshotAsync(new PageScreenshotOptions
            {
                Path = screenshotPath,
                FullPage = true
            });

            TestContext.Progress.WriteLine(
                $"Failure screenshot saved to: {screenshotPath}");
        }
        catch (Exception exception)
        {
            TestContext.Progress.WriteLine(
                $"Could not capture failure screenshot: {exception.Message}");
        }
    }
        protected static void Log(string message)
    {
        TestContext.Progress.WriteLine($"[SnipeIT] {message}");
    }

    protected async Task CaptureEvidenceScreenshotAsync(string fileName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(fileName);

        var outputDirectory = Path.Combine(
            TestContext.CurrentContext.WorkDirectory,
            "test-results");

        Directory.CreateDirectory(outputDirectory);

        var screenshotPath = Path.Combine(outputDirectory, fileName);

        await Page.ScreenshotAsync(new PageScreenshotOptions
        {
            Path = screenshotPath,
            FullPage = true
        });

        Log($"Evidence screenshot saved: {screenshotPath}");
    }
    
}