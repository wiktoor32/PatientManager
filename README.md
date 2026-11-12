# PatientManager

PatientManager is a **RESTful ASP.NET Core Web API** developed as a C#/.NET backend learning and portfolio project. It provides a small patient management system for managing patients, doctors, emergency contacts, and medical appointments using Entity Framework Core and Microsoft SQL Server.

The project focuses on practical backend concepts: layered responsibilities, DTOs, validation, relationships, business rules, asynchronous operations, and database migrations. It also uses ASP.NET Core middleware for centralized exception handling.

## Features

### Patient management

- Create, retrieve, list, and update patients.
- Search patients by last name and filter by active status.
- Activate and deactivate patients instead of permanently deleting them.
- Retrieve a brief patient summary or a detailed patient card with emergency contact information and appointment history.
- Validate required personal information, email addresses, dates of birth, Polish PESEL numbers, and phone numbers.
- Normalize valid phone numbers to the **E.164** format.
- Enforce **unique PESEL values** through a SQL Server unique index.

### Doctor management

- Create, retrieve, list, and update doctors.
- Search by last name or specialization and filter by active status.
- Activate and deactivate doctors.
- Retrieve a detailed doctor card, including appointments and associated patient summaries.

### Emergency contacts

- Add, retrieve, update, or delete a patient's emergency contact.
- Allow **at most one emergency contact per patient**.
- Manage contacts through routes nested under the associated patient.

### Appointment management

- Schedule, retrieve, list, and reschedule appointments, including detailed views with associated patient and doctor information.
- Retrieve appointments by patient or doctor.
- Automatically assign Scheduled status and validate patient and doctor availability, appointment dates, and durations.
- Prevent overlapping appointments for the same doctor, excluding cancelled appointments.
- Allow rescheduling only for future appointments with Scheduled status.
- Support Scheduled, Completed, Cancelled, and NoShow statuses with controlled transitions, preserving appointment history instead of deleting records.
- Handle business errors using a lightweight Result Pattern and C# pattern matching to map errors to appropriate HTTP responses.

## Tech stack

- **C# / .NET 10**
- **ASP.NET Core Web API** (controllers)
- **Entity Framework Core 10** (Code First, Fluent API, migrations)
- **Microsoft SQL Server**
- **LINQ**, **async/await**, and **Dependency Injection**
- **DTOs** and dedicated mapping classes
- **Data Annotations** and custom validation attributes
- **libphonenumber-csharp** for phone validation and E.164 normalization
- **OpenAPI** documentation (Development only)
- **JetBrains Rider HTTP Client** for manual request testing

## API endpoints

All paths below are relative to the API host (for example, `https://localhost:7244`). The tables document the API routes. The `PatientManager.Api.http` file contains sample requests for trying them locally.

### Patients

| Method | Endpoint | Description | Success |
| --- | --- | --- | --- |
| `POST` | `/api/patients` | Create a patient | `201 Created` |
| `GET` | `/api/patients` | Get all patients | `200 OK` |
| `GET` | `/api/patients/{id}` | Get a patient by ID | `200 OK` |
| `GET` | `/api/patients/{id}/summary` | Get a short patient summary | `200 OK` |
| `PUT` | `/api/patients/{id}` | Update patient information | `204 No Content` |
| `GET` | `/api/patients/search/last-name?lastName=Jura` | Search patients by last name | `200 OK` |
| `GET` | `/api/patients/filter?active=true` | Filter patients by active status (`true` or `false`) | `200 OK` |
| `PUT` | `/api/patients/{id}/status?active=false` | Set patient active status (`true` or `false`) | `200 OK` |

Patients are deactivated rather than deleted; there is no `DELETE /api/patients/{id}` endpoint.

### Doctors

| Method | Endpoint | Description | Success |
| --- | --- | --- | --- |
| `POST` | `/api/doctors` | Create a doctor | `201 Created` |
| `GET` | `/api/doctors` | Get all doctors | `200 OK` |
| `GET` | `/api/doctors/{id}` | Get a doctor by ID | `200 OK` |
| `PUT` | `/api/doctors/{id}` | Update doctor information | `204 No Content` |
| `GET` | `/api/doctors/search/last-name?lastName=Zieliński` | Search doctors by last name | `200 OK` |
| `GET` | `/api/doctors/search/specialization?specialization=Kardiologia` | Search doctors by specialization | `200 OK` |
| `GET` | `/api/doctors/filter?active=true` | Filter doctors by active status (`true` or `false`) | `200 OK` |
| `PUT` | `/api/doctors/{id}/status?active=false` | Set doctor active status (`true` or `false`) | `200 OK` |

### Emergency contacts

| Method | Endpoint | Description | Success |
| --- | --- | --- | --- |
| `POST` | `/api/patients/{patientId}/emergency-contact` | Add an emergency contact | `201 Created` |
| `GET` | `/api/patients/{patientId}/emergency-contact` | Get the patient's emergency contact | `200 OK` |
| `PUT` | `/api/patients/{patientId}/emergency-contact` | Update the emergency contact | `204 No Content` |
| `DELETE` | `/api/patients/{patientId}/emergency-contact` | Delete the emergency contact | `204 No Content` |

### Appointments

| Method | Endpoint | Description | Success |
| --- | --- | --- | --- |
| `POST` | `/api/appointments` | Schedule a new appointment | `201 Created` |
| `GET` | `/api/appointments` | Get all appointments | `200 OK` |
| `GET` | `/api/appointments/{id}` | Get an appointment by ID | `200 OK` |
| `PUT` | `/api/appointments/{id}` | Update or reschedule an appointment | `204 No Content` |
| `GET` | `/api/patients/{patientId}/appointments` | Get appointments for a patient | `200 OK` |
| `GET` | `/api/doctors/{doctorId}/appointments` | Get appointments for a doctor | `200 OK` |
| `PUT` | `/api/appointments/{id}/status?status=Cancelled` | Change appointment status (subject to business rules) | `200 OK` |

Appointments are cancelled through the status endpoint rather than physically deleted.

### Detailed views

| Method | Endpoint | Description | Success |
| --- | --- | --- | --- |
| `GET` | `/api/appointments/{id}/details` | Appointment details with nested patient summary and doctor information | `200 OK` |
| `GET` | `/api/patients/{id}/details` | Full patient card with emergency contact and appointments (including doctors) | `200 OK` |
| `GET` | `/api/doctors/{id}/details` | Full doctor card with appointments (including patient summaries) | `200 OK` |

The detailed views use EF Core eager loading (`Include` / `ThenInclude`) and dedicated response DTOs. A patient without an emergency contact receives `emergencyContact: null`; a patient or doctor without appointments receives an empty `appointments` list.

## Data model and database rules

Relationships:

- **Patient 1 → many Appointments**
- **Doctor 1 → many Appointments**
- **Patient 1 → zero or one EmergencyContact**

Entity Framework Core recognizes standard keys and relationships automatically. **Fluent API** is used to define additional database rules:

- A unique index on `Patient.Pesel`.
- Maximum lengths for text fields, such as names, PESEL, email, phone numbers, specialization, relationship, and appointment description.
- `DeleteBehavior.NoAction` to prevent automatic deletion of related records.

Database schema changes are managed through **EF Core migrations** (`InitialCreate`, `ExtendPatientAndAddHospitalEntities`, and `AddEntityConstraints`).

## Project structure

```text
PatientManager/
├── PatientManager.sln
├── README.md
└── PatientManager.Api/
    ├── Controllers/             # API endpoints
    ├── Data/
    │   ├── AppDbContext.cs      # EF Core DbContext
    │   └── Configurations/      # Entity configurations (Fluent API)
    ├── Dtos/                    # Request and response DTOs
    ├── Enums/                   # Gender and AppointmentStatus
    ├── Helpers/                 # PESEL and phone-number helpers
    ├── Mappings/                # Mapping between entities and DTOs
    ├── Migrations/              # EF Core migrations
    ├── Models/                  # Domain entities
    ├── Services/                # Service interfaces and implementations
    ├── Validation/              # Custom validation attributes
    ├── Properties/              # Launch settings
    ├── Program.cs               # DI, middleware, and application setup
    └── PatientManager.Api.http  # Manual API request examples
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