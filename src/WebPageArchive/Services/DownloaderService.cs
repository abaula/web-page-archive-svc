using Grpc.Core;
using WebPageArchive.GrpcProto;
using WebPageArchive.Services.Abstractions;

namespace WebPageArchive.Services;

class DownloaderService : PageDownloader.PageDownloaderBase
{
    private readonly Lazy<IDownloadPage> _downloadPage;
    private readonly Lazy<ICreateRequest> _createRequest;
    private readonly Lazy<ICreateResponse> _createResponse;

    public DownloaderService(Lazy<IDownloadPage> downloadPage,
        Lazy<ICreateRequest> createRequest,
        Lazy<ICreateResponse> createResponse)
    {
        _downloadPage = downloadPage;
        _createRequest = createRequest;
        _createResponse = createResponse;
    }

    public override async Task<DownloadResponse> DownloadPage(DownloadRequest request, ServerCallContext context)
    {
        var serviceRequest = _createRequest.Value.Execute(request);
        var serviceResponse = await _downloadPage.Value.Execute(serviceRequest);
        return _createResponse.Value.Execute(serviceResponse);
    }
}
