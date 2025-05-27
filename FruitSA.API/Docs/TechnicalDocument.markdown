# Software Technical Document for FruitSA Web Application

## 1. Introduction

The FruitSA Web Application is a secure, scalable web-based system for managing products and categories for registered users. Built using ASP.NET Core, Entity Framework Core, SQL Server LocalDB, and Bootstrap, it follows N-Tier architecture, SOLID principles, and clean code practices. This document details the system's architecture, technology stack, design patterns, and implementation specifics.

## 2. System Overview

The application enables users to:
- Register and log in using email and password with email confirmation.
- Manage categories (view, add, edit) with unique category codes (e.g., ABC123).
- Manage products (view, add, edit, delete) with auto-generated product codes (e.g., 202505-001), paginated display (10 items per page), and image uploads.
- Import/export products via Excel spreadsheets.
- Ensure security, auditing, concurrency handling, and performance optimization.

## 3. Technology Stack

- **Language**: C#
- **Front-end**:
  - ASP.NET Core MVC (Razor Pages, `FruitSA.Web`)
  - JavaScript, Bootstrap (CSS framework)
- **Middleware**:
  - ASP.NET Core Web API (`FruitSA.API`)
  - Entity Framework Core
  - **MediatR**: For CQRS pattern implementation
  - **FluentValidation**: For validation pipelines
- **Data Persistence**: SQL Server LocalDB
- **Excel Processing**: **ClosedXML**
- **Logging**: **Serilog**
- **Source Control**: Git (hosted on GitHub)
- **Unit Testing**: xUnit (`FruitSA.UnitTest`)

## 4. System Architecture

The application follows an **N-Tier Architecture** with CQRS:

- **Presentation Layer** (`FruitSA.Web`):
  - Built with ASP.NET Core MVC and Razor Pages.
  - Uses Bootstrap for responsive UI.
  - Handles user interactions (registration, login, product/category management).
- **Application Layer** (`FruitSA.API`):
  - Exposes RESTful APIs for CRUD operations.
  - Implements CQRS using MediatR for command and query separation.
  - Uses FluentValidation for input validation.
- **Domain Layer** (`FruitSA.Domain`):
  - Contains domain models (`Product`, `Category`) and business rules.
- **Infrastructure Layer** (`FruitSA.Infrastructure`):
  - Manages data access using Entity Framework Core.
  - Configures database context, migrations, Serilog logging, and email services.
- **Test Layer** (`FruitSA.UnitTest`):
  - Includes unit tests for validation, business logic, and API endpoints.

## 5. Database Design

### Entity Relationship Diagram (ERD)
Available in `docs/ERD.png`. Key entities:

- **Product**:
  - `ProductId` (PK, Guid, Required)
  - `ProductCode` (string, Required, Format: yyyyMM-###, e.g., 202505-001)
  - `Name` (string, Required)
  - `Description` (string, Optional)
  - `CategoryId` (FK, Guid, Required)
  - `Price` (decimal, Required)
  - `Image` (string, Optional, stores file path)
  - `CreatedBy` (string), `CreatedDate` (DateTime), `UpdatedBy` (string), `UpdatedDate` (DateTime)
  - `RowVersion` (byte[], for concurrency)

- **Category**:
  - `CategoryId` (PK, Guid, Required)
  - `Name` (string, Required)
  - `CategoryCode` (string, Required, Format: ABC123)
  - `IsActive` (bool, Required)
  - `CreatedBy` (string), `CreatedDate` (DateTime), `UpdatedBy` (string), `UpdatedDate` (DateTime)
  - `RowVersion` (byte[], for concurrency)

- **User** (via ASP.NET Core Identity):
  - Managed by `IdentityUser` with `Email` and `Password`.

**Relationships**:
- One `Category` to many `Products` (1:N).

## 6. Design Patterns and Principles

- **SOLID Principles**:
  - **Single Responsibility**: Each class (e.g., `ProductCommandHandler`, `CategoryQueryHandler`) handles one responsibility.
  - **Open/Closed**: Interfaces and MediatR pipelines allow extensibility.
  - **Liskov Substitution**: Interfaces (`IProductRepository`, `ICategoryRepository`) ensure substitutability.
  - **Interface Segregation**: Specific interfaces for repositories and services.
  - **Dependency Injection**: Used for services, repositories, and MediatR via ASP.NET Core DI.
- **CQRS**: Implemented using MediatR for command (`CreateProductCommand`, `UpdateCategoryCommand`) and query (`GetProductsQuery`) separation.
- **Repository Pattern**: Abstracts data access (`ProductRepository`, `CategoryRepository`).
- **Unit of Work**: Manages transactions via `DbContext`.
- **DTO Pattern**: Data Transfer Objects for API communication.
- **Factory Pattern**: Used for generating `ProductCode` (e.g., `202505-001`).
- **Auditing**: Tracks `CreatedBy`, `CreatedDate`, `UpdatedBy`, `UpdatedDate`.

## 7. Security

- **Authentication**: ASP.NET Core Identity with **JWT** tokens for API authentication.
  - Configured in `appsettings.json`:
    ```json
    "Jwt": {
      "Key": "The theatricality and deception... are powerful agents to the uninitiated, but we are initiated. Bane",
      "Issuer": "FruitSA.API",
      "Audience": "FruitSA.Client",
      "ExpireHours": 6
    }
    ```
- **Authorization**: Role-based access for authenticated users.
- **Input Validation**:
  - Client-side: JavaScript/Bootstrap for UI validation.
  - Server-side: FluentValidation pipelines for commands (e.g., `CreateProductCommandValidator`).
  - `CategoryCode` validated with regex: `^[A-Z]{3}\d{3}$`.
- **HTTPS**: Enforced for secure communication.
- **Concurrency**: Uses `RowVersion` for optimistic concurrency control.

## 8. Implementation Details

### 8.1 Database Setup
- **Connection String** (in `FruitSA.API/appsettings.json`):
  ```json
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FruitSADB;Integrated Security=True;TrustServerCertificate=True"
  }
  ```
- **Migrations**:
  - Run in the project root:
    ```bash
    dotnet ef migrations add InitialCreateMigration --startup-project FruitSA.API --project FruitSA.Infrastructure
    dotnet ef database update --startup-project FruitSA.API --project FruitSA.Infrastructure
    ```

### 8.2 Logging
- **Serilog**:
  - Configured in `FruitSA.Infrastructure` with fallback to default logging in `appsettings.json`:
    ```json
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft.AspNetCore": "Warning"
      }
    }
    ```
  - Logs API requests, errors, and business logic events to console and file (`FruitSA.API/logs/log.txt`).
  - Example Serilog configuration (if extended):
    ```json
    "Serilog": {
      "WriteTo": [
        { "Name": "Console" },
        { "Name": "File", "Args": { "path": "logs/log.txt" } }
      ]
    }
    ```

### 8.3 CQRS and Validation
- **MediatR**:
  - Commands: `CreateProductCommand`, `UpdateCategoryCommand`, etc.
  - Queries: `GetProductsQuery`, `GetCategoryByIdQuery`, etc.
  - Handlers in `FruitSA.API/Handlers/` process commands and queries.
- **FluentValidation**:
  - Validators (e.g., `CreateProductCommandValidator`) ensure valid input.
  - Example: Validates `CategoryCode` format and `Price` positivity.
  - Pipeline behaviors in MediatR enforce validation before command execution.

### 8.4 Email Configuration
- **EmailSettings** (in `appsettings.json`):
  ```json
  "EmailSettings": {
    "FromEmail": "sfiso2120@gmail.com",
    "FromName": "FruitSA Interviewee",
    "SmtpServer": "smtp.gmail.com",
    "Port": 587,
    "Password": "htfd yeiy zveu mcmc"
  }
  ```
- Used for user registration confirmation emails via SMTP.

### 8.5 Functional Requirements
- **User Registration/Login**: Handled via ASP.NET Core Identity with JWT token generation and email confirmation.
- **Category Management**:
  - CRUD operations via MediatR commands/queries.
  - `CategoryCode` validation using FluentValidation.
  - Concurrency checking via `RowVersion`.
- **Product Management**:
  - Paginated display (10 items/page) using `GetProductsQuery` with `Skip` and `Take`.
  - `ProductCode` generated as `yyyyMM-###` using a factory.
  - Image uploads stored as file paths.
- **Excel Import/Export**:
  - Uses **ClosedXML** for Excel processing.
  - Import: Reads `ProductsTemplate.xlsx` (columns: `Name`, `Description`, `CategoryName`, `Price`).
  - Export: Generates Excel file with product data.
- **Auditing**: Logs user and timestamp for create/update operations.

### 8.6 Performance
- **Paging**: Implemented in `GetProductsQuery` for efficient data retrieval.
- **Indexing**: Database indexes on `ProductCode` and `CategoryCode`.

## 9. UML Diagrams

Available in `docs/UML/`. Includes:
- **Class Diagram**: Shows relationships between `Product`, `Category`, and MediatR handlers.
- **Use Case Diagram**: Outlines user interactions.
- **Sequence Diagram**: Details API call flows with CQRS.

## 10. Testing

- **Unit Tests** (`FruitSA.UnitTest`):
  - Covers FluentValidation rules, MediatR handlers, and API endpoints.
  - Run with:
    ```bash
    cd FruitSA.UnitTest
    dotnet test
    ```
- **Integration Tests**: Test API endpoints with mocked data.

## 11. Setup Instructions

1. **Clone Repository**:
   ```bash
   git clone https://github.com/ssvincen/FruitSAProductManagement.git
   cd FruitSA.Solution
   ```
2. **Restore Dependencies**:
   ```bash
   dotnet restore
   ```
3. **Configure Database**: See Section 8.1.
4. **Build and Run**:
   ```bash
   cd FruitSA.API
   dotnet build
   dotnet run
   ```
5. Access at `https://localhost:60000`.

## 12. Troubleshooting

- **Database Issues**: Ensure LocalDB is running (`sqllocaldb start MSSQLLocalDB`).
- **Build Errors**: Run `dotnet restore` or check `FruitSA.API/Log/FruitSA.API_{2025-05-26}/Date of the log.log`.
- **Validation Errors**: Review Serilog logs for FluentValidation failures.
- **JWT Issues**: Ensure token is included in API requests.
- **Email Issues**: Verify `EmailSettings` in `appsettings.json` and SMTP credentials.

## 13. Support

For issues, create a GitHub issue: [https://github.com/ssvincen/FruitSAProductManagement.git](https://github.com/ssvincen/FruitSAProductManagement.git).