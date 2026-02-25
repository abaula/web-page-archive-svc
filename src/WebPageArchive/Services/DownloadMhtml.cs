using System.Text.Json;
using Microsoft.Playwright;
using WebPageArchive.Services.Abstractions;

namespace WebPageArchive.Services;

class DownloadMhtml : IDownloadMhtml
{
    private readonly IBrowser _browser;

    public DownloadMhtml(IBrowser browser)
    {
        _browser = browser;
    }

    public async Task<string?> Execute(string url)
    {
        await using var context = await _browser.NewContextAsync();
        var page = await context.NewPageAsync();

        // Dowload page.
        var gotoOptions = new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        };
        await page.GotoAsync(url, gotoOptions);

        // Create CDP‑session for the page
        await using var session = await context.NewCDPSessionAsync(page);

        // Call Page.captureSnapshot, default response format = "mhtml"
        var result = await session.SendAsync("Page.captureSnapshot");

        if (result == null)
            return default;

        // Get "data" field from json dictionary.
        var jsonString = result.ToString()!;
        var mhtmlJson = JsonDocument.Parse(jsonString);
        return mhtmlJson.RootElement.GetProperty("data").GetString();
    }
}
