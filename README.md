# PatientManager

PatientManager is a simple ASP.NET Core Web API created as a learning project for practicing backend development in C# and .NET.

The application allows managing patient data through REST API endpoints.  
The project focuses on clean separation between controllers and services, DTO usage, Dependency Injection, routing, model binding, input validation and proper HTTP responses.

## Features

- Get all patients
- Get patient by ID
- Create a new patient
- Update patient data
- Delete patient
- Search patients by last name
- Filter patients by active status
- Update patient active status
- Get patient summary
- Input validation using Data Annotations

## Technologies

- C#
- .NET 10
- ASP.NET Core Web API
- LINQ
- Dependency Injection
- OpenAPI
- JetBrains Rider

## Project structure

- `Controllers` – handles HTTP requests and responses
- `Dtos` – request and response models used by the API
- `Models` – application models
- `Services` – application logic and data operations

## Data storage

The current version uses an in-memory collection to store patient data.

The project is planned to be extended with Entity Framework Core and Microsoft SQL Server.

## Running the project

1. Clone the repository.
2. Open the solution in JetBrains Rider, Visual Studio or another .NET-compatible IDE.
3. Run the `PatientManager.Api` project.
4. Use the included `PatientManager.Api.http` file to test the API endpoints.

## Author

Wiktor Kościelak