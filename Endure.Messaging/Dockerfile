# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Endure.Messaging/Endure.Messaging.csproj", "Endure.Messaging/"]
COPY ["Endure.Dispatcher/Endure.Dispatcher.csproj", "Endure.Dispatcher/"]
COPY ["Endure.Service/Endure.Service.csproj", "Endure.Service/"]
COPY ["Endure.Data/Endure.Data.csproj", "Endure.Data/"]
RUN dotnet restore "./Endure.Messaging/Endure.Messaging.csproj"
COPY . .
WORKDIR "/src/Endure.Messaging"
RUN dotnet build "./Endure.Messaging.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Endure.Messaging.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Endure.Messaging.dll"]