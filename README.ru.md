# Web Page Archive Service

[English version](README.md)

Web Page Archive Service - это сервис для скачивания отдельных веб-страниц с использованием веб-скрейпинга, архивации в [ZIP](https://en.wikipedia.org/wiki/ZIP_(file_format)) и предоставления доступа через [gRPC](https://en.wikipedia.org/wiki/GRPC) [API](https://en.wikipedia.org/wiki/API). Проект позволяет захватывать контент страницы, включая ресурсы, и возвращать его в удобном формате.

## Назначение проекта
Сервис предназначен для автоматизированного скачивания и архивации одной веб-страницы в [ZIP-архив](https://en.wikipedia.org/wiki/ZIP_(file_format)) с помощью [Playwright](https://playwright.dev/) для скрейпинга. Он предоставляет [gRPC](https://en.wikipedia.org/wiki/GRPC)-интерфейс для интеграции в другие системы, упрощая скачивание веб-контента.

Основная цель — создание шаблонного микросервиса, с возможностью дальнейшего расширения, для задач парсинга и хранения страниц в оффлайн-формате [MHTML](https://en.wikipedia.org/wiki/MHTML).

## Технологии
Язык: [C#](https://learn.microsoft.com/en-us/dotnet/csharp/). \
Платформа: [.NET 8](https://dotnet.microsoft.com/). \
API: [gRPC](https://en.wikipedia.org/wiki/GRPC) для высокопроизводительного [RPC](https://en.wikipedia.org/wiki/Remote_procedure_call). \
Веб-скрейпинг: [Playwright](https://github.com/microsoft/playwright-dotnet) (используется [Chromium](https://playwright.dev/dotnet/docs/browsers)). \
Архивация: [ZipArchive](https://learn.microsoft.com/en-us/dotnet/api/system.io.compression.ziparchive?view=net-8.0) — системная библиотека  `System.IO.Compression` для создания ZIP-архивов.

## Архитектура решения

Лучший вариант для долгоживущего сервиса, который много раз создаёт контексты/страницы:
- создавать один IPlaywright и один IBrowser на весь lifetime приложения.
- на каждую задачу скачивания страницы по url, создавать новые IBrowserContext, IPage, ICDPSession, которые уничтожать после использования.

## Разработка

```bash
cd ~/YourWorkDir
git clone git@github.com:abaula/web-page-archive-svc.git
cd web-page-archive-svc
# build solution
dotnet build src/WebPageArchive.sln
# Install powershell (Ubuntu)
sudo apt-get update && install -y powershell
# Install Chromium browser headless only
pwsh src/WebPageArchive/bin/Debug/net8.0/playwright.ps1 install --with-deps --only-shell chromium
```

**Запуск сервера**
```bash
cd ~/YourWorkDir/web-page-archive-svc/src/WebPageArchive
dotnet run
```

**Запуск клиента**
```bash
cd ~/YourWorkDir/web-page-archive-svc/src/WebPageArchiveClient
dotnet run
```

## Публикация в Container

Я использую [Podman](https://podman.io/).

**Создание образа**

```bash
cd ~/YourWorkDir
git clone git@github.com:abaula/web-page-archive-svc.git
cd web-page-archive-svc
podman build -t web-page-archive-svc:1.0.0 .
```

**Создание контейнера**
```bash
podman run -d \
  --name web-page-archive-svc \
  -p 50001:8000 \
  web-page-archive-svc:1.0.0
```

*Примечания*
- `-p 50001:8000` означает: порт 50001 на хосте, порт 8000 в контейнере.
- порт `8000` в контейнере можно изменить в файле `src/WebPageArchive/appsettings.json`.

## Лицензия

GNU GPL 3.0 © 2026 \
Полный текст: [LICENSE](LICENSE)