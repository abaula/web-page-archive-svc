namespace WebPageArchive.Services.Abstractions;

interface ICreateZipWithMhtml
{
    byte[] Execute(string mhtml, string fileName);
}
