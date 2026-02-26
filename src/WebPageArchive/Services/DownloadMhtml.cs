using System.Text.Json;
using Microsoft.Playwright;
using WebPageArchive.Dto;
using WebPageArchive.Services.Abstractions;

namespace WebPageArchive.Services;

class DownloadMhtml : IDownloadMhtml
{
    private readonly IBrowser _browser;
    private readonly Lazy<IGetPageEvaluateSettings> _getPageEvaluateSettings;
    private readonly Lazy<IEvaluateWithTimeout> _evaluateWithTimeout;

    public DownloadMhtml(IBrowser browser,
        Lazy<IGetPageEvaluateSettings> getPageEvaluateSettings,
        Lazy<IEvaluateWithTimeout> evaluateWithTimeout)
    {
        _browser = browser;
        _getPageEvaluateSettings = getPageEvaluateSettings;
        _evaluateWithTimeout = evaluateWithTimeout;
    }

    public async Task<MhtmlResult?> Execute(Request request)
    {
        await using var context = await _browser.NewContextAsync();
        var page = await context.NewPageAsync();

        // Dowload page.
        var gotoOptions = new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded
        };
        await page.GotoAsync(request.Url, gotoOptions);

        var pageEvaluateSettings = _getPageEvaluateSettings.Value.Execute(request);
        var timeout = false;

        if (pageEvaluateSettings != null)
            timeout = await _evaluateWithTimeout.Value.Execute(page, pageEvaluateSettings);

        // Create CDP‑session for the page
        await using var session = await context.NewCDPSessionAsync(page);

        // Call Page.captureSnapshot, default response format = "mhtml"
        var result = await session.SendAsync("Page.captureSnapshot");

        if (result == null)
            return default;

        // Get "data" field from json dictionary.
        var jsonString = result.ToString()!;
        var mhtmlJson = JsonDocument.Parse(jsonString);
        var mhtml = mhtmlJson.RootElement.GetProperty("data").GetString();

        return new MhtmlResult(mhtml, timeout);
    }
}
