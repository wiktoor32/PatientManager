# PatientManager

PatientManager is a simple ASP.NET Core Web API created as a learning project for practicing backend development in C# and .NET.

The application provides REST API endpoints for managing patient records stored in a Microsoft SQL Server database. It focuses on separating HTTP handling from application logic, using DTOs, Dependency Injection, input validation, asynchronous database operations, and appropriate HTTP responses.

## Features

- Get all patients or retrieve a patient by ID
- Create, update, and delete patient records
- Search patients by last name
- Filter patients by active status
- Update a patient's active status
- Retrieve a patient summary
- Validate incoming data using Data Annotations and a custom date-of-birth validator
- Store patient data persistently with Entity Framework Core and SQL Server
- Handle unhandled request exceptions using ASP.NET Core Problem Details

## Technologies

- C# / .NET 10
- ASP.NET Core Web API (controllers)
- Entity Framework Core 10 (SQL Server provider and migrations)
- Microsoft SQL Server
- LINQ and async/await
- Dependency Injection
- Data Annotations
- OpenAPI

## API endpoints

| Method | Endpoint | Description |
| --- | --- | --- |
| `GET` | `/api/patients` | Get all patients |
| `GET` | `/api/patients/{id}` | Get a patient by ID |
| `POST` | `/api/patients` | Create a patient |
| `PUT` | `/api/patients/{id}` | Update a patient |
| `DELETE` | `/api/patients/{id}` | Delete a patient |
| `GET` | `/api/patients/search?lastName=...` | Search by last name |
| `GET` | `/api/patients/filter?active=true` | Filter by active status |
| `PUT` | `/api/patients/{id}/status?active=false` | Change active status |
| `GET` | `/api/patients/{id}/summary` | Get a patient summary |

The API uses DTOs for requests and responses. Creating a patient returns `201 Created`; successful updates and deletions return `204 No Content`. Invalid input returns `400 Bad Request`, and requests for missing patients return `404 Not Found` where applicable.

## Project structure

```text
PatientManager/
├── PatientManager.sln
├── README.md
└── PatientManager.Api/
    ├── Controllers/       # HTTP endpoints and responses
    ├── Data/              # EF Core AppDbContext
    ├── Dtos/              # Request and response DTOs
    ├── Migrations/        # EF Core database migrations
    ├── Models/            # Patient entity
    ├── Services/          # Business logic and database operations
    ├── Validation/        # Custom date-of-birth validation
    ├── Properties/        # Launch profiles
    ├── Program.cs         # Application configuration and DI
    └── PatientManager.Api.http
```

## Data storage

Patient records are stored in the `PatientManagerDb` SQL Server database using Entity Framework Core. The database schema is managed through migrations included in this repository.

The database is **not included** in the repository. Each developer creates a local database by configuring a connection string and applying the existing migrations. A new database starts without patient records.


## Getting started

### Prerequisites

- .NET 10 SDK
- Microsoft SQL Server (e.g. SQL Server Express)
- EF Core CLI tools (`dotnet-ef`)

### Setup

1. Clone the repository and navigate to the API project:

   ```bash
   cd PatientManager.Api
   ```

2. Configure `DefaultConnection` in `appsettings.Development.json` to match your SQL Server instance.

   Example for SQL Server Express:

   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=.\\SQLEXPRESS;Database=PatientManagerDb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;"
   }
   ```

3. Apply the existing EF Core migrations to create the database:

   ```bash
   dotnet ef database update -- --environment Development
   ```

4. Run the application:

   ```bash
   dotnet run --launch-profile https
   ```

The API is available at `https://localhost:7244` when using the default HTTPS launch profile.

API endpoints can be tested using the included `PatientManager.Api.http` file.

## Author

Wiktor Kościelak