using WebPageArchive.Dto;
using WebPageArchive.Services.Abstractions;

namespace WebPageArchive.Services;

class DownloadPage : IDownloadPage
{
    private readonly Lazy<IDownloadMhtml> _downloadMhtml;
    private readonly Lazy<ICreateZipWithMhtml> _createZipWithMhtml;

    public DownloadPage(Lazy<IDownloadMhtml> downloadMhtml,
        Lazy<ICreateZipWithMhtml> createZipWithMhtml)
    {
        _downloadMhtml = downloadMhtml;
        _createZipWithMhtml = createZipWithMhtml;
    }

    public async Task<Response> Execute(Request request)
    {
        var mhtmlResult = await _downloadMhtml.Value.Execute(request);

        if (string.IsNullOrWhiteSpace(mhtmlResult?.Mhtml))
            throw new InvalidOperationException();

        var zipBytes = _createZipWithMhtml.Value.Execute(mhtmlResult.Mhtml!, "index.mhtml");

        return new Response(request.Url, zipBytes, mhtmlResult.Timeout);
    }
}
