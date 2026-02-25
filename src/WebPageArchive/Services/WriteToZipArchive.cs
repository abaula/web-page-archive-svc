using System.IO.Compression;
using System.Text;
using WebPageArchive.Services.Abstractions;

namespace WebPageArchive.Services;

class WriteToZipArchive : IWriteToZipArchive
{
    public void Execute(ZipArchive archive, string mhtml, string fileName)
    {
        var entry = archive.CreateEntry(fileName, CompressionLevel.Optimal);
        using var entryStream = entry.Open();
        using var writer = new StreamWriter(entryStream, Encoding.UTF8);
        writer.Write(mhtml);
    }
}
