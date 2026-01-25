# 🚢 GodScheduler (港湾荷役AI配番システム)

港湾業務における「作業員（ギャング）の配置」と「荷役オーダー」をAIが自動最適化するスケジューリングシステムです。
Docker Compose Watch を採用し、モダンな開発体験（Hot Reload）を実現しています。

## 🛠️ 技術スタック (Tech Stack)

| Category | Technology | Description |
| --- | --- | --- |
| **Frontend** | Next.js 16 (React) | App Router, Tailwind CSS |
| **Backend** | .NET 9 (C#) | ASP.NET Core Web API, EF Core |
| **Database** | SQL Server 2022 | Docker Container |
| **Infra** | Docker Compose | Watch Mode enabled |

## 🚀 環境構築 (Getting Started)

### 前提条件 (Prerequisites)
* Docker Desktop がインストールされていること

### 起動手順 (How to start)

1. リポジトリをクローン
   ```bash
   git clone [https://github.com/EGAMIJUN/GodScheduler.git](https://github.com/EGAMIJUN/GodScheduler.git)
   cd GodScheduler