using System.Globalization;

namespace SnipeIt.Playwright.Assessment.Models;

public sealed record AssetData(
    string AssetTag,
    string SerialNumber,
    string Notes,
    string ModelName,
    string Status)
{
    public const string MacbookPro13ModelName = "Macbook Pro 13\"";
    public const string ReadyToDeployStatus = "Ready to Deploy";

    public static AssetData Create()
    {
        var runId = DateTimeOffset.UtcNow.ToString(
            "yyyyMMddHHmmssfff",
            CultureInfo.InvariantCulture);

        return new AssetData(
            AssetTag: runId,
            SerialNumber: $"SERIAL-{runId}",
            Notes: $"Created by Playwright assessment - {runId}",
            ModelName: MacbookPro13ModelName,
            Status: ReadyToDeployStatus);
    }
}