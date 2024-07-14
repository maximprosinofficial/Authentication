FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Authentication/Authentication.csproj", "Authentication/"]
RUN dotnet restore "Authentication/Authentication.csproj"
COPY . .
WORKDIR "/src/Authentication"
RUN dotnet build "Authentication.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Authentication.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authentication.dll"]
