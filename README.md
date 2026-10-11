# Alumni Tracking System

A centralized, enterprise-grade platform designed to track, connect, and empower university alumni throughout their post-graduation journeys.

---

## 📌 Project Overview

The **Alumni Tracking System** bridges the gap between graduating students, alumni, and academic institutions. The system enables universities to maintain an active directory of their graduates, track career trajectories, facilitate mentorship, organize networking events, and gather institutional feedback.

### Core Objectives & Capabilities
- **Alumni Directory & Profiles**: Comprehensive profiles detailing graduation year, faculty, major, current employment, skills, and contact preferences.
- **Career Trajectory & Analytics**: Track employment history, promotions, industries, and geographical distribution.
- **Mentorship & Networking**: Facilitate connections between senior alumni and recent graduates or current students.
- **Job & Internship Board**: Enable alumni to post job openings and refer peers within their organizations.
- **Events & Reunions**: Management of alumni gatherings, webinars, and homecoming events.

---

## 🛠️ Technology Stack & Architectural Decisions

| Layer | Technology | Rationale | One Thing It Is Bad At |
| :--- | :--- | :--- | :--- |
| **Back-end Language & Framework** | **C# / ASP.NET Core (.NET 7)** | Enterprise-grade type safety, high-performance asynchronous runtime (Kestrel), first-class native Dependency Injection, and direct alignment with MVC, routing, and middleware patterns. | **Verbosity & Boilerplate Ceremony**: C# requires significant ceremony compared to scripting languages—explicit type declarations, `.csproj` project manifests, and compilation steps, leading to larger container sizes and slower prototyping cycles. |
| **Database & ORM** | **PostgreSQL & Entity Framework Core** | Industry-standard relational integrity, ACID compliance, mature schema migrations, and first-class EF Core support via `Npgsql`. | **Horizontal Write Scalability**: Relational databases demand complex sharding and clustering setups to scale write throughput across distributed multi-region environments compared to NoSQL data stores. |
| **Containerization** | **Docker & Docker Compose** | Reproducible development and deployment environment, guaranteeing that the entire system boots cleanly with a single command (`docker compose up`). | **Resource Overhead**: Container virtualization and .NET runtime images consume higher baseline memory and CPU overhead compared to running single compiled binaries directly on the host OS. |
| **AI Assistant** | **Antigravity (Google DeepMind)** | Autonomous pair programming assistant assisting with architectural structuring, syntax drafting, and container orchestration. | — |

> **Defense Statement (What to say out loud to your professor):**
> *"I have chosen **C# with ASP.NET Core** for the backend and **PostgreSQL** for the database.*
> 
> *I chose C# and ASP.NET Core because our 14-week curriculum covers Routing, MVC, ORM, Middleware, and Service Layers—all of which are first-class native citizens in .NET. It gives us compile-time type safety, world-class Dependency Injection, and Entity Framework Core, which is the gold standard for relational ORM.*
> 
> *One thing C# is bad at is **verbosity and boilerplate ceremony**: compared to scripting languages like Python or JavaScript, every change requires project files, explicit type definitions, and compilation steps, resulting in slower quick-and-dirty iteration.*
> 
> *I chose PostgreSQL because tracking graduates, jobs, and faculties demands strict relational integrity and ACID guarantees. One thing PostgreSQL is bad at is **horizontal write scaling**, which requires complex clustering and sharding compared to distributed NoSQL databases."*

---

## 🚀 Getting Started

The system conforms to the course's cardinal rule: **single-command startup**.

### Prerequisites
- [Docker](https://www.docker.com/) (version 24.0 or higher)
- [Docker Compose](https://docs.docker.com/compose/) (v2.0 or higher)
- [Git](https://git-scm.com/)

### Quick Start

1. **Clone the repository:**
   ```bash
   git clone https://github.com/<your-username>/alumni.git
   cd alumni
   ```

2. **Configure environment variables:**
   ```bash
   cp .env.example .env
   ```

3. **Start the system:**
   ```bash
   docker compose up --build
   ```

4. **Verify the services:**
   - **Web Application**: [http://localhost:8000](http://localhost:8000)
   - **Health Check**: [http://localhost:8000/health](http://localhost:8000/health)
   - **PostgreSQL Database**: `localhost:5432`

To run in the background (detached mode):
```bash
docker compose up -d
```

To stop all services:
```bash
docker compose down
```

---

## 📂 Project Architecture

The directory structure is organized to cleanly accommodate the 14-week roadmap:

```text
alumni/
├── docker-compose.yml              # Multi-container orchestration (App + PostgreSQL)
├── Dockerfile                      # Multi-stage .NET SDK build & runtime container
├── .env.example                    # Environment variable templates
├── .gitignore                      # .NET and environment ignore rules
├── README.md                       # Project documentation and architectural defense
└── src/                            # ASP.NET Core Application
    ├── Alumni.csproj               # .NET Project file
    ├── Program.cs                  # WebApplication builder, pipeline & root routes
    ├── appsettings.json            # Configuration and connection strings
    ├── Controllers/                # (Week 04) MVC & API Controllers
    ├── Models/                     # (Week 05) Entity Framework Domain Models & DTOs
    ├── Data/                       # (Week 05 & 06) AlumniDbContext & Migrations
    ├── Middlewares/                # (Week 09) Custom pipeline middlewares
    ├── Services/                   # (Week 12) Business logic & Service Interfaces
    └── Views/                      # (Week 10) Razor views & UI components
```

---

## 🗓️ 14-Week Roadmap & Progress Tracking

| Week | Topic | C# / ASP.NET Implementation Plan | Status |
| :---: | :--- | :--- | :---: |
| **01** | **Project inception & fundamentals** | C# / .NET 7 project setup, Dockerfile, defense rationale, README | ✅ Completed |
| **02** | **Routing — the doors of the system** | Minimal API routes & Controller attribute routing (`[Route("api/[controller]")]`) | ⏳ Planned |
| **03** | **HTTP methods & CRUD** | `[HttpGet]`, `[HttpPost]`, `[HttpPut]`, `[HttpDelete]` with standard HTTP status codes | ⏳ Planned |
| **04** | **MVC architecture** | Separation of Models, Views, and Controllers pattern | ⏳ Planned |
| **05** | **Database & ORM** | Entity Framework Core setup, `AlumniDbContext`, Code-First entities | ⏳ Planned |
| **06** | **Database integration** | PostgreSQL integration (`Npgsql`), EF Core migrations, seeding | ⏳ Planned |
| **07** | **Relational data & advanced routing** | Navigation properties, foreign keys, 1-to-N relationships, nested routes | ⏳ Planned |
| **08** | **Midterm — code review** | Repository code audit, architectural inspection, defense | ⏳ Planned |
| **09** | **Middleware — the bouncer** | Custom ASP.NET Core middleware (`IMiddleware`, logging, error handling) | ⏳ Planned |
| **10** | **Views & front-end integration** | Razor Views / SSR presentation layer | ⏳ Planned |
| **11** | **Authentication & authorization** | ASP.NET Core Identity / JWT tokens, `[Authorize]` guards | ⏳ Planned |
| **12** | **Service layers** | Dependency Injection (`IServiceCollection`), decoupled `IAlumniService` | ⏳ Planned |
| **13** | **Deployment & CI/CD** | Multi-stage Docker optimization, health checks, GitHub Actions CI | ⏳ Planned |
| **14** | **Final presentations** | Live system handover and defense | ⏳ Planned |

---

## 📜 Academic Integrity & Commitment

Every commit represents deliberate engineering decisions. In accordance with course guidelines:
- An AI assistant is used for syntax generation and scaffolding.
- Architecture, data modeling, business constraints, and debugging are owned and defended by the author.
- A minimum of one commit is recorded every week with full traceability.
