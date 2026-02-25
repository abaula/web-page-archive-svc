using WebPageArchive.Dto;
using WebPageArchive.GrpcProto;

namespace WebPageArchive.Services.Abstractions;

interface ICreateRequest
{
    Request Execute(DownloadRequest request);
}
