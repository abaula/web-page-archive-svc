
namespace WebPageArchive.Services.Abstractions
{
    interface IDownloadMhtml
    {
        Task<string?> Download(string url);
    }
}