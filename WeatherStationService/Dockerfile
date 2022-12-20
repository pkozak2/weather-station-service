#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["WeatherStationService/WeatherStationService.csproj", "WeatherStationService/"]
RUN dotnet restore "WeatherStationService/WeatherStationService.csproj"
COPY . .
WORKDIR "/src/WeatherStationService"
RUN dotnet build "WeatherStationService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WeatherStationService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "WeatherStationService.dll"]