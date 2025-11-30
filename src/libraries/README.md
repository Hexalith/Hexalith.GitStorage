# Libraries (NuGet Packages)

This directory contains all the library projects that are packaged and distributed as NuGet packages.

## Architecture Overview

The libraries follow a clean architecture approach with four main layers:

```
┌─────────────────────────────────────────────────────────┐
│                   Presentation                          │
│  (Hexalith.GitStorage.UI.Components)                 │
│  (Hexalith.GitStorage.UI.Pages)                      │
└─────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────┐
│                   Infrastructure                        │
│  (Hexalith.GitStorage.ApiServer)                     │
│  (Hexalith.GitStorage.WebServer)                     │
│  (Hexalith.GitStorage.WebApp)                        │
│  (Hexalith.GitStorage.Servers)                       │
└─────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────┐
│                    Application                          │
│  (Hexalith.GitStorage)                               │
│  (Hexalith.GitStorage.Abstractions)                  │
│  (Hexalith.GitStorage.Commands)                      │
│  (Hexalith.GitStorage.Requests)                      │
│  (Hexalith.GitStorage.Projections)                   │
└─────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────┐
│                      Domain                             │
│  (Hexalith.GitStorage.Aggregates)                    │
│  (Hexalith.GitStorage.Aggregates.Abstractions)       │
│  (Hexalith.GitStorage.Events)                        │
│  (Hexalith.GitStorage.Localizations)                 │
└─────────────────────────────────────────────────────────┘
```

## Layer Descriptions

### Domain Layer (`Domain/`)

The domain layer contains the core business logic and is completely independent of any external framework.

| Project | Description |
|---------|-------------|
| `Hexalith.GitStorage.Aggregates` | Domain aggregate implementations with event sourcing |
| `Hexalith.GitStorage.Aggregates.Abstractions` | Domain helpers, enums, and value objects |
| `Hexalith.GitStorage.Events` | Domain event definitions |
| `Hexalith.GitStorage.Localizations` | Localization resource files |

### Application Layer (`Application/`)

The application layer implements use cases and orchestrates domain operations.

| Project | Description |
|---------|-------------|
| `Hexalith.GitStorage` | Main application logic, command handlers, event handlers |
| `Hexalith.GitStorage.Abstractions` | Module interfaces, policies, roles, service contracts |
| `Hexalith.GitStorage.Commands` | Command definitions for write operations |
| `Hexalith.GitStorage.Requests` | Query/request definitions and view models |
| `Hexalith.GitStorage.Projections` | Projection handlers for updating read models |

### Infrastructure Layer (`Infrastructure/`)

The infrastructure layer provides technical implementations for hosting and integration.

| Project | Description |
|---------|-------------|
| `Hexalith.GitStorage.ApiServer` | REST API controllers, Dapr actor registration |
| `Hexalith.GitStorage.WebServer` | Server-side Blazor hosting |
| `Hexalith.GitStorage.WebApp` | WebAssembly client module |
| `Hexalith.GitStorage.Servers` | Shared server utilities |

### Presentation Layer (`Presentation/`)

The presentation layer contains all UI-related code.

| Project | Description |
|---------|-------------|
| `Hexalith.GitStorage.UI.Components` | Reusable Blazor components |
| `Hexalith.GitStorage.UI.Pages` | Blazor pages and view models |

## Package Dependencies

### Domain Layer Dependencies

```
Hexalith.GitStorage.Aggregates
├── Hexalith.GitStorage.Aggregates.Abstractions
└── Hexalith.GitStorage.Events

Hexalith.GitStorage.Events
└── Hexalith.GitStorage.Aggregates.Abstractions

Hexalith.GitStorage.Localizations
└── (no internal dependencies)
```

### Application Layer Dependencies

```
Hexalith.GitStorage
├── Hexalith.GitStorage.Abstractions
├── Hexalith.GitStorage.Aggregates
├── Hexalith.GitStorage.Commands
├── Hexalith.GitStorage.Events
├── Hexalith.GitStorage.Projections
└── Hexalith.GitStorage.Requests

Hexalith.GitStorage.Commands
├── Hexalith.GitStorage.Aggregates.Abstractions
└── Hexalith.GitStorage.Abstractions

Hexalith.GitStorage.Requests
├── Hexalith.GitStorage.Aggregates.Abstractions
└── Hexalith.GitStorage.Abstractions
```

## Building Packages

### Build All Libraries

```bash
cd src/libraries
dotnet build
```

### Create NuGet Packages

```bash
dotnet pack --configuration Release
```

### Local Development

During local development, project references are used instead of package references. This is controlled by the `UseProjectReference` property in `Directory.Build.props`.

## Adding New Libraries

1. Create a new folder in the appropriate layer directory
2. Add a `.csproj` file following the naming convention
3. Add to the solution file
4. Set up project references following the dependency graph
5. Create a README.md for the new project

### Project Template

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
  </PropertyGroup>
  <ItemGroup>
    <!-- Add package references -->
  </ItemGroup>
  <ItemGroup>
    <!-- Add project references -->
  </ItemGroup>
</Project>
```

## Code Organization Guidelines

1. **One concept per file**: Each class, record, or interface should be in its own file
2. **Consistent naming**: File names should match the type name they contain
3. **Folder structure**: Organize by feature, not by type
4. **README per project**: Each project should have its own README.md
