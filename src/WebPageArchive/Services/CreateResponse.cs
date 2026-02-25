using WebPageArchive.Dto;
using WebPageArchive.GrpcProto;
using WebPageArchive.Services.Abstractions;

namespace WebPageArchive.Services;

class CreateResponse : ICreateResponse
{
    public DownloadResponse Execute(Response response)
    {
        return new DownloadResponse
        {
            OriginalUrl = response.OriginalUrl,
            ZipArchive = Google.Protobuf.ByteString.CopyFrom(response.ZipArchive)
        };
    }
}
