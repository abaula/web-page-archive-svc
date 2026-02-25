using WebPageArchive.Services;

namespace WebPageArchive
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            // Create builder for ASP.NET Core host.
            var builder = WebApplication.CreateBuilder(args);

            // Configue Kestrel for gRPC.
            builder.WebHost.ConfigureKestrel((context, options) =>
            {
                options.Configure(context.Configuration.GetSection("Kestrel"));
            });
            // Register gRPC.
            builder.Services.AddGrpc();
            // Register Services.
            ModuleBootstraper.Bootstrap(builder.Services);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<DownloaderService>();

            // Run web-app.
            await app.RunAsync();
        }
    }
}