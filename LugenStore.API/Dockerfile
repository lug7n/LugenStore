FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["LugenStore.API/LugenStore.WebAPI.csproj", "LugenStore.API/"]
COPY ["LugenStore.Application/LugenStore.Application.csproj", "LugenStore.Application/"]
COPY ["LugenStore.Domain/LugenStore.Domain.csproj", "LugenStore.Domain/"]
COPY ["LugenStore.Infrastructure/LugenStore.Infrastructure.csproj", "LugenStore.Infrastructure/"]
RUN dotnet restore "./LugenStore.API/LugenStore.WebAPI.csproj"
COPY . .
WORKDIR "/src/LugenStore.API"
RUN dotnet build "./LugenStore.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./LugenStore.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LugenStore.WebAPI.dll"]
