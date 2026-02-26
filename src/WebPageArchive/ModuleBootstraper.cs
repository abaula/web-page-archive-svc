using Microsoft.Playwright;
using WebPageArchive.Dto;
using WebPageArchive.Services;
using WebPageArchive.Services.Abstractions;

namespace WebPageArchive;

static class ModuleBootstraper
{
    public static void Bootstrap(IServiceCollection services, IConfigurationManager configuration)
    {
        // Options
        services.Configure<PageWaitSettings>(configuration.GetSection(nameof(PageWaitSettings)));

        // Services
        // ... IPlaywright as singleton.
        services.AddSingleton<IPlaywright>(sp =>
            Playwright.CreateAsync().GetAwaiter().GetResult());

        // ... IBrowser as singleton.
        services.AddSingleton<IBrowser>(sp =>
        {
            var playwright = sp.GetRequiredService<IPlaywright>();
            // Use Chromium.
            return playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = true
            }).GetAwaiter().GetResult();
        });

        // ... App services
        services.AddScopedWithLazy<IDownloadMhtml, DownloadMhtml>();
        services.AddScopedWithLazy<ICreateZipWithMhtml, CreateZipWithMhtml>();
        services.AddScopedWithLazy<IWriteToZipArchive, WriteToZipArchive>();
        services.AddScopedWithLazy<IDownloadPage, DownloadPage>();
        services.AddScopedWithLazy<ICreateRequest, CreateRequest>();
        services.AddScopedWithLazy<ICreateResponse, CreateResponse>();
        services.AddScopedWithLazy<IEvaluateWithTimeout, EvaluateWithTimeout>();
        services.AddScopedWithLazy<IGetPageEvaluateSettings, GetPageEvaluateSettings>();
        services.AddScopedWithLazy<IGetResourceText, GetResourceText>();
    }
}
