using System.IO.Compression;

namespace WebPageArchive.Services.Abstractions;

interface IWriteToZipArchive
{
    void Execute(ZipArchive archive, string mhtml, string fileName);
}
