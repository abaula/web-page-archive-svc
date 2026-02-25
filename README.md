# Web Page Archive Service

[Русская версия](README.ru.md)

**Web Page Archive Service** is a service for downloading individual web pages through web scraping, archiving them into [ZIP](https://en.wikipedia.org/wiki/ZIP_(file_format)) files, and providing access via a [gRPC](https://en.wikipedia.org/wiki/GRPC) [API](https://en.wikipedia.org/wiki/API). The project captures page content, including its resources, and returns it in a convenient format.

## Project Purpose

The service automates the downloading and archiving of a single web page into a [ZIP archive](https://en.wikipedia.org/wiki/ZIP_(file_format)) using [Playwright](https://playwright.dev/) for scraping. It exposes a [gRPC](https://en.wikipedia.org/wiki/GRPC) interface for integration into other systems, simplifying the retrieval of web content.

The main goal is to create a **template microservice** that can be easily extended for parsing and storing web pages in offline [MHTML](https://en.wikipedia.org/wiki/MHTML) format.

## Technologies

Language: [C#](https://learn.microsoft.com/en-us/dotnet/csharp/) \
Platform: [.NET 8](https://dotnet.microsoft.com/) \
API: [gRPC](https://en.wikipedia.org/wiki/GRPC) for high-performance [RPC](https://en.wikipedia.org/wiki/Remote_procedure_call) \
Web scraping: [Playwright](https://github.com/microsoft/playwright-dotnet) (uses [Chromium](https://playwright.dev/dotnet/docs/browsers)) \
Archiving: [ZipArchive](https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.ziparchive?view=net-8.0) — built-in `System.IO.Compression` library for creating ZIP archives

## Solution Architecture

The recommended approach for a long-running service that frequently creates browser contexts and pages:

- Create a single `IPlaywright` and `IBrowser` instance for the entire application lifetime.
- For each page download task, create a new `IBrowserContext`, `IPage`, and `ICDPSession`, and dispose of them after use.

## Development

```bash
cd ~/YourWorkDir
git clone git@github.com:abaula/web-page-archive-svc.git
cd web-page-archive-svc
# build solution
dotnet build src/WebPageArchive.sln
# Install PowerShell (Ubuntu)
sudo apt-get update && sudo apt-get install -y powershell
# Install Chromium browser (headless only)
pwsh src/WebPageArchive/bin/Debug/net8.0/playwright.ps1 install --with-deps --only-shell chromium
```

**Starting the server**
```bash
cd ~/YourWorkDir/web-page-archive-svc/src/WebPageArchive
dotnet run
```

**Starting the client**
```bash
cd ~/YourWorkDir/web-page-archive-svc/src/WebPageArchiveClient
dotnet run
```

## Container Deployment

This project uses [Podman](https://podman.io/).

**Build the image**
```bash
cd ~/YourWorkDir
git clone git@github.com:abaula/web-page-archive-svc.git
cd web-page-archive-svc
podman build -t web-page-archive-svc:1.0.0 .
```

**Create the container**
```bash
podman run -d \
  --name web-page-archive-svc \
  -p 50001:8000 \
  web-page-archive-svc:1.0.0
```

**Notes**
- `-p 50001:8000` means port 50001 on the host maps to port 8000 inside the container.
- The container port `8000` can be changed in `src/WebPageArchive/appsettings.json`.

## License

GNU GPL 3.0 © 2026 \
Full text: [LICENSE](LICENSE)
