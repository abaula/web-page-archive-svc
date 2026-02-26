using WebPageArchive.Dto;

namespace WebPageArchive.Services.Abstractions;

interface IDownloadMhtml
{
    Task<MhtmlResult?> Execute(Request request);
}
