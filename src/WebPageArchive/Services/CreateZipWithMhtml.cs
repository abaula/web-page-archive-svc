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
        using var ms = new MemoryStream();
        using (var archive = new ZipArchive(ms, ZipArchiveMode.Create))
        {
            _zipWriter.Value.WriteMhtml(archive, mhtml, fileName);
            return ms.ToArray();
        }
    }
}
