namespace SnipeIt.Playwright.Assessment.Configuration;

public static class TestSettings
{
    public const string BaseUrl = "https://demo.snipeitapp.com";

    public static string Username => GetRequiredValue("SNIPEIT_USERNAME");
    public static string Password => GetRequiredValue("SNIPEIT_PASSWORD");

    private static string GetRequiredValue(string variableName)
    {
        return Environment.GetEnvironmentVariable(variableName)
            ?? throw new InvalidOperationException(
                $"Set {variableName} in your terminal before running the test.");
    }
}