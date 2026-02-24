# web-page-archive-svc
A microservice for loading a web page at a specified URL and converting it to MHTML format.


# Install Playwright

```bash
# dotnet add package Microsoft.Playwright # уже добавлен в проект
dotnet build
```

## install powershell
```bash
# пример для Ubuntu
sudo apt-get update
sudo apt-get install -y powershell
```

## install browsers headless only
```bash
pwsh bin/Debug/net8.0/playwright.ps1 install --with-deps --only-shell
```
