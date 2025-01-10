FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Payhub.Api/Payhub.Api.csproj", "src/Payhub.Api/"]
COPY ["src/Payhub.Application/Payhub.Application.csproj", "src/Payhub.Application/"]
COPY ["src/Payhub.Domain/Payhub.Domain.csproj", "src/Payhub.Domain/"]
COPY ["src/Shared/Shared.csproj", "src/Shared/"]
COPY ["src/Payhub.Infrastructure/Payhub.Infrastructure.csproj", "src/Payhub.Infrastructure/"]
RUN dotnet restore "src/Payhub.Api/Payhub.Api.csproj"
COPY . .
WORKDIR "/src/src/Payhub.Api"
RUN dotnet build "Payhub.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Payhub.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payhub.Api.dll"]
