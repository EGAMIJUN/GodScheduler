# # ビルド用ステージ (.NET 9 SDK)
# FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
# WORKDIR /src
# COPY ["GodScheduler.Api.csproj", "./"]
# RUN dotnet restore "GodScheduler.Api.csproj"
# COPY . .
# RUN dotnet publish "GodScheduler.Api.csproj" -c Release -o /app/publish

# # 実行用ステージ (ASP.NET Core Runtime)
# FROM mcr.microsoft.com/dotnet/aspnet:9.0
# WORKDIR /app
# COPY --from=build /app/publish .
# ENV ASPNETCORE_HTTP_PORTS=8080
# EXPOSE 8080
# ENTRYPOINT ["dotnet", "GodScheduler.Api.dll"]

# --- 1. ビルド用ステージ (SDKイメージ: 重いけどビルド道具が入ってる) ---
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# ★ベストプラクティス4: キャッシュ活用
# 先にプロジェクトファイルだけコピーして restore する
# (ソースコードが変わっても、ここはキャッシュが効くからスキップされる！)
COPY ["GodScheduler.Api.csproj", "./"]
RUN dotnet restore "GodScheduler.Api.csproj"

# その後、ソースコードを全部コピーしてビルド
COPY . .
RUN dotnet publish "GodScheduler.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# --- 2. 実行用ステージ (Runtimeイメージ: 超軽量・本番用) ---
# ★ベストプラクティス1: 軽量ベースイメージ (aspnet)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# ★ベストプラクティス7: セキュリティ (非rootユーザーで実行)
# .NET 8以降のイメージには最初から一般ユーザーがいる
USER $APP_UID

# ビルドステージで作った成果物だけを持ってくる
# ★ベストプラクティス2: マルチステージビルド
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "GodScheduler.Api.dll"]