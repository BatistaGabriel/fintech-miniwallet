FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY Fintech.MiniWallet.slnx ./
COPY src/Fintech.MiniWallet.Domain/Fintech.MiniWallet.Domain.csproj src/Fintech.MiniWallet.Domain/
COPY src/Fintech.MiniWallet.Application/Fintech.MiniWallet.Application.csproj src/Fintech.MiniWallet.Application/
COPY src/Fintech.MiniWallet.Infrastructure/Fintech.MiniWallet.Infrastructure.csproj src/Fintech.MiniWallet.Infrastructure/
COPY src/Fintech.MiniWallet.Api/Fintech.MiniWallet.Api.csproj src/Fintech.MiniWallet.Api/
COPY tests/Fintech.MiniWallet.UnitTests/Fintech.MiniWallet.UnitTests.csproj tests/Fintech.MiniWallet.UnitTests/
COPY tests/Fintech.MiniWallet.IntegrationTests/Fintech.MiniWallet.IntegrationTests.csproj tests/Fintech.MiniWallet.IntegrationTests/

RUN dotnet restore

COPY . .

RUN dotnet publish src/Fintech.MiniWallet.Api/Fintech.MiniWallet.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Fintech.MiniWallet.Api.dll"]
