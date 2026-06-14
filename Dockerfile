FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["BookingTracker.csproj", "./"]
RUN dotnet restore "BookingTracker.csproj"
COPY . .
RUN dotnet build "BookingTracker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookingTracker.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Ensure data directory exists
RUN mkdir -p /data

ENTRYPOINT ["dotnet", "BookingTracker.dll"]
