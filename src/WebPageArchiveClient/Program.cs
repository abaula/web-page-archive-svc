// See https://aka.ms/new-console-template for more information
using Grpc.Net.Client;
using WebPageArchive.GrpcProto;

var channel = GrpcChannel.ForAddress("http://localhost:8000");
var client = new PageDownloader.PageDownloaderClient(channel);

var request = new DownloadRequest { Url = "https://ixbt.com" };
var response = await client.DownloadPageAsync(request);
var zipBytes = response.ZipArchive.ToByteArray();
// Write bytes to disk.
var path = Path.Combine(Path.GetTempPath(), "ixbt_test.zip");
await File.WriteAllBytesAsync(path, zipBytes);

Console.WriteLine(
    $"\u001b[32mResponse from \u001b[36m{response.OriginalUrl}\u001b[32m " +
    $"has been written to \u001b[33m{path}\u001b[0m"
);
