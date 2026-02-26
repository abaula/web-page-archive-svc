using WebPageArchive.Dto;
using WebPageArchive.GrpcProto;
using WebPageArchive.Services.Abstractions;

namespace WebPageArchive.Services;

class CreateRequest : ICreateRequest
{
    public Request Execute(DownloadRequest request)
    {
        var waitScript = request.HasWaitScript ? request.WaitScript : null;
        var waitTimeoutMs = request.HasWaitTimeoutMs ? (int?)request.WaitTimeoutMs : null;
        return new Request(request.Url, request.UseDefaultWaitScript, waitScript, waitTimeoutMs);
    }
}
