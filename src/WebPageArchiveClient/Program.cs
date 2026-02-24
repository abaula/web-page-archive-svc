// See https://aka.ms/new-console-template for more information
using Grpc.Net.Client;
using WebPageArchive.GprsProto;

var channel = GrpcChannel.ForAddress("http://localhost:8000"); // ваш порт сервера
var client = new PageDownloader.PageDownloaderClient(channel);

var request = new DownloadRequest { Url = "https://ixbt.com" };
var response = await client.DownloadPageAsync(request);
var zipBytes = response.ZipArchive.ToByteArray();
// Пишем на диск
var path = Path.Combine(Path.GetTempPath(), "ixbt_test.zip");
await File.WriteAllBytesAsync(path, zipBytes);

Console.WriteLine($"Response: {response.OriginalUrl} to file {path}");