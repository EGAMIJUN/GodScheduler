# ビルド用ステージ (.NET 9 SDK)
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["GodScheduler.Api.csproj", "./"]
RUN dotnet restore "GodScheduler.Api.csproj"
COPY . .
RUN dotnet publish "GodScheduler.Api.csproj" -c Release -o /app/publish

# 実行用ステージ (ASP.NET Core Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "GodScheduler.Api.dll"]