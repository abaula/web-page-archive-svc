using System.Text.Json;
using Microsoft.Playwright;
using WebPageArchive.Services.Abstractions;

namespace WebPageArchive.Services
{
    class DownloadMhtml : IDownloadMhtml
    {
        private readonly IBrowser _browser;

        public DownloadMhtml(IBrowser browser)
        {
            _browser = browser;
        }

        public async Task<string?> Download(string url)
        {
            await using var context = await _browser.NewContextAsync();
            var page = await context.NewPageAsync();

            var gotoOptions = new PageGotoOptions
            {
                WaitUntil = WaitUntilState.DOMContentLoaded,
                Timeout = 60000
            };
            await page.GotoAsync(url, gotoOptions);
            // Создаём CDP‑сессию для страницы
            await using var client = await context.NewCDPSessionAsync(page);

            // Вызываем Page.captureSnapshot, формат по умолчанию = "mhtml"
            var result = await client.SendAsync("Page.captureSnapshot");

            if (result == null)
                return default;

            // В result лежит JSON словарь, вытаскиваем поле "data"
            var jsonString = result.ToString()!;
            var mhtmlJson = JsonDocument.Parse(jsonString);
            return mhtmlJson.RootElement.GetProperty("data").GetString();
        }
    }
}