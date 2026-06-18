# Simple Monolith

A [.NET template](https://learn.microsoft.com/en-us/dotnet/core/tools/custom-templates) that scaffolds a clean architecture monolith with ASP.NET Core MVC + Web API, Entity Framework Core, and thorough testing at every layer.

## Quick Start

```bash
# Install the template
dotnet new install SimpleMonolith.Template

# Create a new project
dotnet new simple-monolith -n YourAppName
```

## Architecture

```
┌──────────────────────────────────────────────────┐
│                  Presentation                     │
│        ASP.NET Core MVC + Web API                 │
│     (Controllers, Views, Models, Filters)         │
├──────────────────────────────────────────────────┤
│                    Domain                          │
│   (Models, Services, Contracts/Business Logic)    │
├──────────────────────────────────────────────────┤
│                     Data                           │
│     (EF Core DbContext, Entities, Repositories)   │
├──────────────────────────────────────────────────┤
│                    Shared                          │
│           (Result<T>, Common Primitives)           │
└──────────────────────────────────────────────────┘
```

**Dependency rules** (enforced by architecture tests):

| Layer | May Reference |
|---|---|
| `Presentation` | Domain |
| `Domain` | Data, Shared |
| `Data` | Shared |
| `Shared` | _(nothing)_ |

## Projects

| Project | Description |
|---|---|
| `MyApp.Presentation` | ASP.NET Core MVC app with Razor views and a REST API (`/api/products`). |
| `MyApp.Domain` | Business logic layer with domain models and service contracts. |
| `MyApp.Data` | EF Core `DbContext`, entities, and repository implementations. |
| `MyApp.Shared` | Shared primitives — the `Result<T>` pattern used across all layers. |

## Testing

| Project | Type | What It Covers |
|---|---|---|
| `MyApp.Domain.UnitTests` | Unit | `ProductService` logic with mocked repository |
| `MyApp.Data.UnitTests` | Unit | `ProductRepository` with EF Core InMemory |
| `MyApp.Presentation.UnitTests` | Unit | `ProductsController` with mocked service |
| `MyApp.IntegrationTests` | Integration | Full API workflow via Testcontainers + WebApplicationFactory |
| `MyApp.ArchitectureTests` | Architecture | Layer dependency rules via NetArchTest |

```bash
# Run all tests
dotnet test
```

**Stack:** xUnit, Moq, Testcontainers (MSSQL), TestStack.BDDfy, NetArchTest, coverlet.

## Tech Stack

- **.NET 10** (C# 12+, primary constructors, records)
- **ASP.NET Core MVC** with Razor views
- **Entity Framework Core 10** (SQL Server)
- **Custom `Result<T>` pattern** (no exceptions for control flow)
- **.slnx** solution format

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Docker](https://www.docker.com/) (integration tests only)

## Building the Template

From the repository root:

```bash
dotnet pack -o ./nupkg
dotnet new install ./nupkg/SimpleMonolith.Template.*.nupkg
```

## License

MIT
