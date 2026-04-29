# RealEstateShop — Full Stack Web Application

A full-stack real estate web application built with .NET N-Tier architecture and Angular frontend.

## Tech Stack

- **Backend:** .NET 10, ASP.NET Core, Entity Framework Core, SQL Server
- **Frontend:** Angular 17+
- **Auth:** JWT Bearer tokens, BCrypt password hashing
- **Architecture:** N-Tier (Domain, Data, BLL, WebApp)

## Projects

| Project | Description |
|---|---|
| RealEstate.Domain | Entity classes |
| RealEstate.Data | EF Core DbContext, Repository, UnitOfWork |
| RealEstate.BLL | Business logic services |
| RealEstateShop.WebApp | ASP.NET Core REST API |
| RealEstate.Tests | All tests (Unit + Integration + E2E) |

## Testing — 168 Tests

| Layer | Tests | Tools |
|---|---|---|
| Unit Tests | 45 | xUnit, Moq, FluentAssertions |
| Integration Tests | 36 | xUnit, WebApplicationFactory, InMemory DB |
| E2E Tests | 87 | Microsoft Playwright |

## Running the Tests

```bash
cd RealEstate.Tests
dotnet test
```

## Coverage Report

Run coverage with:

```bash
dotnet test --collect:"XPlat Code Coverage"
reportgenerator -reports:"TestResults\*\coverage.cobertura.xml" -targetdir:"CoverageReport" -reporttypes:Html
```

Open `CoverageReport/index.html` to view the interactive report.

## Student

- **Name:** Stefan Ristevski
- **Index:** 231086
- **Subject:** Software Quality and Testing
