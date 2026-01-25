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

1. **リポジトリをクローン**
   ```bash
   git clone https://github.com/EGAMIJUN/GodScheduler.git
   cd GodScheduler


2. **Docker 監視モードで起動 (推奨) バックエンド・フロントエンド共に、コード修正が即座に反映されます。**
* バックエンド・フロントエンド共に、コード修正が即座に反映されます。
```bash
docker compose up --watch

```


3. **データベースの初期化 (Seed)**
初回起動時、DBは空の状態です。以下の手順で初期データを投入してください。
* Swagger UI にアクセス: [http://localhost:5078/swagger](https://www.google.com/search?q=http://localhost:5078/swagger)
* `GET /api/Seed` を実行 (Try it out -> Execute)


4. **アプリケーションにアクセス**
* フロントエンド (画面): [http://localhost:3000](https://www.google.com/search?q=http://localhost:3000)



## 🏗️ アーキテクチャ情報

* **API Server**: `http://localhost:5078` (Internal: 8080)
* **Web Client**: `http://localhost:3000`
* **Database**: `localhost:1433`
* **User**: `sa`
* **Password**: `GodScheduler2026`



## 👨‍💻 開発者 (Author)

* **EGAMIJUN** - Port IT Specialist

`