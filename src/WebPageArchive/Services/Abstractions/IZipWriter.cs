using System.IO.Compression;

namespace WebPageArchive.Services.Abstractions;

interface IZipWriter
{
    void WriteMhtml(ZipArchive archive, string mhtml, string fileName);
}
