FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG configuration=Release
WORKDIR /src

# COPY ["Catalog.Api/Catalog.Api.csproj", "Catalog.Api/"]
## RUN dotnet restore "Catalog.Api/Catalog.Api.csproj"
COPY . .

RUN dotnet restore "Users.Api.csproj"
RUN dotnet build "Users.Api.csproj" -c $configuration -o /app/build

FROM build AS publish
RUN dotnet publish "Users.Api.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Users.Api.dll"]
