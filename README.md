# Alumni Tracking System

[![Runtime](https://img.shields.io/badge/.NET-7.0%20%7C%20C%23-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Database](https://img.shields.io/badge/PostgreSQL-16-4169E1?logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Container](https://img.shields.io/badge/Docker-Compose-2496ED?logo=docker&logoColor=white)](https://www.docker.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

An enterprise-ready, modular web application engineered to track university graduates, monitor career trajectories, facilitate academic mentorship networks, and coordinate institutional alumni engagement.

---

## 📖 Table of Contents
- [🎯 Project Overview](#-project-overview)
- [🏛️ Architectural Design & Layering](#️-architectural-design--layering)
- [⚖️ Technology Stack & Trade-offs (Defense)](#️-technology-stack--trade-offs-defense)
- [🚀 Quick Start: The One-Command Rule](#-quick-start-the-one-command-rule)
- [📡 API Contract & Health Check](#-api-contract--health-check)
- [📂 Repository Structure](#-repository-structure)
- [🐳 Containerization & Orchestration](#-containerization--orchestration)
- [🎓 Coursework Specification](#-coursework-specification)

---

## 🎯 Project Overview

The **Alumni Tracking System** bridges academic institutions and their graduates through a structured, data-driven platform. 

### Core System Capabilities
* **Alumni Directory & Profiles**: Centralized records containing graduation year, faculty, department, current professional role, and contact preferences.
* **Career Trajectory & Analytics**: Longitudinal tracking of employment histories, promotions, industries, and geographical distribution.
* **Mentorship & Networking Hub**: Facilitating verified connections between experienced alumni and recent graduates or active undergraduates.
* **Opportunities & Referral Board**: Peer-to-peer job openings, academic research postings, and referral pipelines.
* **Events & Reunions**: Management of homecoming gatherings, webinars, and regional chapter events.

---

## 🏛️ Architectural Design & Layering

The system is constructed with strict separation of concerns following clean enterprise ASP.NET Core architectural patterns:

```text
[ HTTP Client / Browser / Single-Page Application ]
                        │
                        ▼
┌────────────────────────────────────────────────────────┐
│               HTTP Request Pipeline                    │
│   - Middleware: Routing, HSTS, Logging, Exception      │
│   - Security & Authentication Filters                  │
└───────────────────────┬────────────────────────────────┘
                        │
                        ▼
┌────────────────────────────────────────────────────────┐
│               Controller / Endpoint Layer              │
│   - RESTful API Controllers (src/Controllers/)         │
│   - Request validation, parameter mapping & dispatch   │
│   - Standard HTTP response codes (200, 201, 400, 404)  │
└───────────────────────┬────────────────────────────────┘
                        │
                        ▼
┌────────────────────────────────────────────────────────┐
│               Service / Business Logic Layer           │
│   - Domain services & business rules (src/Services/)   │
│   - Decoupled contracts via Interfaces (IAlumniService)│
│   - Dependency Injection registration                  │
└───────────────────────┬────────────────────────────────┘
                        │
                        ▼
┌────────────────────────────────────────────────────────┐
│               Data Access & ORM Layer                  │
│   - Entity Framework Core DbContext (src/Data/)        │
│   - Relational Domain Entities & Mappings (src/Models/)│
│   - Code-First Migrations                              │
└───────────────────────┬────────────────────────────────┘
                        │
                        ▼
┌────────────────────────────────────────────────────────┐
│               Persistent Storage                       │
│   - PostgreSQL 16 Relational Database Engine           │
│   - ACID Compliance, Foreign Key Relational Integrity   │
└────────────────────────────────────────────────────────┘
```

---

## ⚖️ Technology Stack & Trade-offs (Defense)

Every foundational technology in this platform was chosen deliberately, with full awareness of its strengths and architectural compromises:

| Component | Selected Technology | Architectural Rationale | Inherent Trade-off / Weakness |
| :--- | :--- | :--- | :--- |
| **Backend Runtime** | **C# / ASP.NET Core (.NET 7)** | Compile-time type safety, first-class native Dependency Injection, high throughput (Kestrel web server), and explicit support for enterprise MVC and middleware patterns. | **Verbosity & Boilerplate Ceremony**: Compared to lightweight interpreted runtimes (Node.js, Python), C# requires project manifests (`.csproj`), strict type definitions, and compilation pipelines, resulting in slower rapid-prototyping cycles and larger container base image footprints. |
| **Relational Database** | **PostgreSQL 16** | Strict relational integrity, robust ACID guarantees, mature foreign-key relational graph enforcement, and first-class Entity Framework Core integration via `Npgsql`. | **Horizontal Write Scaling**: Relational engines require complex clustering, replication management, and sharding architectures to distribute write-heavy loads across multi-region clusters compared to natively distributed NoSQL data stores. |
| **Orchestration** | **Docker & Docker Compose** | Complete environment determinism, ensuring zero configuration divergence across local machines and evaluation environments. | **Virtualization Overhead**: Local container daemons incur higher memory and CPU baseline resource consumption compared to running native bare-metal executables. |

---

## 🚀 Quick Start: The One-Command Rule

In compliance with system requirements, the entire stack initializes deterministically with a single command:

```bash
docker compose up --build
```

### Prerequisites
* [Docker](https://www.docker.com/) (v24.0+)
* [Docker Compose](https://docs.docker.com/compose/) (v2.0+)
* [Git](https://git-scm.com/)

### Step-by-Step Execution

1. **Clone the repository:**
   ```bash
   git clone https://github.com/NazifeTurk/alumni.git
   cd alumni
   ```

2. **Initialize environment configuration:**
   ```bash
   cp .env.example .env
   ```

3. **Spin up the stack:**
   ```bash
   docker compose up --build
   ```

4. **Access the platform:**
   * **Web API**: [http://localhost:8000](http://localhost:8000)
   * **System Health Diagnostics**: [http://localhost:8000/health](http://localhost:8000/health)
   * **PostgreSQL Engine**: `localhost:5432`

To execute in background (detached mode):
```bash
docker compose up -d
```

To gracefully tear down the environment:
```bash
docker compose down
```

---

## 📡 API Contract & Routing Specifications (Week 02)

The application implements full RESTful resource semantics, parameterized path extraction, query filtering, and status code handling:

### 1. Whiteboard Assigned Endpoints

| Method | Route | Description | Status Code | Example cURL |
| :---: | :--- | :--- | :---: | :--- |
| `GET` | `/` or `/ok` | System health confirmation ("ok") or interactive web dashboard | `200 OK` | `curl -s http://localhost:8000/ok` |
| `GET` | `/hello` | Static greeting response | `200 OK` | `curl -s http://localhost:8000/hello` |
| `GET` | `/hello/{name}` | Dynamic path extraction (e.g. `/hello/emre`) | `200 OK` | `curl -s http://localhost:8000/hello/emre` |
| `GET` | `/sum/{n1}/{n2}` | Mathematical summation with numerical validation | `200 OK` / `400 Bad Request` | `curl -s http://localhost:8000/sum/15/27` |
| `GET` | `/home` | Temporary semantic HTML main landing page | `200 OK` | (Open in browser: `http://localhost:8000/home`) |
| `GET` | `/about` | Temporary semantic HTML about page | `200 OK` | (Open in browser: `http://localhost:8000/about`) |

#### Parameterized Summation Verification:
* **Valid Request**: `GET /sum/15/27` → `200 OK`
  ```json
  {
    "status": "success",
    "operation": "sum",
    "number1": 15,
    "number2": 27,
    "result": 42,
    "expression": "15 + 27 = 42"
  }
  ```
* **Invalid Request (Type Mismatch)**: `GET /sum/abc/27` → `400 Bad Request`
  ```json
  {
    "status": "error",
    "code": 400,
    "message": "Both parameters must be valid numeric values."
  }
  ```

---

### 2. Resource Routing Table (Slide Section D)

| Method | Endpoint | Description | Status Code | Example cURL |
| :---: | :--- | :--- | :---: | :--- |
| `GET` | `/api/alumni` | List alumni resources (optional `?sort=year`) | `200 OK` | `curl -s http://localhost:8000/api/alumni` |
| `POST` | `/api/alumni` | Create new alumnus record | `201 Created` | `curl -X POST http://localhost:8000/api/alumni -H "Content-Type: application/json" -d '{"name":"Ada Lovelace","faculty":"Computing"}'` |
| `GET` | `/api/alumni/{id}` | Dynamic wildcard lookup by ID (e.g. `/api/alumni/42`) | `200 OK` / `404 Not Found` | `curl -s http://localhost:8000/api/alumni/42` |
| `GET` | `/api/alumni/{id}/jobs` | Dynamic nested wildcard route: employment history | `200 OK` / `404 Not Found` | `curl -s http://localhost:8000/api/alumni/42/jobs` |
| `ANY` | `/*` | Unmatched path catch-all fallback | `404 Not Found` | `curl -s http://localhost:8000/invalid-route` |

---

## 🔄 Week 03: HTTP Methods, Status Codes & In-Memory CRUD

As instructed during Week 03 (*"don't use any database yet"*), the platform implements thread-safe in-memory User CRUD operations backed by a `ConcurrentDictionary`, accompanied by interactive Swagger/OpenAPI documentation and REST status codes:

### 📖 Interactive Swagger API Documentation
Explore, test, and execute live HTTP requests directly from the browser without manual Postman configuration:
* **Interactive UI**: [http://localhost:8000/api/swagger](http://localhost:8000/api/swagger) (also accessible at [http://localhost:8000/swagger](http://localhost:8000/swagger))
* **OpenAPI V1 Spec**: [http://localhost:8000/swagger/v1/swagger.json](http://localhost:8000/swagger/v1/swagger.json)

---

### 📡 Week 03 Whiteboard Assigned Endpoints

| # | Method | Endpoint | Description | Status Code | Example cURL |
|---|:---:|:---|:---|:---:|:---|
| **1** | `GET` | `/api/health` | Comprehensive JSON diagnostics & uptime | `200 OK` | `curl -s http://localhost:8000/api/health` |
| **2** | - | *Postman / Swagger* | Use Swagger at `/api/swagger` or Postman for testing | - | [Swagger UI](http://localhost:8000/api/swagger) |
| **3** | `POST` | `/api/users` | Create user in in-memory store | `201 Created` | `curl -X POST http://localhost:8000/api/users -H "Content-Type: application/json" -d '{"name":"Elif Kaya","graduationYear":2024}'` |
| **4** | `GET` | `/api/users` | List all users (pre-seeded slide dataset) | `200 OK` | `curl -s http://localhost:8000/api/users` |
| **4b** | `GET` | `/api/users/{id}` | Retrieve single user by ID | `200 OK` / `404 Not Found` | `curl -s http://localhost:8000/api/users/42` |
| **5a** | `PUT` | `/api/users/{id}` | Replace entire user record | `200 OK` / `404 Not Found` | `curl -X PUT http://localhost:8000/api/users/42 -H "Content-Type: application/json" -d '{"name":"Elif Kaya","email":"elif@iu.edu.tr","graduationYear":2025,"role":"lead"}'` |
| **5b** | `PATCH` | `/api/users/{id}` | Partially update single field | `200 OK` / `404 Not Found` | `curl -X PATCH http://localhost:8000/api/users/42 -H "Content-Type: application/json" -d '{"role":"director"}'` |
| **6** | `DELETE` | `/api/users/{id}` | Remove user from in-memory store | `200 OK` / `404 Not Found` | `curl -X DELETE http://localhost:8000/api/users/42` |

> **Pre-Seeded Alumni Dataset (From Lecture Slide B):**
> 1. `Id: 42` — **Elif Kaya** (Graduation: 2024)
> 2. `Id: 43` — **Mert Aydın** (Graduation: 2021)
> 3. `Id: 44` — **Zeynep Arslan** (Graduation: 2019)
> 4. `Id: 45` — **Can Öztürk** (Graduation: 2023)

---

## 🏛️ Week 04: MVC Architecture & Dual Controller Pattern

In Week 04, the application formalizes the **Model-View-Controller (MVC)** design pattern, demonstrating clear separation of concerns without database coupling:

```text
       ┌────────────────────────────────────────────────────────┐
       │             Browser / Client HTTP Request              │
       └───────────────────────────┬────────────────────────────┘
                                   │
              ┌────────────────────┴────────────────────┐
              ▼                                         ▼
    [ GET/POST /users ]                     [ GET/POST /api/users ]
              │                                         │
              ▼                                         ▼
   ┌──────────────────────┐                  ┌──────────────────────┐
   │    UserController    │                  │  ApiUserController   │
   │  (Web/View Controller│                  │   (API Controller)   │
   └──────────┬───────────┘                  └──────────┬───────────┘
              │                                         │
              ├───────────────┐         ┌───────────────┤
              ▼               ▼         ▼               ▼
      ┌──────────────┐   ┌───────────────────┐   ┌──────────────┐
      │  View Layer  │   │    Model Layer    │   │  JSON Output │
      │ (HTML/CSS UI)│   │ (User/Announcement│   │  (Swagger UI)│
      └──────────────┘   └───────────────────┘   └──────────────┘
```

### 1. Dual Controller Pattern
To differentiate between human-facing UI and machine-facing data contracts:
* **Web / View Controllers (`UserController`, `AnnouncementController`)**: Serves semantic HTML views (`text/html`), table listings, input forms, and redirects.
* **API Controllers (`ApiUserController`, `ApiAnnouncementController`)**: Decorated with `[ApiController]`, serving raw JSON payloads, RFC status codes (`200`, `201`, `400`, `404`), and documented via Swagger.

### 2. Controller & Route Mapping

| Domain | Controller | Base Route | Response Type | Role |
| :--- | :--- | :--- | :---: | :--- |
| **Users** | `UserController` | `/users` | HTML View | Human interface: List, Add, Detail, Delete alumni |
| **Users** | `ApiUserController` | `/api/users` | JSON | Programmatic REST API with full CRUD |
| **Homework** | `AnnouncementController` | `/announcements` | HTML View | Campus announcements noticeboard & publish form |
| **Homework** | `ApiAnnouncementController` | `/api/announcements` | JSON | REST API backed by `IAnnouncementService` interface |

---

## 📂 Repository Structure

```text
alumni/
├── docker-compose.yml              # Container orchestration (Web API + PostgreSQL)
├── Dockerfile                      # Multi-stage .NET SDK build & runtime image
├── .env.example                    # Environment configuration template
├── .gitignore                      # .NET build artifacts (bin/, obj/, .vs/) & secrets
├── README.md                       # Architecture, design decisions, and system contract
└── src/                            # ASP.NET Core Web Application
    ├── Alumni.csproj               # .NET 7 Web SDK project file (Swashbuckle Swagger enabled)
    ├── Program.cs                  # Pipeline bootstrap, Swagger UI & controller mapping
    ├── appsettings.json            # Configuration settings & database connection strings
    ├── models/                     # 📦 Model Layer (Domain Entities & Embedded CRUD)
    │   ├── User.cs                 # User entity with in-memory CRUD methods
    │   └── Announcement.cs         # (Homework) Announcement entity & DTOs
    ├── services/                   # ⚙️ Service & Interface Layer
    │   ├── InMemoryUserService.cs  # In-memory user management
    │   ├── IAnnouncementService.cs # (Homework) Interface contract to manage announcements
    │   └── InMemoryAnnouncementService.cs # (Homework) In-memory announcement implementation
    ├── Controllers/                # 🎮 Controller Layer (MVC & API)
    │   ├── UserController.cs       # View Controller -> renders HTML at /users
    │   ├── ApiUserController.cs    # API Controller -> returns JSON at /api/users
    │   ├── AnnouncementController.cs # (Homework) View Controller -> HTML at /announcements
    │   └── ApiAnnouncementController.cs # (Homework) API Controller -> JSON at /api/announcements
    ├── routes/                     # Modular Routing Handlers (Week 02 & 03)
    │   ├── BasicRoutes.cs          # Whiteboard routes: /ok, /hello, /sum, /home, /about
    │   ├── UserApiRoutes.cs        # Health check diagnostics (/api/health)
    │   └── AlumniApiRoutes.cs      # Slide D REST resources: /api/alumni
    ├── Data/                       # (Week 05 & 06) Entity Framework Core DbContext
    ├── Middlewares/                # (Week 09) Custom HTTP request pipeline bouncers
    └── Views/                      # (Week 10) Presentation layer components
```

---

## 🐳 Containerization & Orchestration

The application container uses a multi-stage Docker build pipeline to maximize build efficiency and minimize final image size:
1. **Build Stage (`mcr.microsoft.com/dotnet/sdk:7.0`)**: Restores NuGet dependencies and compiles optimized release binaries.
2. **Runtime Stage (`mcr.microsoft.com/dotnet/aspnet:7.0`)**: Runs only the lean, security-hardened ASP.NET Core runtime container without SDK build tooling.

The PostgreSQL container includes an automatic health check probe (`pg_isready`) that prevents the web service from accepting traffic until the database engine is fully initialized.

---

## 🎓 Coursework Specification

* **Institution**: Istanbul University (`ogr.iu.edu.tr`)
* **Author**: Nazife Türk
* **Repository**: [github.com/NazifeTurk/alumni](https://github.com/NazifeTurk/alumni)
* **Design Philosophy**: Built from foundational principles without opaque frameworks. Every architectural layer, data model, and service boundary is designed to be fully defended in code reviews.
