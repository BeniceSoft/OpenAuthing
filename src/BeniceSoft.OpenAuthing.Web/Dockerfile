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
    wget -qO- https://deb.nodesource.com/setup_16.x | bash - && \
    apt-get -qq install -y build-essential nodejs

WORKDIR /src
COPY ["Directory.Packages.props", "Directory.Packages.props"]
COPY ["NuGet.Config", "NuGet.Config"]
COPY ["src/LinkMore.KA.AM.Web/LinkMore.KA.AM.Web.csproj", "src/LinkMore.KA.AM.Web/"]
COPY ["src/LinkMore.KA.AM.Application/LinkMore.KA.AM.Application.csproj", "src/LinkMore.KA.AM.Application/"]
COPY ["src/LinkMore.KA.AM.Application.Contracts/LinkMore.KA.AM.Application.Contracts.csproj", "src/LinkMore.KA.AM.Application.Contracts/"]
COPY ["src/LinkMore.KA.AM.Domain.Shared/LinkMore.KA.AM.Domain.Shared.csproj", "src/LinkMore.KA.AM.Domain.Shared/"]
COPY ["src/LinkMore.KA.AM.Domain/LinkMore.KA.AM.Domain.csproj", "src/LinkMore.KA.AM.Domain/"]
COPY ["src/LinkMore.KA.AM.EntityFrameworkCore/LinkMore.KA.AM.EntityFrameworkCore.csproj", "src/LinkMore.KA.AM.EntityFrameworkCore/"]
COPY ["src/LinkMore.KA.AM.RemoteService.Implements/LinkMore.KA.AM.RemoteService.Implements.csproj", "src/LinkMore.KA.AM.RemoteService.Implements/"]
COPY ["src/LinkMore.KA.AM.RemoteService.Abstractions/LinkMore.KA.AM.RemoteService.Abstractions.csproj", "src/LinkMore.KA.AM.RemoteService.Abstractions/"]
COPY ["src/LinkMore.KA.AM.OAuth/LinkMore.KA.AM.OAuth.csproj", "src/LinkMore.KA.AM.OAuth/"]
COPY ["src/LinkMore.KA.AM.Web/ClientApp/package.json", "src/LinkMore.KA.AM.Web/ClientApp/"]
RUN dotnet restore "src/LinkMore.KA.AM.Web/LinkMore.KA.AM.Web.csproj"
COPY . .
WORKDIR "/src/src/LinkMore.KA.AM.Web"
RUN dotnet build "LinkMore.KA.AM.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LinkMore.KA.AM.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN ln -sf /usr/share/zoneinfo/Asia/Shanghai /etc/localtime
RUN echo 'Asia/Shanghai' > /etc/timezone
ENTRYPOINT ["dotnet", "LinkMore.KA.AM.Web.dll"]
