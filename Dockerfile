FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

COPY src/Protos src/Protos/
COPY src/WebPageArchive src/WebPageArchive/
RUN dotnet publish ./src/WebPageArchive/WebPageArchive.csproj -c Release -r linux-x64 -o ./app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Зависимости для запуска браузеров
RUN apt-get update && apt-get install -y --no-install-recommends \
    ca-certificates wget gnupg

# Install Powershell
RUN wget https://packages.microsoft.com/config/debian/12/packages-microsoft-prod.deb && \
    dpkg -i packages-microsoft-prod.deb && \
    rm packages-microsoft-prod.deb && \
    apt-get update && \
    apt-get install -y powershell

COPY --from=build /app/publish ./
RUN pwsh playwright.ps1 install --with-deps --only-shell chromium

ENTRYPOINT ["dotnet", "/app/WebPageArchive.dll"]
