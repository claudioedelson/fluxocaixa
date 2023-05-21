#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80
ENV DOTNET_NOLOGO=true
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true
ENV DOTNET_EnableDiagnostics=0

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["FluxoDeCaixa.API/FluxoDeCaixa.API.csproj", "FluxoDeCaixa.API/"]
COPY ["FluxoDeCaixa.Application/FluxoDeCaixa.Application.csproj", "FluxoDeCaixa.Application/"]
COPY ["FluxoDeCaixa.Domain/FluxoDeCaixa.Domain.csproj", "FluxoDeCaixa.Domain/"]
COPY ["FluxoDeCaixa.Shared/FluxoDeCaixa.Shared.csproj", "FluxoDeCaixa.Shared/"]
COPY ["FluxoDeCaixa.Infrastructure/FluxoDeCaixa.Infrastructure.csproj", "FluxoDeCaixa.Infrastructure/"]
RUN dotnet restore "FluxoDeCaixa.API/FluxoDeCaixa.API.csproj"
COPY . .
WORKDIR "/src/FluxoDeCaixa.API"
RUN dotnet build "FluxoDeCaixa.API.csproj" -c Release -o /app/build --no-restore

FROM build AS publish
RUN dotnet publish "FluxoDeCaixa.API.csproj" -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FluxoDeCaixa.API.dll"]