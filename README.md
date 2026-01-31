# ⚓️ GodScheduler
### AI-Driven Port Logistics & Gang Allocation System

<div align="center">

![Next.js](https://img.shields.io/badge/Next.js-15-black?style=for-the-badge&logo=next.js&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-Watch_Mode-2496ED?style=for-the-badge&logo=docker&logoColor=white)
![Status](https://img.shields.io/badge/Status-Active_Dev-orange?style=for-the-badge)

**Optimizing "Gang" (Worker) Allocation with AI & Modern Tech.** *Built for the demanding environment of Japanese Port Logistics.*

[Demo (Coming Soon)] | [Documentation](#)

</div>

---

## 📖 Overview

**GodScheduler** is a modern resource management platform designed for **Port Stevedoring (港湾荷役)**.

Managing "Gangs" (teams of port workers) and cargo orders is complex. This system solves it by:
1.  **AI Scheduling:** Auto-assigns workers based on skills and availability.
2.  **Real-Time Updates:** Uses **Next.js + SignalR** for instant dashboard updates.
3.  **Welfare Focus:** Includes a **"Lunch Order System"** to improve worker satisfaction (Lunch is life at the port! 🍱).

> **Note:** This project utilizes **Docker Compose Watch** for a superior Developer Experience (DX) with Hot Reloading for both Backend (.NET) and Frontend (Next.js).

---

## 🏗 Architecture

Modern Monorepo structure fully containerized with Docker.

```mermaid
graph TD
    User[👷 Port Worker / Admin] -->|Browser| FE[💻 Frontend (Next.js 15)]
    
    subgraph "Docker Container Network"
        FE -->|REST / JSON| API[⚙️ Backend API (.NET 9)]
        API -->|EF Core| DB[(🛢 SQL Server 2022)]
        
        API -.->|Hot Reload| Watch[👀 Docker Compose Watch]
        FE -.->|Hot Reload| Watch
    end

```

---

## 🛠 Tech Stack

| Category | Technology | Description |
| --- | --- | --- |
| **Frontend** | Next.js 16 (React) | App Router, Tailwind CSS, Turbopack |
| **Backend** | .NET 9 (C#) | ASP.NET Core Web API, EF Core |
| **Database** | SQL Server 2022 | Docker Container |
| **Infra** | Docker Compose | Watch Mode enabled |

## 🚀 Getting Started

### Prerequisites

* Docker Desktop (installed & running)

1. **リポジトリをクローン**
   ```bash
   git clone [https://github.com/EGAMIJUN/GodScheduler.git](https://github.com/EGAMIJUN/GodScheduler.git)
   cd GodScheduler
'''

2. **Docker 監視モードで起動 (推奨)**
バックエンド・フロントエンド共に、コード修正が即座に反映されます（ホットリロード）。
```bash
# 1. Clone the repository
git clone [https://github.com/EGAMIJUN/GodScheduler.git](https://github.com/EGAMIJUN/GodScheduler.git)
cd GodScheduler

# 2. Start in Watch Mode (Recommended)
docker compose up --watch

```

### 📦 Database Seeding

3. **データベースの初期化 (Seed)**
初回起動時、DBは空の状態です。以下の手順で初期データを投入してください。
* Swagger UI にアクセス: [http://localhost:5078/swagger](https://www.google.com/search?q=http://localhost:5078/swagger)
* `GET /api/Seed` を実行 (Try it out -> Execute)
* ※ これを実行すると、既存のデータはリセットされ、テスト用データが再生成されます。

1. Go to **Swagger UI**: [http://localhost:5078/swagger](https://www.google.com/search?q=http://localhost:5078/swagger)
2. Execute `GET /api/Seed` to populate initial data.

### 🌐 Access Points

* **Frontend:** [http://localhost:3000](https://www.google.com/search?q=http://localhost:3000)
* **API Server:** [http://localhost:5078](https://www.google.com/search?q=http://localhost:5078)
* **Database:** `localhost:1433` (User: `sa` / Pass: `GodScheduler2026`)

---

## 🔮 Roadmap

| Service | Port (Host) | Internal Port | Credential |
| --- | --- | --- | --- |
| **API Server** | `5078` | `8080` | - |
| **Web Client** | `3000` | `3000` | - |
| **Database** | `1433` | `1433` | User: `sa` / Pass: `GodScheduler2026` |

**Jun Egami** *Port-Tech Architect* [GitHub Profile](https://github.com/EGAMIJUN)

* **EGAMIJUN** - Port IT Specialist
