# ShiftMaster

ShiftMaster is a workforce scheduling and shift management platform built with .NET 10 and SQL Server.

## Overview

This repository currently contains the backend foundation and database layer for a multi-tenant scheduling system.

## Architecture

The solution follows a layered architecture:

- **API** (`API/`) - ASP.NET Core Web API host and configuration
- **Services** (`Services/`) - business/application layer (scaffolded)
- **Data** (`Data/`) - Entity Framework Core context and migrations
- **Domain** (`Domain/`) - core entities, enums, and interfaces

Dependency direction:

`API -> Services + Data -> Domain`

## Tech Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server

## Current Status

- Database schema and EF Core migrations are in place
- Domain model and tenant-aware data foundation are completed
- API modules and business services are the next implementation phase

## Prerequisites

- Git
- .NET 10 SDK
- SQL Server (LocalDB / SQL Express / SQL Server)
- Visual Studio 2026 (recommended)

## Getting Started

1. Clone the repository:

```bash
git clone https://github.com/Aditya56Shankar/shiftmaster.git
cd shiftmaster
```

2. Open `shiftmaster.slnx` in Visual Studio.

3. Restore and build:

```bash
dotnet restore shiftmaster.slnx
dotnet build shiftmaster.slnx
```

4. Update the DB connection string in `API/appsettings.json` if needed.

5. Apply migrations:

```bash
dotnet ef database update --project Data --startup-project API
```

6. Run the API:

```bash
dotnet run --project API
```

Default URL: `http://localhost:5229`

## NuGet Packages

Main packages used:

- `Microsoft.AspNetCore.OpenApi` (API)
- `Microsoft.EntityFrameworkCore` (Data)
- `Microsoft.EntityFrameworkCore.Design` (API/Data)
- `Microsoft.EntityFrameworkCore.SqlServer` (Data)
- `Microsoft.EntityFrameworkCore.Tools` (optional for PMC commands)

## Entity Framework Tooling

Install EF CLI tool (if not installed):

```bash
dotnet tool install --global dotnet-ef
```

## Development Workflow

1. Pull latest `main`
2. Create feature branch
3. Implement changes by layer (`Domain` -> `Data` -> `Services` -> `API`)
4. Add/apply migration if schema changes
5. Build and verify locally
6. Push and open PR

## Repository Structure

```text
shiftmaster/
├─ API/
├─ Services/
├─ Data/
├─ Domain/
├─ shiftmaster.slnx
└─ README.md
```
