using WebPageArchive.Dto;
using WebPageArchive.GrpcProto;
using WebPageArchive.Services.Abstractions;

namespace WebPageArchive.Services;

class CreateRequest : ICreateRequest
{
    public Request Execute(DownloadRequest request)
    {
        return new Request(request.Url);
    }
}
