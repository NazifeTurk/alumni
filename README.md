# Alumni Tracking System

[![Runtime](https://img.shields.io/badge/.NET-7.0%20%7C%20C%23-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Database](https://img.shields.io/badge/PostgreSQL-16-4169E1?logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Container](https://img.shields.io/badge/Docker-Compose-2496ED?logo=docker&logoColor=white)](https://www.docker.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

An enterprise-ready, modular web application engineered to track university graduates, monitor career trajectories, facilitate academic mentorship networks, and coordinate institutional alumni engagement.

> 🗄️ **Database Milestone**: Comprehensive database architecture, ER diagrams, data dictionaries, environment configuration, and "The Forgetting Test" proofs are documented in [**Data Design Documentation (`ddd.md`)**](ddd.md).

---

## 📖 Table of Contents
- [🗄️ Data Design Documentation (ddd.md)](ddd.md)
- [🎯 Project Overview](#-project-overview)
- [🏛️ Architectural Design & Layering](#️-architectural-design--layering)
- [⚖️ Technology Stack & Trade-offs (Defense)](#️-technology-stack--trade-offs-defense)
- [🚀 Quick Start: The One-Command Rule](#-quick-start-the-one-command-rule)
- [🧪 The Forgetting Test (Database Persistence)](#-the-forgetting-test-database-persistence-verification)
- [📡 API Contract & Health Check](#-api-contract--health-check)
- [💼 Relational Domain & JobPostings (Week 06)](#-relational-domain--jobpostings-week-06)
- [🛡️ Enterprise Middleware Pipeline & Error Guardians](#️-enterprise-middleware-pipeline--error-guardians)
- [🚫 Generic 404 Page & Invalid Request Redirection](#-generic-404-page--invalid-request-redirection)
- [📝 Persistent File Error Logging (logs/error.logs)](#-persistent-file-error-logging-logserrorlogs)
- [🧪 Postman Invalid Requests Test Suite](#-postman-invalid-requests-test-suite)
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

## 🧪 The Forgetting Test (Database Persistence Verification)

The **Forgetting Test** is the empirical benchmark that proves the transition from volatile in-memory storage to persistent relational storage:

```text
  [ Step 1: CREATE ]              [ Step 2: RESTART ]             [ Step 3: CHECK ]
  POST /api/users                 docker compose down             GET /api/users
  {"name": "Nazife Türk"}    ───► docker compose up        ───►  "Nazife Türk" is STILL THERE!
                                  (Containers destroyed)          (Persistence confirmed ✅)
```

### How to Run the Test:

1. **Create a new persistent record:**
   ```bash
   curl -X POST http://localhost:8000/api/users \
     -H "Content-Type: application/json" \
     -d '{"name": "Nazife Türk", "graduationYear": 2026, "role": "Software Engineer"}'
   ```

2. **Tear down and restart the entire container stack:**
   ```bash
   docker compose down
   docker compose up -d
   ```

3. **Check the record:**
   ```bash
   curl -s http://localhost:8000/api/users
   ```

> **Why the System Does NOT Forget:**
> In Weeks 1–4, an in-memory store forgets all records on restart. In Week 5, the application connects to **PostgreSQL 16** via **Entity Framework Core**. Docker Compose mounts a persistent named volume (`postgres_data:/var/lib/postgresql/data`). Even when containers are stopped and destroyed (`docker compose down`), the physical database files on host storage persist, guaranteeing that all records survive restarts intact.

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
| :--- | :--- | :--- | :--- | :--- |
| **Users** | `UserController` | `/users` | HTML View | Human interface: List, Add, Detail, Delete alumni |
| **Users** | `ApiUserController` | `/api/users` | JSON | Programmatic REST API with full CRUD |
| **Homework** | `AnnouncementController` | `/announcements` | HTML View | Campus announcements noticeboard & publish form |
| **Homework** | `ApiAnnouncementController` | `/api/announcements` | JSON | REST API backed by `IAnnouncementService` interface |

---

## 💼 Relational Domain & JobPostings (Week 06)

The platform expands into relational multi-entity persistence by modeling **Job Postings** and candidate **Applications** linked to alumni users:

```text
┌─────────────────────────┐          1:N          ┌─────────────────────────┐
│          users          │ ─────────────────────►│      job_postings       │
│  • Id (PK)              │ (Creates Job Posting) │  • Id (PK)              │
│  • Name, Email          │                       │  • UserId (FK -> users) │
│  • GraduationYear, Role │                       │  • Title, Company, City │
└────────────┬────────────┘                       └────────────┬────────────┘
             │                                                 │
             │                    N:M Join                     │
             └──────────────►┌────────────────┐◄───────────────┘
                             │  applications  │
                             │  • Id (PK)     │
                             │  • UserId (FK) │
                             │  • JobId (FK)  │
                             └────────────────┘
```

### 1. Business Logic & Validation Rules
* **`Title` (Başlık)**: **Required**, maximum **120 characters** (`[Required]`, `[StringLength(120)]`). Payloads with missing titles or titles exceeding 120 characters are rejected with **`HTTP 400 Bad Request`**.
* **`Company` (Şirket)**: **Required** (`[Required]`, `[StringLength(100)]`). Missing company values return **`HTTP 400 Bad Request`**.
* **`City` (Şehir)**: **Required** (`[Required]`, `[StringLength(100)]`). Location city (e.g. `'Istanbul'`, `'Ankara'`, `'Remote'`) must be specified; missing values return **`HTTP 400 Bad Request`**.

### 2. Relational Endpoints & Include Pattern
* **Nested Creation Route (`POST /api/users/{id}/jobs`)**: The creator's identity is dictated strictly by the URL path (`/api/users/1/jobs`). The payload contains only job details. Validates user existence (`404` if non-existent) and inserts relational record (`201 Created`).
* **Include Pattern (`GET /api/users/{id}`)**: Performs an optimized SQL `LEFT JOIN` on `job_postings`, returning the user entity with an embedded `"jobs"` collection.
* **Candidate Application (`POST /api/jobs/{id}/applications`)**: Enforces `UNIQUE(user_id, job_posting_id)`. Applying twice to the same job opening triggers **`HTTP 409 Conflict`**.
* **Referential Integrity (`ON DELETE RESTRICT`)**: Deleting a user with active job postings is blocked by database constraints, returning **`HTTP 409 Conflict`** to prevent cascading data loss.

---

## 🛡️ Enterprise Middleware Pipeline & Error Guardians

A robust three-tier middleware architecture intercepts and standardizes all HTTP traffic passing through the Kestrel web server:

```text
                           Incoming Request
                                  │
                                  ▼
      ┌────────────────────────────────────────────────────────┐
      │  ⏱️ Middleware 1: RequestLoggingMiddleware             │
      │  Single-line stdout: time method route restype duration│
      │  Example: "10:20:30 POST /user 201 18ms"               │
      └───────────────────────────┬────────────────────────────┘
                                  │
                                  ▼
      ┌────────────────────────────────────────────────────────┐
      │  🛡️ Middleware 2: ServerErrorGuardianMiddleware (5xx)  │
      │  Catches unhandled exceptions, shields secrets/paths,  │
      │  returns structured JSON with "reason" & "traceId"     │
      └───────────────────────────┬────────────────────────────┘
                                  │
                                  ▼
      ┌────────────────────────────────────────────────────────┐
      │  🛡️ Middleware 3: NotFoundGuardianMiddleware (404)    │
      │  API: Returns structured 404 JSON with domain "reason" │
      │  Web: Passes /404 HTML view without JSON overwriting   │
      └───────────────────────────┬────────────────────────────┘
                                  │
                                  ▼
                 [ Routing / Controllers / MapFallback ]
```

### 1. Request Logging Middleware (`RequestLoggingMiddleware`)
* **Terminal Format**: Benchmark stdout in a single line: `time method route restype duration` (e.g. `10:20:30 POST /user 201 18ms`).
* **Precision**: Uses `System.Diagnostics.Stopwatch` for millisecond-level telemetry.

### 2. 404 Guardian Middleware (`NotFoundGuardianMiddleware`)
* Intercepts HTTP 404 Not Found responses and ensures all API responses contain a clear **`"reason"`** explaining *why* the resource was not found (missing entity ID, unmapped route, or missing parent author).

### 3. 5xx Guardian Middleware (`ServerErrorGuardianMiddleware`)
* Serves as the enterprise crash protector. Catches all unhandled exceptions, shields internal database connection strings and filesystem paths, and responds with a standardized JSON payload including diagnostic **`traceId`** and **`reason`**.
* **Simulation Endpoint**: `GET /api/test/500` intentionally throws an exception for verification.

---

## 🚫 Generic 404 Page & Invalid Request Redirection

To handle invalid web traffic gracefully without breaking machine-to-machine API contracts:

* **Custom 404 HTML View (`src/views/404.html`)**: Styled responsive template featuring dark modern aesthetics, glowing university badge, glitch header, and dynamic route interpolation (`{{REQUESTED_PATH}}`).
* **Endpoint `GET /404`**: Mapped in `BasicRoutes.cs`. Returns `HTTP 404 Not Found` with `Content-Type: text/html; charset=utf-8`.
* **Automatic Redirection (`HTTP 302 Found`)**: Unmapped browser/web requests (e.g. `GET /some-invalid-page`, `GET /unknown`) are automatically redirected to `/404?requestedPath=<escaped-path>`. Following the redirect renders the branded 404 page.
* **API JSON Preservation**: Non-web requests (`/api/...`) bypass HTML redirection and continue returning structured JSON with `"reason"`.

---

## 📝 Persistent File Error Logging (`logs/error.logs`)

All client rejections (4xx), missing routes (404), unhandled server exceptions (5xx), and invalid redirects are appended to disk in **`logs/error.logs`** via `src/services/FileErrorLogger.cs`:
* **Volume Mount**: Mounted via `./logs:/app/logs` in `docker-compose.yml`, guaranteeing log persistence across container rebuilds.
* **Log Entry Pattern**:
  ```text
  [YYYY-MM-DD HH:mm:ss] [STATUS] METHOD PATH - Reason: <reason> [TraceId: <traceId>]
  ```

---

## 🧪 Postman Invalid Requests Test Suite

The repository includes **`postman_alumni_invalid_requests.json`**, an automated 12-scenario test collection covering edge cases and negative assertions:

| # | Test Scenario | HTTP Method & Route | Expected Status | Key Assertion / Assertion Rule |
|---|:---|:---|:---:|:---|
| **1** | Missing Required Field (`Name`) | `POST /api/users` | `400 Bad Request` | Payload must mention missing `Name` |
| **2** | Malformed JSON Syntax | `POST /api/users` | `400 Bad Request` | Catches syntax errors before model binding |
| **3** | Type Mismatch (String for Int) | `POST /api/users` | `400 Bad Request` | Invalid `graduationYear` type rejected |
| **4** | Missing Title & Company | `POST /api/users/1/jobs` | `400 Bad Request` | Nested route requires `Title` and `Company` |
| **4b**| Title Exceeds 120 Chars | `POST /api/users/1/jobs` | `400 Bad Request` | JobPosting rule: Title max 120 characters |
| **4c**| Missing City | `POST /api/users/1/jobs` | `400 Bad Request` | JobPosting rule: City is required |
| **5** | Non-Existent User ID | `GET /api/users/99999` | `404 Not Found` | Guardian JSON with `"reason"` |
| **6** | Nested Route with Missing User | `POST /api/users/99999/jobs` | `404 Not Found` | Explains parent user not found |
| **7** | Apply to Non-Existent Job | `POST /api/jobs/99999/applications` | `404 Not Found` | Explains job posting does not exist |
| **8** | Apply with Non-Existent User | `POST /api/jobs/10/applications` | `404 Not Found` | Explains candidate user does not exist |
| **9** | Duplicate Job Application | `POST /api/jobs/10/applications` | `409 Conflict` | Enforces `UNIQUE(UserId, JobPostingId)` |
| **10**| Delete User with Active Jobs | `DELETE /api/users/1` | `409 Conflict` | Enforces `ON DELETE RESTRICT` |
| **11**| Plain Text to JSON API | `POST /api/users` | `415 Unsupported Media` | Rejects non-JSON `Content-Type` |
| **12**| 5xx Server Exception Simulation | `GET /api/test/500` | `500 Server Error` | Catches crash, returns `reason` & `traceId` |

---

## 📂 Repository Structure

```text
alumni/
├── docker-compose.yml              # Container orchestration (Web API + PostgreSQL + Volume mounts)
├── Dockerfile                      # Multi-stage .NET SDK build & runtime image
├── .env.example                    # Environment configuration template
├── .gitignore                      # .NET build artifacts (bin/, obj/, .vs/) & secrets
├── README.md                       # Master architecture, design decisions, and system contract
├── ddd.md                          # 🗄️ Formal Data Design Documentation & Persistence Proofs
├── postman_alumni_invalid_requests.json # 🧪 12-scenario Postman negative testing collection
├── logs/                           # 📝 Persistent error logging directory (volume-mounted)
│   └── error.logs                  # Runtime audit log (4xx, 5xx, redirects)
└── src/                            # ASP.NET Core Web Application
    ├── Alumni.csproj               # .NET 7 Web SDK project file (Swashbuckle Swagger enabled)
    ├── Program.cs                  # Pipeline bootstrap, Middlewares & Controller mappings
    ├── appsettings.json            # Configuration settings & database connection strings
    ├── models/                     # 📦 Model Layer (Domain Entities & Data Annotations)
    │   ├── User.cs                 # User entity with relational navigation properties
    │   ├── JobPosting.cs           # JobPosting entity (Title max 120, Company, City required)
    │   ├── Application.cs          # N:M Join entity for candidate applications
    │   └── Announcement.cs         # Announcement entity & DTOs
    ├── services/                   # ⚙️ Service & Interface Layer
    │   ├── IUserService.cs / DbUserService.cs         # Database-backed user management
    │   ├── IJobPostingService.cs / DbJobPostingService.cs # Database-backed job postings
    │   ├── IAnnouncementService.cs / DbAnnouncementService.cs # Announcement management
    │   └── FileErrorLogger.cs      # Disk logging service writing to logs/error.logs
    ├── middlewares/                # 🛡️ Enterprise Request Pipeline Bouncers
    │   ├── RequestLoggingMiddleware.cs     # Single-line console benchmark logging
    │   ├── NotFoundGuardianMiddleware.cs   # 404 Guardian with reason & HTML passthrough
    │   └── ServerErrorGuardianMiddleware.cs# 5xx Guardian crash shield with traceId
    ├── Controllers/                # 🎮 Controller Layer (MVC & API)
    │   ├── UserController.cs / ApiUserController.cs       # View & API Controllers for Users
    │   ├── JobPostingController.cs / ApiJobPostingController.cs # View & API for Jobs
    │   └── AnnouncementController.cs / ApiAnnouncementController.cs # Announcements
    ├── routes/                     # Modular Routing Handlers (Week 02, 03 & 04)
    │   ├── BasicRoutes.cs          # Whiteboard routes: /ok, /hello, /sum, /home, /about, /404
    │   ├── UserApiRoutes.cs        # Health check diagnostics (/api/health)
    │   └── AlumniApiRoutes.cs      # Slide D REST resources: /api/alumni
    ├── views/                      # 🎨 Static & Generic HTML Views
    │   └── 404.html                # Branded responsive generic 404 error page
    └── Data/                       # 🗄️ Relational Persistence & Factory Seeding
        ├── AlumniDbContext.cs      # Entity Framework Core DbContext
        ├── DatabaseSeeder.cs       # PostgreSQL seeder & identity sequence reset
        └── Factories/              # Deterministic model factories (UserFactory, etc.)
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
