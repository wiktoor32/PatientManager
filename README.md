# PatientManager

PatientManager is an ASP.NET Core Web API developed as a C#/.NET backend learning and portfolio project. It is being expanded into a hospital-oriented patient management system, with an emphasis on clean separation of responsibilities, relational data modeling, input validation, and asynchronous database access.

**Current scope:** Patient management is implemented. Models, relationships, and database tables for doctors, appointments, and emergency contacts are prepared, but their API endpoints and application services are not implemented yet.

## Features

### Implemented

- Create, retrieve, and update patient records, with automatically assigned IDs, active status by default, and UTC creation timestamps
- Search for patients by last name and filter by active status
- Activate and deactivate patients without physically deleting their records
- Retrieve a patient summary
- Use separate request/response DTOs and a dedicated `PatientMapper`
- Validate required fields, name lengths, email addresses, gender, and dates of birth
- Validate the structure, checksum, encoded date, and non-future date of Polish PESEL numbers
- Validate international phone numbers with libphonenumber-csharp and store them in the E.164 format (e.g., +48600123456)
- Store data in Microsoft SQL Server with Entity Framework Core and migrations
- Return appropriate HTTP responses (200, 201, 204, 400, 404); handle unhandled exceptions with ASP.NET Core Problem Details


### Database model prepared for future features

- **Patient → Appointments:** one-to-many; an appointment requires a patient
- **Doctor → Appointments:** one-to-many; an appointment requires a doctor
- **Patient → EmergencyContact:** one-to-zero-or-one; each emergency contact requires a patient
- Required relationships use `DeleteBehavior.NoAction` to prevent cascade deletion of related records
- Entity configurations are kept in `Data/Configurations/` and use EF Core conventions wherever possible

The `Doctors`, `Appointments`, and `EmergencyContacts` tables exist, but their management endpoints are planned for later development.

## Tech stack

- C# and .NET 10
- ASP.NET Core Web API (controllers)
- Entity Framework Core 10, SQL Server provider, and migrations
- Microsoft SQL Server
- LINQ, `async` / `await`, and Dependency Injection
- Data Annotations and custom validation attributes
- `libphonenumber-csharp` for phone number validation and E.164 formatting
- ASP.NET Core Problem Details and OpenAPI
- JetBrains Rider HTTP Client (`.http` requests)

## API endpoints

The following table lists all **11 positive-path requests** from `PatientManager.Api.http`. Several requests use the same route with different data or query parameters, so these represent **8 distinct method-and-route combinations**.

| # | Method | Endpoint | Description | Success response |
| --- | --- | --- | --- | --- |
| 1 | `POST` | `/api/patients` | Create the first patient | `201 Created` |
| 2 | `POST` | `/api/patients` | Create the second patient | `201 Created` |
| 3 | `GET` | `/api/patients/1` | Get patient details by ID | `200 OK` |
| 4 | `GET` | `/api/patients/1/summary` | Get patient summary, including PESEL | `200 OK` |
| 5 | `GET` | `/api/patients` | List all patients | `200 OK` |
| 6 | `PUT` | `/api/patients/1` | Update patient information | `204 No Content` |
| 7 | `GET` | `/api/patients/search?lastName=Jura` | Search patients by last name | `200 OK` |
| 8 | `GET` | `/api/patients/filter?active=true` | Filter active patients | `200 OK` |
| 9 | `GET` | `/api/patients/filter?active=false` | Filter inactive patients | `200 OK` |
| 10 | `PUT` | `/api/patients/1/status?active=false` | Deactivate a patient | `200 OK` |
| 11 | `PUT` | `/api/patients/1/status?active=true` | Reactivate a patient | `200 OK` |

The IDs (`1`, `2`) are examples from the `.http` file and may need adjusting to match your database. Invalid request data is rejected with `400 Bad Request`; requests for non-existent patients return `404 Not Found` where applicable. There is **no `DELETE /api/patients/{id}` endpoint**: patients are deactivated rather than physically deleted.

## Project structure

```text
PatientManager/
├── PatientManager.sln
├── README.md
└── PatientManager.Api/
    ├── Controllers/             # HTTP endpoints
    ├── Data/
    │   ├── AppDbContext.cs      # EF Core context
    │   └── Configurations/      # Fluent API (relationship delete behavior)
    ├── Dtos/                    # Patient request and response DTOs
    ├── Enums/                   # Gender and AppointmentStatus
    ├── Helpers/                 # PESEL and phone number logic
    ├── Mappings/                # PatientMapper
    ├── Migrations/              # Database schema history
    ├── Models/                  # Patient, Doctor, Appointment, EmergencyContact
    ├── Services/                # IPatientService and PatientService
    ├── Validation/              # Custom validation attributes
    ├── Properties/              # Launch profiles
    ├── Program.cs               # Dependency injection and middleware
    └── PatientManager.Api.http  # Positive-path API requests
```

## Getting started

### Prerequisites

- .NET 10 SDK
- Microsoft SQL Server (e.g. SQL Server Express)
- EF Core CLI tool (`dotnet-ef`)

### Setup

1. Clone the repository and open the API project directory:

   ```bash
   cd PatientManager.Api
   ```

2. Configure `ConnectionStrings:DefaultConnection` in `appsettings.Development.json` for your local SQL Server instance. For example, with SQL Server Express and Windows authentication:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=.\\SQLEXPRESS;Database=PatientManagerDb;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;"
     }
   }
   ```

3. Apply the existing migrations:

   ```bash
   dotnet ef database update -- --environment Development
   ```

4. Start the API using the HTTPS launch profile:

   ```bash
   dotnet run --launch-profile https
   ```

The API is available at `https://localhost:7244` when using the default HTTPS launch profile.

API endpoints can be tested using the included `PatientManager.Api.http` file.

## Author

Wiktor Kościelak
