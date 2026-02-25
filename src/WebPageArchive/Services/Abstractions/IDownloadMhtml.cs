namespace WebPageArchive.Services.Abstractions;

interface IDownloadMhtml
{
    Task<string?> Execute(string url);
}
