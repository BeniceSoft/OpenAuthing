FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
# Install Node.js
RUN sed -i 's/deb.debian.org/mirrors.tuna.tsinghua.edu.cn/g' /etc/apt/sources.list && \
    sed -i 's/security.debian.org/mirrors.tuna.tsinghua.edu.cn/g' /etc/apt/sources.list && \
    apt-get update && \
    apt-get -qq install -y wget && \
    apt-get -qq install -y gnupg2 && \
    wget -qO- https://deb.nodesource.com/setup_18.x | bash - && \
    apt-get -qq install -y build-essential nodejs

WORKDIR /src
COPY ["Directory.Packages.props", "Directory.Packages.props"]
COPY ["NuGet.Config", "NuGet.Config"]
COPY ["src/BeniceSoft.OpenAuthing.SSO/BeniceSoft.OpenAuthing.SSO.csproj", "src/BeniceSoft.OpenAuthing.SSO/"]
COPY ["src/BeniceSoft.OpenAuthing.Application/BeniceSoft.OpenAuthing.Application.csproj", "src/BeniceSoft.OpenAuthing.Application/"]
COPY ["src/BeniceSoft.OpenAuthing.Application.Contracts/BeniceSoft.OpenAuthing.Application.Contracts.csproj", "src/BeniceSoft.OpenAuthing.Application.Contracts/"]
COPY ["src/BeniceSoft.OpenAuthing.Domain.Shared/BeniceSoft.OpenAuthing.Domain.Shared.csproj", "src/BeniceSoft.OpenAuthing.Domain.Shared/"]
COPY ["src/BeniceSoft.OpenAuthing.Domain/BeniceSoft.OpenAuthing.Domain.csproj", "src/BeniceSoft.OpenAuthing.Domain/"]
COPY ["src/BeniceSoft.OpenAuthing.EntityFrameworkCore/BeniceSoft.OpenAuthing.EntityFrameworkCore.csproj", "src/BeniceSoft.OpenAuthing.EntityFrameworkCore/"]
COPY ["src/BeniceSoft.OpenAuthing.RemoteService.Implements/BeniceSoft.OpenAuthing.RemoteService.Implements.csproj", "src/BeniceSoft.OpenAuthing.RemoteService.Implements/"]
COPY ["src/BeniceSoft.OpenAuthing.RemoteService.Abstractions/BeniceSoft.OpenAuthing.RemoteService.Abstractions.csproj", "src/BeniceSoft.OpenAuthing.RemoteService.Abstractions/"]
COPY ["src/BeniceSoft.OpenAuthing.OAuth/BeniceSoft.OpenAuthing.OAuth.csproj", "src/BeniceSoft.OpenAuthing.OAuth/"]
COPY ["src/BeniceSoft.OpenAuthing.SSO/ClientApp/package*.json", "src/BeniceSoft.OpenAuthing.SSO/ClientApp/"]
COPY ["src/BeniceSoft.OpenAuthing.SSO/ClientApp/.npmrc", "src/BeniceSoft.OpenAuthing.SSO/ClientApp/"]
RUN dotnet restore "src/BeniceSoft.OpenAuthing.SSO/BeniceSoft.OpenAuthing.SSO.csproj"
COPY . .
WORKDIR "/src/src/BeniceSoft.OpenAuthing.SSO"
RUN dotnet build "BeniceSoft.OpenAuthing.SSO.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BeniceSoft.OpenAuthing.SSO.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime
RUN echo 'Asia/Shanghai' > /etc/timezone
ENTRYPOINT ["dotnet", "BeniceSoft.OpenAuthing.SSO.dll"]
