using WebPageArchive.Dto;

namespace WebPageArchive.Services.Abstractions;

interface IDownloadMhtml
{
    Task<string?> Execute(Request request);
}
