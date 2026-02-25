using WebPageArchive.Dto;

namespace WebPageArchive.Services.Abstractions;

interface IDownloadPage
{
    Task<Response> Execute(Request request);
}
