# Medical Prescription Management Platform

A full-stack web application for managing pharmacies, medical prescriptions and the people around them — patients, doctors, pharmacists and administrators — each with their own role and permissions.

Built as an academic project at North American Private University (IIT), Sfax — December 2024.

<!-- TODO: add a screenshot or short GIF here. One image does more than three paragraphs. -->
<!-- ![Dashboard](docs/screenshot-dashboard.png) -->

## What it does

- **Role-based access** for patients, doctors, pharmacists and administrators
- **Prescription workflow** — doctors issue prescriptions, pharmacists fulfil them, patients track their own
- **Medicine catalogue** management
- **Requests/demands** handling between roles
- **Messaging** between users
- **Email notifications** on key events

## Architecture

The backend is a layered .NET 8 solution rather than a single project:

```
backend/
├── Domain/            # entities and domain contracts
├── Data/              # persistence, EF Core DbContext, migrations
└── WebApplication1/   # ASP.NET Core Web API — controllers, DTOs, configuration
frontend/              # React single-page application
```

Separating domain, persistence and API keeps business entities independent of both the database and the HTTP layer.

## Tech stack

**Backend** — ASP.NET Core 8 Web API · Entity Framework Core (code-first, with migrations) · SQL Server · ASP.NET Core Identity · JWT Bearer authentication · AutoMapper · Swagger / Swashbuckle · MailKit · CORS

**Frontend** — React · Bootstrap 5 · SASS · axios · ApexCharts · FilePond (file uploads)

## Running it locally

### Prerequisites
- .NET 8 SDK
- Node.js 18+
- SQL Server (or SQL Server LocalDB)

### Backend
```bash
cd backend/WebApplication1/WebApplication1
# set your connection string and JWT settings in appsettings.Development.json
dotnet restore
dotnet ef database update
dotnet run
```
Swagger UI is then available at the API's `/swagger` endpoint.

### Frontend
```bash
cd frontend/package
npm install
npm start
```

<!-- TODO: confirm the exact ports and add them here, plus any seed/admin account needed to log in. -->

## Notes

Developed independently as a university project. The data model, API design and frontend were built from scratch.
