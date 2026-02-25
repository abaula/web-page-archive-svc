using Microsoft.Playwright;
using WebPageArchive.Services;
using WebPageArchive.Services.Abstractions;

namespace WebPageArchive
{
    static class ModuleBootstraper
    {
        public static void Bootstrap(IServiceCollection services)
        {
            // IPlaywright как singleton, инициализируется асинхронно
            services.AddSingleton<IPlaywright>(sp =>
                Playwright.CreateAsync().GetAwaiter().GetResult());

            // IBrowser как singleton, шарится между всеми сервисами
            services.AddSingleton<IBrowser>(sp =>
            {
                var playwright = sp.GetRequiredService<IPlaywright>();
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
}