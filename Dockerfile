# ========== build stage ==========
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Если у проекта есть .sln — скопируйте его и csproj отдельно для кэширования restore
# COPY YourSolution.sln ./
 COPY Protos Protos/
 COPY WebPageArchive/WebPageArchive.csproj WebPageArchive/
 RUN dotnet restore WebPageArchive/WebPageArchive.csproj

# Упрощённый вариант: копируем всё и делаем restore
#COPY . .
#RUN dotnet restore

# Сборка и публикация
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# ========== runtime + Playwright stage ==========
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Установим зависимости для Playwright браузеров
RUN apt-get update && apt-get install -y --no-install-recommends \
    ca-certificates \
    wget \
    gnupg \
    libnss3 \
    libatk1.0-0 \
    libatk-bridge2.0-0 \
    libcups2 \
    libdrm2 \
    libxkbcommon0 \
    libxcomposite1 \
    libxdamage1 \
    libxfixes3 \
    libxrandr2 \
    libgbm1 \
    libpango-1.0-0 \
    libasound2 \
    fonts-liberation \
    libcurl4 \
    && rm -rf /var/lib/apt/lists/*

# Устанавливаем Playwright CLI как dotnet tool
RUN dotnet tool install --global Microsoft.Playwright.CLI
ENV PATH="$PATH:/root/.dotnet/tools"

# Копируем опубликованное приложение
COPY --from=build /app/publish ./

# Устанавливаем браузеры Playwright (chromium/webkit/firefox по необходимости)
RUN playwright install --with-deps --only-shell chromium

# Настройки Kestrel / gRPC:
# gRPC через Kestrel ожидает HTTP/2 и чаще всего HTTPS.
# Для простого старта в контейнере можно слушать HTTP/2 без TLS (при использовании gRPC client, умеющего в h2c):
ENV ASPNETCORE_URLS=http://+:8000
ENV ASPNETCORE_Kestrel__EndpointDefaults__Protocols=Http2

# Если используете appsettings.json для настройки Kestrel, эти env-переменные можно не ставить,
# либо наоборот — использовать их вместо конфигурации.

EXPOSE 8000

ENTRYPOINT ["dotnet", "WebPageArchive.dll"]
