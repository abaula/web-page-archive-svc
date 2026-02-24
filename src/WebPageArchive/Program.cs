using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace WebPageArchive;

internal class Program
{
    public static async Task Main()
    {
        using var playwright = await Playwright.CreateAsync();
        await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true
        });
        await using var context = await browser.NewContextAsync();
        var page = await context.NewPageAsync();
        var gotoOptions = new PageGotoOptions
        {
            WaitUntil = WaitUntilState.DOMContentLoaded,
            Timeout = 60000
        };

        await page.GotoAsync("https://ixbt.com", gotoOptions);
        /*
        await page.ScreenshotAsync(new PageScreenshotOptions
        {
            Path = "screenshot.png",
            FullPage = true
        });
        */

        // Создаём CDP‑сессию для страницы
        await using var client = await context.NewCDPSessionAsync(page);   // аналог new_cdp_session в Python/JS [web:61]

        // Вызываем Page.captureSnapshot, формат по умолчанию = "mhtml" [web:56][web:63]
        var result = await client.SendAsync("Page.captureSnapshot");

        // В result лежит словарь, вытаскиваем поле "data"
        if (result != null)
        {
            // гарантируем ненулевую строку
            var jsonString = result.ToString()!;
            var mhtmlJson = JsonDocument.Parse(jsonString);
            foreach(var el in mhtmlJson.RootElement.EnumerateObject())
            {
                var a = el;
            }


            var mhtml = mhtmlJson.RootElement.GetProperty("data").GetString();

            await File.WriteAllTextAsync("page.mhtml", mhtml ?? "");

            Console.WriteLine("Saved to page.mhtml");
        }

        await page.CloseAsync();
    }
}
