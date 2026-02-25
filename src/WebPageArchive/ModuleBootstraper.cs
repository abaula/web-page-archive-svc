using Microsoft.Playwright;
using WebPageArchive.Services;
using WebPageArchive.Services.Abstractions;

namespace WebPageArchive;

static class ModuleBootstraper
{
    public static void Bootstrap(IServiceCollection services)
    {
        // IPlaywright as singleton.
        services.AddSingleton<IPlaywright>(sp =>
            Playwright.CreateAsync().GetAwaiter().GetResult());

        // IBrowser as singleton.
        services.AddSingleton<IBrowser>(sp =>
        {
            var playwright = sp.GetRequiredService<IPlaywright>();
            // Use Chromium.
            return playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            }).GetAwaiter().GetResult();
        });

        services.AddScopedWithLazy<IDownloadMhtml, DownloadMhtml>();
        services.AddScopedWithLazy<ICreateZipWithMhtml, CreateZipWithMhtml>();
        services.AddScopedWithLazy<IZipWriter, ZipWriter>();
        services.AddScopedWithLazy<IDownloadPage, DownloadPage>();
        services.AddScopedWithLazy<ICreateRequest, CreateRequest>();
        services.AddScopedWithLazy<ICreateResponse, CreateResponse>();
    }
}
