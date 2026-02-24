using System.Net;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using WebPageArchive.Services;

namespace WebPageArchive
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            // Создаём builder для хоста ASP.NET Core.
            var builder = WebApplication.CreateBuilder(args);

            // ---------- Настройка Kestrel под gRPC ----------
            builder.WebHost.ConfigureKestrel(options =>
            {
                options.Listen(IPAddress.Any, 8000, listenOptions =>
                {
                    // gRPC требует HTTP/2
                    listenOptions.Protocols = HttpProtocols.Http2;

                    // Для простоты оставляем без TLS (http://localhost:8000).
                    // В проде можно добавить UseHttps(...) и ходить по https.
                });
            });

            // ---------- DI и gRPC ----------
            // Регистрируем gRPC‑инфраструктуру
            builder.Services.AddGrpc();
            ModuleBootstraper.Bootstrap(builder.Services);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<DownloaderService>();

            // Простой HTTP‑endpoint, чтобы быстро проверить, что сервис жив
            app.MapGet("/", () => "gRPC server (.NET 8, Kestrel) listening on http://localhost:8000");

            // Запуск веб‑приложения (блокирует Main до остановки)
            await app.RunAsync();
        }
    }
}