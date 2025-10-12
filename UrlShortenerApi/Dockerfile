FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# Copy solution and project files so restore uses the renamed solution
COPY UrlShortner.sln ./
# Copy both projects referenced by the solution so restore can find them
COPY UrlShortenerApi/UrlShortenerApi.csproj UrlShortenerApi/
COPY UrlShortenerApi.UnitTests/UrlShortenerApi.UnitTests.csproj UrlShortenerApi.UnitTests/
RUN dotnet restore UrlShortner.sln
COPY UrlShortenerApi/. UrlShortenerApi/
COPY UrlShortenerApi.UnitTests/. UrlShortenerApi.UnitTests/
WORKDIR /src/UrlShortenerApi
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UrlShortenerApi.dll"]
