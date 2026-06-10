# ShiftMaster

A web-based frontline workforce scheduling and shift management system for enterprise environments.  
Built as a **Multi-Tenant SaaS** platform using **React** (frontend) and **.NET 10 ASP.NET Core** (backend) with **SQL Server**.

## Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Technology Stack](#technology-stack)
- [User Roles](#user-roles)
- [Development Phases and Current Status](#development-phases-and-current-status)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [NuGet Dependencies](#nuget-dependencies)
- [Entity Framework Tools](#entity-framework-tools)
- [API Endpoint Reference](#api-endpoint-reference)
- [Business Rules and Constraints](#business-rules-and-constraints)
- [Multi-Tenancy Rules for Developers](#multi-tenancy-rules-for-developers)
- [Daily Development Workflow](#daily-development-workflow)
- [Standard Feature Development Sequence](#standard-feature-development-sequence)
- [Troubleshooting](#troubleshooting)
- [Repository Structure](#repository-structure)

---

## Overview

ShiftMaster centralizes shift planning, roster creation, employee availability, real-time attendance tracking, overtime management, shift swap workflows, and labour cost analytics for enterprise clients such as retail chains, manufacturing plants, hospitals, contact centres, and hospitality businesses.

Each enterprise client is a **Tenant**. A single deployed instance of the API and database securely isolates all data per tenant. Users from one enterprise client can never view, query, or modify another client's data under any circumstances.

---

## Architecture

### Physical 3-Tier Architecture

```
[ TIER 1: CLIENT TIER ]
+-------------------------------------------------------+
|                 REACT FRONTEND (SPA)                  |
|  (Views, UI Components, React Router, Local State)    |
+-------------------------------------------------------+
                          |
                          |  HTTP Requests (JSON)
                          v
[ TIER 2: APPLICATION TIER ]
+-------------------------------------------------------+
|              ASP.NET CORE WEB API BACKEND             |
|                                                       |
|  +-----------------------------------------------+   |
|  | LAYER 1: PRESENTATION (/Controllers)          |   |
|  | Translates HTTP requests and validates DTOs   |   |
|  +-----------------------------------------------+   |
|                          |                            |
|                          v                            |
|  +-----------------------------------------------+   |
|  | LAYER 2: BUSINESS LOGIC (/Services)           |   |
|  | Enforces rules (rest periods, overtime, etc.) |   |
|  +-----------------------------------------------+   |
|                          |                            |
|                          v                            |
|  +-----------------------------------------------+   |
|  | LAYER 3: DATA ACCESS (/Data and /Domain)      |   |
|  | Entity Framework Core and SQL Connections     |   |
|  +-----------------------------------------------+   |
+-------------------------------------------------------+
                          |
                          |  TCP/IP (SQL Queries)
                          v
[ TIER 3: DATABASE TIER ]
+-------------------------------------------------------+
|                       SQL SERVER                      |
|  (Relational Tables: Users, Rosters, Locations, ...)  |
+-------------------------------------------------------+
```

### Logical Layer Responsibilities

| Layer | Folder | Responsibility |
| :--- | :--- | :--- |
| Presentation | `API/Controllers/` | Receives HTTP requests, validates DTOs, returns HTTP responses. No business logic allowed. |
| Middleware | `API/Middlewares/` | Intercepts requests, extracts JWT, injects `TenantId` into request scope. |
| Business Logic | `Services/` | Enforces HR rules, scheduling constraints, and validations before any save. |
| Data Access | `Data/` | `ApplicationDbContext`, EF Core mappings, migrations, and global query filters. |
| Domain | `Domain/` | Core entities, enums, and the `IMustHaveTenant` interface. |

### Code Dependency Direction

```
API  ->  Services + Data  ->  Domain
```

---

## Technology Stack

| Layer | Technology |
| :--- | :--- |
| Frontend | React (SPA) |
| Backend API | ASP.NET Core Web API (.NET 10) |
| ORM | Entity Framework Core 10.0.8 |
| Database | SQL Server |
| Authentication | ASP.NET Core Identity + JWT |
| Authorization | Role-Based Access Control (RBAC) |

---

## User Roles

| Role | Portal | Core Responsibility |
| :--- | :--- | :--- |
| **Scheduling Admin** | Admin Console | Configures locations, shift patterns, skill requirements, and constraint rules. |
| **Frontline Employee** | Employee Portal | Submits availability, leave blocks, views schedules, and requests shift swaps. |
| **Shift Supervisor** | Supervisor Console | Builds rosters, monitors attendance, approves swaps, assigns emergency covers. |
| **HR Business Partner** | HR Dashboard | Monitors labour compliance, overtime accumulation, and absenteeism trends. |
| **Operations Manager** | Operations View | Reviews cross-location staffing dashboards, skill gaps, and scheduling efficiency. |
| **Payroll Executive** | Payroll Export Console | Exports finalized approved timesheets and overtime data for payroll processing. |

---

## Development Phases and Current Status

| Phase | Description | Status |
| :--- | :--- | :--- |
| **Phase 1.0** | System Architecture and Database Design | Complete |
| **Phase 2.0** | Backend API Development (.NET) | Up Next |
| **Phase 3.0** | Frontend Development (React) | Pending |
| **Phase 4.0** | Integration and Testing | Pending |

### Phase 1 - Completed

- .NET ASP.NET Core REST API scaffolded (4-project layered solution)
- All domain entities and enums defined in `Domain/`
- `IMustHaveTenant` interface applied across all organizational entities
- `ApplicationDbContext` configured with full entity mappings and relationship rules
- EF Core global query filters for tenant data isolation
- Restrictive cascade deletes and B-Tree indexing on `TenantId`
- Initial migration and `FixCascadePaths` migration applied

### Phase 2 - Next Steps (Backend API)

Developers picking up from here should implement the following in order:

1. **Authentication and Identity** - ASP.NET Core Identity setup, JWT generation with `TenantId` claim, login/register endpoints
2. **Tenant Middleware** - Extract `TenantId` from JWT and inject into `ApplicationDbContext.CurrentTenantId` per request
3. **DTOs** - Create request/response DTOs for each module (never expose raw entity models to the frontend)
4. **Service Interfaces and Implementations** - Build business logic in `Services/` following interfaces in `Domain/Interfaces/`
5. **Controllers** - Wire up all REST endpoints listed in the [API Endpoint Reference](#api-endpoint-reference) below
6. **Constraint Validation** - Implement hard and soft scheduling constraint checks in the Services layer

---

## Prerequisites

1. Git
2. .NET 10 SDK
3. SQL Server (LocalDB / SQL Express / full instance)
4. Visual Studio 2026 with ASP.NET and .NET workloads
5. Node.js *(required for React frontend - Phase 3)*

---

## Getting Started

### 1. Clone and open

```bash
git clone https://github.com/Aditya56Shankar/shiftmaster.git
cd shiftmaster
```

Open `shiftmaster.slnx` in Visual Studio.

### 2. Verify .NET SDK

```bash
dotnet --version
```

Expected: `10.x`

### 3. Restore and build

```bash
dotnet restore shiftmaster.slnx
dotnet build shiftmaster.slnx
```

> In Visual Studio, package restore runs automatically on open/build.

### 4. Configure database connection

Edit `API/appsettings.json` and update `ConnectionStrings:DefaultConnection` to match your local SQL Server:

```json
"ConnectionStrings": {
  "DefaultConnection": "server=.\\SQLEXPRESS;database=shiftMasterDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

### 5. Apply migrations

```bash
dotnet ef database update --project Data --startup-project API
```

This creates the `shiftMasterDB` database and applies all existing migrations under `Data/Migrations/`.

### 6. Run the API

```bash
dotnet run --project API
```

Default local URL: `http://localhost:5229`

### 7. Quick verification

Use `API/API.http` or send:

```
GET http://localhost:5229/weatherforecast
```

---

## NuGet Dependencies

All packages are defined in the project files and restore automatically.

### API project

| Package | Version |
| :--- | :--- |
| `Microsoft.AspNetCore.OpenApi` | `10.0.8` |
| `Microsoft.EntityFrameworkCore.Design` | `10.0.8` |

### Data project

| Package | Version |
| :--- | :--- |
| `Microsoft.EntityFrameworkCore` | `10.0.8` |
| `Microsoft.EntityFrameworkCore.Design` | `10.0.8` |
| `Microsoft.EntityFrameworkCore.SqlServer` | `10.0.8` |

If manual installation is needed in Visual Studio:

1. Right-click the target project -> **Manage NuGet Packages**
2. Open the **Browse** tab
3. Search for the package name
4. Select version `10.0.8` -> **Install**

---

## Entity Framework Tools

### Option A: .NET CLI - recommended

Install once globally:

```bash
dotnet tool install --global dotnet-ef
```

### Option B: Visual Studio Package Manager Console

For `Add-Migration` and `Update-Database` commands, install in the **API** project:

```bash
dotnet add API package Microsoft.EntityFrameworkCore.Tools --version 10.0.8
```

---

## API Endpoint Reference

> `TenantId` is **never** passed by the frontend. The backend derives it automatically from the user's JWT token.

### Identity and Access Management

| Method | Endpoint | Table | Purpose |
| :--- | :--- | :--- | :--- |
| POST | `/api/tenants` | Tenant | System Admin only: onboard a new enterprise client |
| GET, POST, PUT, DELETE | `/api/users` | User | Manage employee profiles and role assignments |
| GET | `/api/audit-logs` | AuditLog | Retrieve full audit trail for compliance |

### Location, Department and Shift Configuration

| Method | Endpoint | Table | Purpose |
| :--- | :--- | :--- | :--- |
| GET, POST, PUT, DELETE | `/api/locations` | WorkLocation | Define and manage work locations |
| GET, POST, PUT, DELETE | `/api/shift-patterns` | ShiftPattern | Configure shift templates and rules |
| GET, POST, PUT, DELETE | `/api/skill-requirements` | SkillRequirement | Manage skill counts required per shift |

### Employee Availability and Preferences

| Method | Endpoint | Table | Purpose |
| :--- | :--- | :--- | :--- |
| GET, POST, PUT | `/api/availability` | AvailabilitySubmission | Submit and update weekly availability windows |
| GET, POST, PUT | `/api/leaves` | LeaveBlock | Record and manage leave requests |
| GET, POST, PUT, DELETE | `/api/employee-skills` | EmployeeSkill | Manage employee skill proficiency records |

### Roster Planning and Publication

| Method | Endpoint | Table | Purpose |
| :--- | :--- | :--- | :--- |
| GET, POST, PUT | `/api/rosters` | WeeklyRoster | Generate, update, and publish weekly schedules |
| GET, POST, PUT, DELETE | `/api/rosters/{id}/assignments` | ShiftAssignment | Assign employees to shift slots within a roster |
| GET, PUT | `/api/rosters/{id}/violations` | SchedulingConstraintViolation | Flag and override constraint rule violations |

### Attendance Tracking and Timekeeping

| Method | Endpoint | Table | Purpose |
| :--- | :--- | :--- | :--- |
| GET, POST, PUT | `/api/attendance` | AttendanceRecord | Record clock-in/clock-out times against shifts |
| GET, POST, PUT | `/api/timesheets` | TimesheetSummary | Aggregate hours for payroll approval |

### Shift Swap, Cover and Overtime

| Method | Endpoint | Table | Purpose |
| :--- | :--- | :--- | :--- |
| GET, POST, PUT | `/api/swaps` | SwapRequest | Employee shift trade requests and supervisor approval |
| GET, POST, PUT | `/api/covers` | CoverAssignment | Emergency cover assignments |
| GET, POST, PUT | `/api/overtime` | OvertimeAuthorisation | Overtime requests and authorizations |

### Labour Analytics and Notifications

| Method | Endpoint | Table | Purpose |
| :--- | :--- | :--- | :--- |
| GET, POST | `/api/reports` | LabourReport | Generate HR and operations metrics |
| GET, PUT | `/api/notifications` | Notification | In-app alerts and read/dismiss status |

---

## Business Rules and Constraints

These rules must be implemented in the **Services layer** and map to the `SchedulingConstraintViolation` entity.

### Hard Constraints - Block the save

| Rule | Description |
| :--- | :--- |
| **Double Booking** | An employee cannot have overlapping `ShiftAssignment` records |
| **Leave Conflict** | An employee with an approved `LeaveBlock` cannot be scheduled on those dates |
| **Skill Minimums** | A roster cannot be published if assigned skill counts fall below `SkillRequirement.MinCountPerShift` |

### Soft Constraints - Allow save, generate warning, require supervisor override in AuditLog

| Rule | Description |
| :--- | :--- |
| **Minimum Rest Period** | Minimum **11 hours** rest between end of one shift and start of the next |
| **Maximum Weekly Hours** | Cannot exceed `AvailabilitySubmission.MaxHoursPerWeek` or standard limits (40 hrs + 8 OT) without active `OvertimeAuthorisation` |
| **Maximum Consecutive Days** | No more than **6 consecutive calendar days** without a mandatory 24-hour rest block |
| **Availability Mismatch** | Warning if assigned shift falls outside submitted availability days or preferred shift types |

---

## Multi-Tenancy Rules for Developers

1. **Every** organizational or transactional domain model must implement `IMustHaveTenant`
2. **Do not** manually add tenant filters or indexes - the reflection-based helper in `OnModelCreating` applies them automatically to all `IMustHaveTenant` entities
3. **Use composite indexes** for natural keys (e.g. `TenantId` + `EmployeeID`) to allow the same keys across different tenants
4. **Every JWT token** must include `TenantId` as a mandatory claim - the middleware injects it into `ApplicationDbContext.CurrentTenantId` per request
5. **Never** trust `TenantId` values from frontend request bodies or query strings

---

## Daily Development Workflow

1. Pull latest `main`
2. Create a feature branch: `feature/<module>-<description>`
3. Implement in layer order: `Domain` -> `Data` -> `Services` -> `API`
4. Add migration if schema changed:

```bash
dotnet ef migrations add <MigrationName> --project Data --startup-project API
```

5. Apply migration:

```bash
dotnet ef database update --project Data --startup-project API
```

6. Build and verify locally before committing
7. Push branch and open pull request against `main`

---

## Standard Feature Development Sequence

Follow this exact sequence for every new feature:

| Step | Action |
| :--- | :--- |
| 1 | Create the entity class in `Domain/models/` |
| 2 | Add `DbSet<T>` to `ApplicationDbContext` in `Data/` |
| 3 | Run `Add-Migration` then `Update-Database` |
| 4 | Create the service interface in `Domain/Interfaces/` |
| 5 | Implement the service class in `Services/Implementation/` |
| 6 | Create request/response DTOs in `Services/DTOs/` |
| 7 | Create the controller in `API/Controllers/` using constructor injection |

> **Key rules:** Use constructor DI - never `new`. Always use `async`/`await` for database calls. Never return raw entity models to the frontend; always use DTOs.

---

## Troubleshooting

### SQL connection issues

- Verify SQL Server instance name in `API/appsettings.json`
- Ensure the SQL Server service is running
- For SQL Express: confirm instance is `.\SQLEXPRESS`

### EF migration command fails

- Run commands from the repository root
- Confirm `dotnet-ef` global tool is installed:

```bash
dotnet tool install --global dotnet-ef
```

### Port conflict

- Update `applicationUrl` in `API/Properties/launchSettings.json`
- Or stop the process already using port `5229`

---

## Repository Structure

```
shiftmaster/
|-- API/
|   |-- Controllers/       <- LAYER 1: HTTP endpoints (Phase 2)
|   |-- Middlewares/       <- JWT tenant extraction (Phase 2)
|   |-- Properties/
|   |-- appsettings.json
|   `-- Program.cs
|-- Services/
|   |-- DTOs/              <- Request/Response contracts (Phase 2)
|   |-- Interfaces/        <- Service contracts (Phase 2)
|   `-- Implementation/    <- Business logic (Phase 2)
|-- Data/
|   |-- Context/
|   |   `-- ApplicationDbContext.cs
|   `-- Migrations/        <- InitialSetup + FixCascadePaths applied
|-- Domain/
|   |-- models/            <- All entity classes defined
|   |-- Enums/             <- All domain enums defined
|   `-- Interfaces/
|       `-- Interfaces.cs  <- IMustHaveTenant interface
|-- shiftmaster.slnx
`-- README.md
```
