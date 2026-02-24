using System.IO.Compression;
using Grpc.Core;
using WebPageArchive.GrpcProto;
using WebPageArchive.Services.Abstractions;

namespace WebPageArchive.Services
{
    class DownloaderService : PageDownloader.PageDownloaderBase
    {
        private readonly Lazy<IDownloadMhtml> _downloadMhtml;
        private readonly Lazy<IZipWriter> _zipWriter;

        public DownloaderService(Lazy<IDownloadMhtml> downloadMhtml,
            Lazy<IZipWriter> zipWriter)
        {
            _downloadMhtml = downloadMhtml;
            _zipWriter = zipWriter;
        }

        public override async Task<DownloadResponse> DownloadPage(DownloadRequest request, ServerCallContext context)
        {
            var mhtml = await _downloadMhtml.Value.Download(request.Url);

            if (mhtml == null)
                throw new InvalidOperationException();

            var zipBytes = CreateZipWithMhtml(mhtml!, "page.mhtml");

            return new DownloadResponse
            {
                OriginalUrl = request.Url,
                ZipArchive = Google.Protobuf.ByteString.CopyFrom(zipBytes)
            };
        }

        private byte[] CreateZipWithMhtml(string mhtml, string fileName)
        {
            using var ms = new MemoryStream();
            using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, leaveOpen: true))
            {
                _zipWriter.Value.WriteMhtml(archive, mhtml, fileName);
            }

            // после Dispose ZipArchive данные оказываются в ms
            return ms.ToArray();
        }
    }
}