using System.IO.Compression;
using WebPageArchive.Services.Abstractions;

namespace WebPageArchive.Services;

class CreateZipWithMhtml : ICreateZipWithMhtml
{
    private readonly Lazy<IZipWriter> _zipWriter;

    public CreateZipWithMhtml(Lazy<IZipWriter> zipWriter)
    {
        _zipWriter = zipWriter;
    }

    public byte[] Execute(string mhtml, string fileName)
    {
        // Important: Always dispose ZipArchive before reading the underlying stream.
        // ZipArchive writes essential ZIP structures (e.g. central directory) only on Dispose().
        // If you call ms.ToArray() while the ZipArchive is still open, the archive will be incomplete
        // and most ZIP tools will report it as corrupted. Always close/dispose the ZipArchive first,
        // then read the MemoryStream (ToArray, ToArraySegment, etc.).
        using var ms = new MemoryStream();
        using (var archive = new ZipArchive(ms, ZipArchiveMode.Create, leaveOpen: true))
        {
            _zipWriter.Value.WriteMhtml(archive, mhtml, fileName);
        }

        // Take bytes only after the archive is closed.
        return ms.ToArray();
    }
}
