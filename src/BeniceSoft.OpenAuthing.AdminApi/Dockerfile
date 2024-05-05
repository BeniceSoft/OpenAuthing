FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Packages.props", "Directory.Packages.props"]
COPY ["NuGet.Config", "NuGet.Config"]
COPY ["src/BeniceSoft.OpenAuthing.AdminApi/BeniceSoft.OpenAuthing.AdminApi.csproj", "src/BeniceSoft.OpenAuthing.AdminApi/"]
COPY ["src/BeniceSoft.OpenAuthing.Application/BeniceSoft.OpenAuthing.Application.csproj", "src/BeniceSoft.OpenAuthing.Application/"]
COPY ["src/BeniceSoft.OpenAuthing.Application.Contracts/BeniceSoft.OpenAuthing.Application.Contracts.csproj", "src/BeniceSoft.OpenAuthing.Application.Contracts/"]
COPY ["src/BeniceSoft.OpenAuthing.Domain.Shared/BeniceSoft.OpenAuthing.Domain.Shared.csproj", "src/BeniceSoft.OpenAuthing.Domain.Shared/"]
COPY ["src/BeniceSoft.OpenAuthing.Domain/BeniceSoft.OpenAuthing.Domain.csproj", "src/BeniceSoft.OpenAuthing.Domain/"]
COPY ["src/BeniceSoft.OpenAuthing.EntityFrameworkCore/BeniceSoft.OpenAuthing.EntityFrameworkCore.csproj", "src/BeniceSoft.OpenAuthing.EntityFrameworkCore/"]
COPY ["src/BeniceSoft.OpenAuthing.RemoteService.Implements/BeniceSoft.OpenAuthing.RemoteService.Implements.csproj", "src/BeniceSoft.OpenAuthing.RemoteService.Implements/"]
COPY ["src/BeniceSoft.OpenAuthing.RemoteService.Abstractions/BeniceSoft.OpenAuthing.RemoteService.Abstractions.csproj", "src/BeniceSoft.OpenAuthing.RemoteService.Abstractions/"]
COPY ["src/BeniceSoft.OpenAuthing.Configuration/BeniceSoft.OpenAuthing.Configuration.csproj", "src/BeniceSoft.OpenAuthing.Configuration/"]
RUN dotnet restore "src/BeniceSoft.OpenAuthing.AdminApi/BeniceSoft.OpenAuthing.AdminApi.csproj"
COPY . .
WORKDIR "/src/src/BeniceSoft.OpenAuthing.AdminApi"
RUN dotnet build "BeniceSoft.OpenAuthing.AdminApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "BeniceSoft.OpenAuthing.AdminApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BeniceSoft.OpenAuthing.AdminApi.dll"]
