# Hexalith.MyNewModule

A comprehensive template repository for creating new Hexalith modules following Domain-Driven Design (DDD), CQRS (Command Query Responsibility Segregation), and Event Sourcing architectural patterns.

## Build Status

[![License: MIT](https://img.shields.io/github/license/hexalith/hexalith.MyNewModule)](https://github.com/hexalith/hexalith/blob/main/LICENSE)
[![Discord](https://img.shields.io/discord/1063152441819942922?label=Discord&logo=discord&logoColor=white&color=d82679)](https://discordapp.com/channels/1102166958918610994/1102166958918610997)

[![Coverity Scan Build Status](https://scan.coverity.com/projects/31529/badge.svg)](https://scan.coverity.com/projects/hexalith-hexalith-MyNewModule)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/d48f6d9ab9fb4776b6b4711fc556d1c4)](https://app.codacy.com/gh/Hexalith/Hexalith.MyNewModule/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.MyNewModule&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.MyNewModule)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.MyNewModule&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.MyNewModule)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.MyNewModule&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.MyNewModule)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.MyNewModule&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.MyNewModule)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.MyNewModule&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.MyNewModule)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.MyNewModule&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.MyNewModule)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.MyNewModule&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.MyNewModule)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.MyNewModule&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.MyNewModule)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.MyNewModule&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.MyNewModule)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.MyNewModule&metric=bugs)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.MyNewModule)

[![Build status](https://github.com/Hexalith/Hexalith.MyNewModule/actions/workflows/build-release.yml/badge.svg)](https://github.com/Hexalith/Hexalith.MyNewModule/actions)
[![NuGet](https://img.shields.io/nuget/v/Hexalith.MyNewModule.svg)](https://www.nuget.org/packages/Hexalith.MyNewModule)
[![Latest](https://img.shields.io/github/v/release/Hexalith/Hexalith.MyNewModule?include_prereleases&label=latest)](https://github.com/Hexalith/Hexalith.MyNewModule/pkgs/nuget/Hexalith.MyNewModule)

## Table of Contents

- [Overview](#overview)
- [Architecture](#architecture)
- [Prerequisites](#prerequisites)
- [Getting Started](#getting-started)
- [Project Structure](#project-structure)
- [Domain Layer](#domain-layer)
- [Application Layer](#application-layer)
- [Infrastructure Layer](#infrastructure-layer)
- [Presentation Layer](#presentation-layer)
- [Testing](#testing)
- [Configuration](#configuration)
- [Running with .NET Aspire](#running-with-net-aspire)
- [Development Workflow](#development-workflow)
- [Contributing](#contributing)
- [License](#license)

## Overview

This repository provides a production-ready template for creating new Hexalith modules. It implements a clean architecture with clear separation of concerns across multiple layers:

- **Domain Layer**: Contains domain aggregates, events, and value objects
- **Application Layer**: Contains commands, command handlers, requests, and projections
- **Infrastructure Layer**: Contains API servers, web servers, and integration services
- **Presentation Layer**: Contains Blazor UI components and pages

The module follows CQRS and Event Sourcing patterns, using Dapr for distributed application runtime and Azure Cosmos DB for persistence.

## Architecture

### Architectural Patterns

```
┌─────────────────────────────────────────────────────────────────┐
│                     Presentation Layer                          │
│  ┌─────────────────────┐  ┌──────────────────────────────────┐ │
│  │   UI.Components     │  │         UI.Pages                 │ │
│  │  (Blazor Components)│  │    (Blazor Pages & Views)        │ │
│  └─────────────────────┘  └──────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                    Infrastructure Layer                         │
│  ┌─────────────────┐ ┌──────────────┐ ┌─────────────────────┐  │
│  │   ApiServer     │ │  WebServer   │ │      WebApp         │  │
│  │  (REST API)     │ │ (SSR Host)   │ │  (WASM Client)      │  │
│  └─────────────────┘ └──────────────┘ └─────────────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                     Application Layer                           │
│  ┌────────────┐ ┌───────────┐ ┌───────────┐ ┌───────────────┐  │
│  │  Commands  │ │ Requests  │ │Projections│ │   Handlers    │  │
│  │            │ │ (Queries) │ │           │ │               │  │
│  └────────────┘ └───────────┘ └───────────┘ └───────────────┘  │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│                       Domain Layer                              │
│  ┌────────────┐ ┌───────────┐ ┌───────────┐ ┌───────────────┐  │
│  │ Aggregates │ │  Events   │ │  Value    │ │ Localizations │  │
│  │            │ │           │ │  Objects  │ │               │  │
│  └────────────┘ └───────────┘ └───────────┘ └───────────────┘  │
└─────────────────────────────────────────────────────────────────┘
```

### Key Design Principles

1. **Domain-Driven Design (DDD)**: Domain logic is encapsulated in aggregates with clear boundaries
2. **CQRS**: Commands (writes) and Queries (reads) are separated into different models
3. **Event Sourcing**: State changes are captured as a sequence of events
4. **Clean Architecture**: Dependencies flow inward, with the domain layer at the core
5. **Modular Design**: Each module is self-contained and can be deployed independently

## Prerequisites

Before getting started, ensure you have the following installed:

- [.NET 9 SDK](https://dotnet.microsoft.com/download) or later (currently targeting .NET 10.0)
- [PowerShell 7](https://github.com/PowerShell/PowerShell) or later
- [Git](https://git-scm.com/)
- [Docker](https://www.docker.com/) (for running with Aspire)
- [Dapr CLI](https://docs.dapr.io/getting-started/install-dapr-cli/) (optional, for local development)

### Optional Tools

- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)
- [Cursor](https://cursor.sh/) (recommended for AI-assisted development)
- [Azure CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli) (for Azure deployments)

## Getting Started

### 1. Clone or Use as Template

**Option A: Use as GitHub Template**

1. Click "Use this template" on GitHub
2. Create a new repository with your desired name

**Option B: Clone the Repository**

```bash
git clone https://github.com/Hexalith/Hexalith.MyNewModule.git YourModuleName
cd YourModuleName
```

### 2. Initialize Your Package

Run the initialization script to customize the template for your module:

```powershell
./initialize.ps1 -PackageName "YourPackageName"
```

**Example:**

```powershell
./initialize.ps1 -PackageName "Inventory"
```

This creates a module named `Hexalith.Inventory`.

The initialization script will:
- Replace all occurrences of `MyToDo` with your package name
- Rename directories and files containing `MyToDo`
- Initialize and update Git submodules (Hexalith.Builds and HexalithApp)
- Set up the project structure for your new module

### 3. Initialize Git Submodules

The template uses Git submodules for shared build configurations:

```powershell
git submodule init
git submodule update
```

### 4. Build the Solution

```bash
dotnet restore
dotnet build
```

### 5. Run Tests

```bash
dotnet test
```

## Project Structure

```
Hexalith.MyNewModule/
├── AspireHost/                          # .NET Aspire orchestration
│   ├── Program.cs                       # Aspire app configuration
│   ├── Components/                      # Aspire component configurations
│   └── appsettings.json                # Application settings
│
├── src/                                 # Source code root
│   ├── HexalithMyNewModuleApiServerApplication.cs
│   ├── HexalithMyNewModuleWebAppApplication.cs
│   ├── HexalithMyNewModuleWebServerApplication.cs
│   │
│   └── libraries/                       # NuGet package projects
│       │
│       ├── Application/                 # Application layer
│       │   ├── Hexalith.MyNewModule/              # Main application logic
│       │   ├── Hexalith.MyNewModule.Abstractions/ # Interfaces & contracts
│       │   ├── Hexalith.MyNewModule.Commands/     # Command definitions
│       │   ├── Hexalith.MyNewModule.Projections/  # Read model projections
│       │   └── Hexalith.MyNewModule.Requests/     # Query requests
│       │
│       ├── Domain/                      # Domain layer
│       │   ├── Hexalith.MyNewModule.Aggregates/             # Domain aggregates
│       │   ├── Hexalith.MyNewModule.Aggregates.Abstractions/# Domain helpers
│       │   ├── Hexalith.MyNewModule.Events/                 # Domain events
│       │   └── Hexalith.MyNewModule.Localizations/          # Resource files
│       │
│       ├── Infrastructure/              # Infrastructure layer
│       │   ├── Hexalith.MyNewModule.ApiServer/   # REST API server
│       │   ├── Hexalith.MyNewModule.Servers/     # Server helpers
│       │   ├── Hexalith.MyNewModule.WebApp/      # WASM client module
│       │   └── Hexalith.MyNewModule.WebServer/   # SSR web server module
│       │
│       └── Presentation/                # Presentation layer
│           ├── Hexalith.MyNewModule.UI.Components/ # Blazor components
│           └── Hexalith.MyNewModule.UI.Pages/      # Blazor pages
│
├── test/                                # Test projects
│   └── Hexalith.MyNewModule.Tests/     # Unit & integration tests
│
├── HexalithApp/                         # Hexalith application (submodule)
├── Hexalith.Builds/                     # Build configurations (submodule)
│
├── Directory.Build.props                # MSBuild properties
├── Directory.Packages.props             # Central package management
├── Hexalith.MyNewModule.sln           # Solution file
└── initialize.ps1                       # Initialization script
```

## Domain Layer

The domain layer contains the core business logic and is framework-agnostic.

### Aggregates

Aggregates are the core domain entities that encapsulate business rules and state changes.

**Location**: `src/libraries/Domain/Hexalith.MyNewModule.Aggregates/`

```csharp
/// <summary>
/// Represents a mytodo aggregate.
/// </summary>
/// <param name="Id">The mytodo identifier.</param>
/// <param name="Name">The mytodo name.</param>
/// <param name="Comments">The mytodo description.</param>
/// <param name="Disabled">The mytodo disabled status.</param>
[DataContract]
public sealed record MyToDo(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments,
    [property: DataMember(Order = 7)] bool Disabled) : IDomainAggregate
{
    public ApplyResult Apply(object domainEvent)
    {
        // Event handling logic
    }
}
```

Key features:
- Implements `IDomainAggregate` interface
- Uses C# records for immutability
- Primary constructors for clean initialization
- `Apply` method handles domain events and returns new state

### Domain Events

Events represent facts that have happened in the domain.

**Location**: `src/libraries/Domain/Hexalith.MyNewModule.Events/MyToDo/`

Available events:
- `MyToDoAdded` - When a new module is created
- `MyToDoDescriptionChanged` - When name or description changes
- `MyToDoDisabled` - When the module is disabled
- `MyToDoEnabled` - When the module is enabled

```csharp
/// <summary>
/// Event raised when a new mytodo is added.
/// </summary>
[PolymorphicSerialization]
public partial record MyToDoAdded(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments)
    : MyToDoEvent(Id);
```

### Value Objects

Value objects are immutable domain concepts with no identity.

**Location**: `src/libraries/Domain/Hexalith.MyNewModule.Aggregates.Abstractions/ValueObjects/`

### Localizations

Resource files for internationalization (i18n) support.

**Location**: `src/libraries/Domain/Hexalith.MyNewModule.Localizations/`

Files:
- `MyToDo.resx` - English (default)
- `MyToDo.fr.resx` - French
- `MyNewModuleMenu.resx` - Menu labels

## Application Layer

The application layer coordinates domain operations and implements use cases.

### Abstractions

**Location**: `src/libraries/Application/Hexalith.MyNewModule.Abstractions/`

Key files:
- `IMyToDoModule.cs` - Module interface
- `IMyToDoService.cs` - Service interface
- `MyToDoPolicies.cs` - Authorization policies
- `MyToDoRoles.cs` - Security roles

```csharp
/// <summary>
/// Defines the roles for MyToDo security.
/// </summary>
public static class MyToDoRoles
{
    public const string Owner = nameof(MyNewModule) + nameof(Owner);
    public const string Contributor = nameof(MyNewModule) + nameof(Contributor);
    public const string Reader = nameof(MyNewModule) + nameof(Reader);
}
```

### Commands

Commands represent intent to change the system state.

**Location**: `src/libraries/Application/Hexalith.MyNewModule.Commands/MyToDo/`

Available commands:
- `AddMyToDo` - Create a new module
- `ChangeMyToDoDescription` - Update name/description
- `DisableMyToDo` - Disable a module
- `EnableMyToDo` - Enable a module

```csharp
/// <summary>
/// Command to add a new mytodo.
/// </summary>
[PolymorphicSerialization]
public partial record AddMyToDo(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments)
    : MyToDoCommand(Id);
```

### Requests (Queries)

Requests represent queries for data retrieval.

**Location**: `src/libraries/Application/Hexalith.MyNewModule.Requests/MyToDo/`

Available requests:
- `GetMyToDoDetails` - Get full details of a module
- `GetMyToDoSummaries` - Get list of module summaries
- `GetMyToDoIds` - Get list of all module IDs
- `GetMyToDoExports` - Export module data

### View Models

View models for presenting data to the UI.

- `MyToDoDetailsViewModel` - Full module details
- `MyToDoSummaryViewModel` - Summary for lists

### Projections

Projections handle event processing to update read models.

**Location**: `src/libraries/Application/Hexalith.MyNewModule.Projections/`

```csharp
public static class MyToDoProjectionHelper
{
    public static IServiceCollection AddMyToDoProjectionHandlers(
        this IServiceCollection services)
        => services
            .AddScoped<IProjectionUpdateHandler<MyToDoAdded>, 
                MyToDoAddedOnSummaryProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<MyToDoDescriptionChanged>, 
                MyToDoDescriptionChangedOnSummaryProjectionHandler>()
            // ... more handlers
}
```

## Infrastructure Layer

The infrastructure layer contains technical implementations and hosting concerns.

### API Server Module

**Location**: `src/libraries/Infrastructure/Hexalith.MyNewModule.ApiServer/`

Provides:
- REST API controllers
- Dapr actor registrations
- Service registrations
- Integration event handling

```csharp
public sealed class HexalithMyNewModuleApiServerModule : 
    IApiServerApplicationModule, IMyToDoModule
{
    public static void AddServices(IServiceCollection services, 
        IConfiguration configuration)
    {
        // Register serialization mappers
        HexalithMyNewModuleEventsSerialization.RegisterPolymorphicMappers();
        HexalithMyNewModuleCommandsSerialization.RegisterPolymorphicMappers();
        
        // Add module services
        services.AddMyToDo();
        services.AddMyToDoProjectionActorFactories();
    }

    public static void RegisterActors(object actorCollection)
    {
        var actorRegistrations = (ActorRegistrationCollection)actorCollection;
        actorRegistrations.RegisterActor<DomainAggregateActor>(
            MyToDoDomainHelper.MyToDoAggregateName.ToAggregateActorName());
        // ... more actor registrations
    }
}
```

### Web Server Module

**Location**: `src/libraries/Infrastructure/Hexalith.MyNewModule.WebServer/`

Provides server-side rendering (SSR) support for Blazor.

### Web App Module

**Location**: `src/libraries/Infrastructure/Hexalith.MyNewModule.WebApp/`

Provides WebAssembly (WASM) client support for Blazor.

## Presentation Layer

The presentation layer contains Blazor UI components and pages.

### UI Components

**Location**: `src/libraries/Presentation/Hexalith.MyNewModule.UI.Components/`

Reusable Blazor components:
- `MyToDoIdField.razor` - ID input field
- `MyToDoSummaryGrid.razor` - Data grid for summaries

### UI Pages

**Location**: `src/libraries/Presentation/Hexalith.MyNewModule.UI.Pages/`

Blazor pages:
- `Home.razor` - Module home page
- `MyToDoIndex.razor` - List/index page
- `MyToDoDetails.razor` - Add/edit page

**Index Page Example:**

```razor
@page "/MyToDo/MyToDo"
@rendermode InteractiveAuto

<HexEntityIndexPage 
    OnLoadData="LoadSummaries"
    OnImport="ImportAsync"
    OnExport="ExportAsync"
    AddPagePath="/MyToDo/Add/MyToDo"
    Title="@Labels.ListTitle">
    <MyToDoSummaryGrid Items="_summariesQuery" 
        EntityDetailsPath="/MyToDo/MyToDo" 
        OnDisabledChanged="OnDisabledChangedAsync" />
</HexEntityIndexPage>
```

### Edit View Model

**Location**: `src/libraries/Presentation/Hexalith.MyNewModule.UI.Pages/MyToDo/`

```csharp
public sealed class MyToDoEditViewModel : IIdDescription, IEntityViewModel
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string? Comments { get; set; }
    public bool Disabled { get; set; }
    public bool HasChanges => /* change detection logic */;

    internal async Task SaveAsync(ClaimsPrincipal user, 
        ICommandService commandService, bool create, 
        CancellationToken cancellationToken)
    {
        // Command submission logic
    }
}
```

## Testing

### Test Project

**Location**: `test/Hexalith.MyNewModule.Tests/`

The project uses:
- **xUnit** - Testing framework
- **Shouldly** - Assertion library
- **Moq** - Mocking framework

### Project Structure

```
test/
└── Hexalith.MyNewModule.Tests/
    ├── Domains/
    │   ├── Aggregates/    # Aggregate tests
    │   ├── Commands/      # Command tests
    │   └── Events/        # Event tests
    └── Hexalith.MyNewModule.Tests.csproj
```

### Writing Tests

```csharp
public class MyToDoAggregateTests
{
    [Fact]
    public void Apply_MyToDoAdded_ShouldInitializeAggregate()
    {
        // Arrange
        var aggregate = new MyToDo();
        var added = new MyToDoAdded("test-id", "Test Name", "Comments");

        // Act
        var result = aggregate.Apply(added);

        // Assert
        result.Succeeded.ShouldBeTrue();
        var newAggregate = result.Aggregate as MyToDo;
        newAggregate.ShouldNotBeNull();
        newAggregate.Id.ShouldBe("test-id");
        newAggregate.Name.ShouldBe("Test Name");
    }
}
```

### Running Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test project
dotnet test test/Hexalith.MyNewModule.Tests/
```

## Configuration

### Central Package Management

Package versions are managed centrally in `Directory.Packages.props`:

```xml
<Project>
  <PropertyGroup>
    <ManagePackageVersionsCentrally>true</ManagePackageVersionsCentrally>
  </PropertyGroup>
  <ItemGroup>
    <PackageVersion Include="Hexalith.Application" Version="1.71.1" />
    <PackageVersion Include="Hexalith.Infrastructure.DaprRuntime" Version="1.71.1" />
    <!-- ... more packages -->
  </ItemGroup>
</Project>
```

### Build Properties

Build configuration is defined in `Directory.Build.props`:

```xml
<Project>
  <PropertyGroup>
    <Product>Hexalith.MyNewModule</Product>
    <RepositoryUrl>https://github.com/Hexalith/Hexalith.MyNewModule.git</RepositoryUrl>
    <PackageProjectUrl>https://github.com/Hexalith/Hexalith.MyNewModule</PackageProjectUrl>
    <PackageTags>hexalith;</PackageTags>
    <Description>Hexalith MyNewModule</Description>
  </PropertyGroup>
</Project>
```

### Application Settings

**API Server** (`HexalithApp/src/HexalithApp.ApiServer/appsettings.json`):
- CosmosDB connection settings
- Dapr configuration
- Logging settings

**Web Server** (`HexalithApp/src/HexalithApp.WebServer/appsettings.json`):
- Identity provider settings
- Email server configuration
- Session settings

## Running with .NET Aspire

### Aspire Host

**Location**: `AspireHost/`

The Aspire host orchestrates all application components:

```csharp
HexalithDistributedApplication app = new(args);

if (app.IsProjectEnabled<Projects.HexalithApp_WebServer>())
{
    app.AddProject<Projects.HexalithApp_WebServer>("MyNewModuleweb")
        .WithEnvironmentFromConfiguration("APP_API_TOKEN")
        .WithEnvironmentFromConfiguration("Hexalith__IdentityStores__Microsoft__Id")
        // ... more configuration
}

if (app.IsProjectEnabled<Projects.HexalithApp_ApiServer>())
{
    app.AddProject<Projects.HexalithApp_ApiServer>("MyNewModuleapi")
        // ... configuration
}

await app.Builder.Build().RunAsync();
```

### Running the Application

1. **Start the Aspire host:**

```bash
cd AspireHost
dotnet run
```

2. **Access the dashboard:**

Open the Aspire dashboard URL shown in the console (typically `https://localhost:17225`).

3. **Access the application:**
- Web Server: `https://localhost:5001`
- API Server: `https://localhost:5002`

### Environment-Specific Configuration

Configuration files are organized by environment in `AspireHost/Components/`:

```
Components/
├── Common/
│   ├── Development/
│   ├── Integration/
│   ├── Production/
│   └── Staging/
├── MyNewModuleApi/
│   └── Development/
└── MyNewModuleWeb/
    └── Development/
```

## Development Workflow

### 1. Create a New Feature

1. **Define domain events** in `Domain/Hexalith.MyNewModule.Events/`
2. **Update aggregate** in `Domain/Hexalith.MyNewModule.Aggregates/`
3. **Create commands** in `Application/Hexalith.MyNewModule.Commands/`
4. **Add request handlers** in `Application/Hexalith.MyNewModule.Projections/`
5. **Create/update UI** in `Presentation/Hexalith.MyNewModule.UI.Pages/`
6. **Write tests** in `test/Hexalith.MyNewModule.Tests/`

### 2. Adding a New Entity

1. Create the aggregate record
2. Define domain events (Added, Updated, Deleted, etc.)
3. Create commands for each operation
4. Add request definitions and view models
5. Create projection handlers
6. Register in the module's `AddServices` method
7. Create UI components and pages
8. Add localization resources

### 3. Code Style

The project uses:
- StyleCop for code analysis
- Global configuration in `Hexalith.globalconfig`
- XML documentation for public APIs

### 4. Branching Strategy

- `main` - Production-ready code
- `develop` - Integration branch
- `feature/*` - Feature branches
- `bugfix/*` - Bug fix branches

## Contributing

### Prerequisites

1. Fork the repository
2. Clone your fork
3. Set up the development environment

### Submitting Changes

1. Create a feature branch
2. Make your changes
3. Write/update tests
4. Ensure all tests pass
5. Submit a pull request

### Code Review Checklist

- [ ] Code follows project conventions
- [ ] Tests are included and passing
- [ ] Documentation is updated
- [ ] No breaking changes (or documented if necessary)
- [ ] XML documentation for public APIs

## Related Repositories

- [Hexalith.Builds](./Hexalith.Builds/README.md) - Shared build configurations
- [HexalithApp](./HexalithApp/README.md) - Base application framework
- [Hexalith](https://github.com/Hexalith/Hexalith) - Core Hexalith libraries

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Support

- **Discord**: [Join our community](https://discordapp.com/channels/1102166958918610994/1102166958918610997)
- **Issues**: [GitHub Issues](https://github.com/Hexalith/Hexalith.MyNewModule/issues)
- **Documentation**: [Wiki](https://github.com/Hexalith/Hexalith.MyNewModule/wiki)
