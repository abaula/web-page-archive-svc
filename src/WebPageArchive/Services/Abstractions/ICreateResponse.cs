using WebPageArchive.Dto;
using WebPageArchive.GrpcProto;

namespace WebPageArchive.Services.Abstractions;

interface ICreateResponse
{
    DownloadResponse Execute(Response response);
}
