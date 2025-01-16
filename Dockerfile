FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["MagniseTask/MagniseTask.csproj", "./"]
RUN dotnet restore "MagniseTask.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "MagniseTask/MagniseTask.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "MagniseTask.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY MagniseTask/appsettings.json /app/
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MagniseTask.dll"]
