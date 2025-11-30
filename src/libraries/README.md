# Libraries (NuGet Packages)

This directory contains all the library projects that are packaged and distributed as NuGet packages.

## Architecture Overview

The libraries follow a clean architecture approach with four main layers:

```
┌─────────────────────────────────────────────────────────┐
│                   Presentation                          │
│  (Hexalith.MyNewModule.UI.Components)                 │
│  (Hexalith.MyNewModule.UI.Pages)                      │
└─────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────┐
│                   Infrastructure                        │
│  (Hexalith.MyNewModule.ApiServer)                     │
│  (Hexalith.MyNewModule.WebServer)                     │
│  (Hexalith.MyNewModule.WebApp)                        │
│  (Hexalith.MyNewModule.Servers)                       │
└─────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────┐
│                    Application                          │
│  (Hexalith.MyNewModule)                               │
│  (Hexalith.MyNewModule.Abstractions)                  │
│  (Hexalith.MyNewModule.Commands)                      │
│  (Hexalith.MyNewModule.Requests)                      │
│  (Hexalith.MyNewModule.Projections)                   │
└─────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────┐
│                      Domain                             │
│  (Hexalith.MyNewModule.Aggregates)                    │
│  (Hexalith.MyNewModule.Aggregates.Abstractions)       │
│  (Hexalith.MyNewModule.Events)                        │
│  (Hexalith.MyNewModule.Localizations)                 │
└─────────────────────────────────────────────────────────┘
```

## Layer Descriptions

### Domain Layer (`Domain/`)

The domain layer contains the core business logic and is completely independent of any external framework.

| Project | Description |
|---------|-------------|
| `Hexalith.MyNewModule.Aggregates` | Domain aggregate implementations with event sourcing |
| `Hexalith.MyNewModule.Aggregates.Abstractions` | Domain helpers, enums, and value objects |
| `Hexalith.MyNewModule.Events` | Domain event definitions |
| `Hexalith.MyNewModule.Localizations` | Localization resource files |

### Application Layer (`Application/`)

The application layer implements use cases and orchestrates domain operations.

| Project | Description |
|---------|-------------|
| `Hexalith.MyNewModule` | Main application logic, command handlers, event handlers |
| `Hexalith.MyNewModule.Abstractions` | Module interfaces, policies, roles, service contracts |
| `Hexalith.MyNewModule.Commands` | Command definitions for write operations |
| `Hexalith.MyNewModule.Requests` | Query/request definitions and view models |
| `Hexalith.MyNewModule.Projections` | Projection handlers for updating read models |

### Infrastructure Layer (`Infrastructure/`)

The infrastructure layer provides technical implementations for hosting and integration.

| Project | Description |
|---------|-------------|
| `Hexalith.MyNewModule.ApiServer` | REST API controllers, Dapr actor registration |
| `Hexalith.MyNewModule.WebServer` | Server-side Blazor hosting |
| `Hexalith.MyNewModule.WebApp` | WebAssembly client module |
| `Hexalith.MyNewModule.Servers` | Shared server utilities |

### Presentation Layer (`Presentation/`)

The presentation layer contains all UI-related code.

| Project | Description |
|---------|-------------|
| `Hexalith.MyNewModule.UI.Components` | Reusable Blazor components |
| `Hexalith.MyNewModule.UI.Pages` | Blazor pages and view models |

## Package Dependencies

### Domain Layer Dependencies

```
Hexalith.MyNewModule.Aggregates
├── Hexalith.MyNewModule.Aggregates.Abstractions
└── Hexalith.MyNewModule.Events

Hexalith.MyNewModule.Events
└── Hexalith.MyNewModule.Aggregates.Abstractions

Hexalith.MyNewModule.Localizations
└── (no internal dependencies)
```

### Application Layer Dependencies

```
Hexalith.MyNewModule
├── Hexalith.MyNewModule.Abstractions
├── Hexalith.MyNewModule.Aggregates
├── Hexalith.MyNewModule.Commands
├── Hexalith.MyNewModule.Events
├── Hexalith.MyNewModule.Projections
└── Hexalith.MyNewModule.Requests

Hexalith.MyNewModule.Commands
├── Hexalith.MyNewModule.Aggregates.Abstractions
└── Hexalith.MyNewModule.Abstractions

Hexalith.MyNewModule.Requests
├── Hexalith.MyNewModule.Aggregates.Abstractions
└── Hexalith.MyNewModule.Abstractions
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
