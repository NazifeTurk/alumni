# Alumni Tracking System

A centralized platform designed to track, connect, and empower university alumni throughout their post-graduation journeys.

---

## 📌 Project Overview

The **Alumni Tracking System** bridges the gap between graduating students, alumni, and academic institutions. The system enables universities to maintain an active directory of their graduates, track career trajectories, facilitate mentorship, organize networking events, and gather institutional feedback.

### Core Objectives & Capabilities
- **Alumni Directory & Profiles**: Comprehensive profiles detailing graduation year, major, current employment, skills, and contact preferences.
- **Career Trajectory & Analytics**: Track employment history, promotions, industries, and geographical distribution.
- **Mentorship & Networking**: Facilitate connections between senior alumni and recent graduates or current students.
- **Job & Internship Board**: Enable alumni to post job openings and refer peers within their organizations.
- **Events & Reunions**: Management of alumni gatherings, webinars, and homecoming events.

---

## 🛠️ Technology Stack & Architectural Decisions

| Layer | Technology | Rationale | One Thing It Is Bad At |
| :--- | :--- | :--- | :--- |
| **Back-end Language** | **Python (FastAPI / Flask)** | Clean syntax, explicit HTTP lifecycle handling, high productivity, and strong typing via Pydantic/type annotations. Excellent fit for step-by-step modular architecture (Routing, MVC, Services). | **CPU-bound execution speed & concurrency**: Python's Global Interpreter Lock (GIL) and interpreted runtime make it significantly slower for heavy computational tasks compared to compiled languages like Go or Rust. |
| **Database** | **PostgreSQL** | Industry standard relational database with ACID compliance, robust relational integrity (foreign keys, joins), advanced indexing, and native JSONB support. | **Horizontal write scaling**: Scaling relational databases horizontally across distributed multi-region nodes requires complex sharding or distributed layers compared to NoSQL databases. |
| **Containerization** | **Docker & Docker Compose** | Reproducible development and deployment environment, ensuring zero configuration drift between local machines and evaluation environments. | **Resource footprint**: Running container daemons on local development laptops incurs higher memory and CPU baseline overhead compared to native processes. |
| **AI Assistant** | **Antigravity (Google DeepMind)** | Autonomous pair programming assistant assisting with architectural structuring, syntax drafting, and container orchestration. | — |

> **Defense Statement**:
> *"We chose Python and PostgreSQL because our 14-week architecture requires explicit control over routing, relational data modeling, and service layer decoupling without fighting opaque framework magic. Python excels at rapid, clean, and readable backend design, but is inherently constrained on CPU-intensive raw processing. PostgreSQL offers strict relational consistency and schema integrity for tracking graduates and their careers, but demands higher operational overhead when scaling writes horizontally across distributed clusters."*

---

## 🚀 Getting Started

The system is containerized and conforms to the single-command startup rule:

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
   - **API Documentation**: [http://localhost:8000/docs](http://localhost:8000/docs)
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

```text
alumni/
├── docker-compose.yml       # Multi-container orchestration (App + PostgreSQL)
├── Dockerfile               # Application container definition
├── .env.example             # Environment variable templates
├── .gitignore               # Ignored files and directories
├── README.md                # Project documentation and defense
└── src/                     # Application source code
    ├── controllers/         # (Week 04) Request handlers & business endpoints
    ├── models/              # (Week 05) Relational ORM models
    ├── routes/              # (Week 02) Routing definitions
    ├── middlewares/         # (Week 09) Authentication, logging, CORS bouncers
    ├── services/            # (Week 12) Business logic & data access layers
    ├── views/               # (Week 10) Presentation layer / templates
    └── main.py              # Application entrypoint
```

---

## 🗓️ 14-Week Roadmap & Progress Tracking

| Week | Topic | Deliverables & Milestones | Status |
| :---: | :--- | :--- | :---: |
| **01** | **Project inception & fundamentals** | Repository setup, stack selection, defense rationale, README | ✅ Completed |
| **02** | **Routing — the doors of the system** | Modular route handlers for Alumni endpoints | ⏳ Planned |
| **03** | **HTTP methods & CRUD** | Full CRUD operations (GET, POST, PUT, DELETE) | ⏳ Planned |
| **04** | **MVC architecture** | Separation into Models, Views/Presenters, Controllers | ⏳ Planned |
| **05** | **Database & ORM** | PostgreSQL schema definition with ORM entities | ⏳ Planned |
| **06** | **Database integration** | Migrations, seed data, and connection pooling | ⏳ Planned |
| **07** | **Relational data & advanced routing** | Foreign keys, nested routes, complex joins | ⏳ Planned |
| **08** | **Midterm — code review** | Repository audit, defense, architecture inspection | ⏳ Planned |
| **09** | **Middleware — the bouncer** | Request validation, auth guard, logging, error handling | ⏳ Planned |
| **10** | **Views & front-end integration** | Server-side rendered views / responsive UI | ⏳ Planned |
| **11** | **Authentication & authorization** | Password hashing, session/JWT auth, role-based access | ⏳ Planned |
| **12** | **Service layers** | Decoupled business logic and transactional services | ⏳ Planned |
| **13** | **Deployment & CI/CD** | Automated testing, build pipeline, container readiness | ⏳ Planned |
| **14** | **Final presentations** | Live handover and system defense | ⏳ Planned |

---

## 📜 Academic Integrity & Commitment

Every commit represents deliberate engineering decisions. In accordance with course guidelines:
- An AI assistant is used for syntax generation and scaffolding.
- Architecture, data modeling, business constraints, and debugging are owned and defended by the author.
- A minimum of one commit is recorded every week with full traceability.
