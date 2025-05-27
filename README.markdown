# FruitSA Web Application

## Overview

The FruitSA Web Application is a secure, scalable web-based system for managing products and categories, developed as part of the Fruit South Africa Developer Test. Built using ASP.NET Core, Entity Framework Core, SQL Server LocalDB, and Bootstrap, it follows N-Tier architecture, SOLID principles, and clean code practices. Key features include user authentication, category and product management with pagination, Excel import/export, image uploads, and comprehensive auditing.

## Technology Stack

- **Language**: C#
- **Front-end**: ASP.NET Core MVC, JavaScript, Bootstrap
- **Middleware**: ASP.NET Core Web API, Entity Framework Core, MediatR (CQRS), FluentValidation
- **Data Persistence**: SQL Server LocalDB
- **Excel Processing**: ClosedXML
- **Logging**: Serilog
- **Source Control**: Git (GitHub)

## Prerequisites

To set up and run the project locally, ensure you have the following installed:
- .NET 8 SDK
- SQL Server LocalDB
- Git
- Visual Studio 2022 (recommended)

## Setup Instructions

For detailed step-by-step instructions to clone, configure, and run the application, refer to the [How-To Guide](docs/HowToGuide.md).(Doc/TechnicalDocument)

### Quick Start
1. Clone the repository:
   ```bash
   git clone https://github.com/ssvincen/FruitSAProductManagement.git
   cd FruitSA.Solution
   ```
2. Restore dependencies:
   ```bash
   dotnet restore
   ```
3. Configure the database (see [How-To Guide](docs/HowToGuide.md) for details).
4. Build and run the Solution:
   ```bash
   cd FruitSA.Solution
   dotnet build
   dotnet run
   ```
5. Access the application at `https://localhost:60000`.

## Documentation

- **Technical Details**: For system architecture, design patterns, database schema, and implementation details, see the [Software Technical Document](docs/TechnicalDocument.md).
- **UML Diagrams and ERD**: Available in `docs/UML/` and `docs/ERD.png` for class, use case, sequence diagrams, and entity relationships.
- **Setup Guide**: The [How-To Guide](docs/HowToGuide.md) provides detailed instructions for setting up and using the application.

## Repository Structure

- `docs/`: Documentation, including How-To Guide, Technical Document, UML diagrams, and ERD.
- `FruitSA.API/`: API layer with RESTful endpoints.
- `FruitSA.Application/`: Application layer (business logic)  and CQRS implementation.
- `FruitSA.Domain/`: Domain models and business rules.
- `FruitSA.Infrastructure/`: Data access, migrations, logging, and email services.
- `FruitSA.UnitTest/`: Unit tests for validation and business logic.
- `FruitSA.Web/`: Web interface using ASP.NET Core MVC.

## Support

For issues or questions, please create an issue on the GitHub repository: [https://github.com/ssvincen/FruitSAProductManagement.git](https://github.com/ssvincen/FruitSAProductManagement.git).

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

**Developed for the Fruit South Africa Developer Test (May 2025)**
