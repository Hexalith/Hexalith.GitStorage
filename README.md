# Hexalith.GitStorage

A comprehensive template repository for creating new Hexalith modules following Domain-Driven Design (DDD), CQRS (Command Query Responsibility Segregation), and Event Sourcing architectural patterns.

## Build Status

[![License: MIT](https://img.shields.io/github/license/hexalith/hexalith.GitStorage)](https://github.com/hexalith/hexalith/blob/main/LICENSE)
[![Discord](https://img.shields.io/discord/1063152441819942922?label=Discord&logo=discord&logoColor=white&color=d82679)](https://discordapp.com/channels/1102166958918610994/1102166958918610997)

[![Coverity Scan Build Status](https://scan.coverity.com/projects/31529/badge.svg)](https://scan.coverity.com/projects/hexalith-hexalith-GitStorage)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/d48f6d9ab9fb4776b6b4711fc556d1c4)](https://app.codacy.com/gh/Hexalith/Hexalith.GitStorage/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.GitStorage&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.GitStorage)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.GitStorage&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.GitStorage)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.GitStorage&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.GitStorage)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.GitStorage&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.GitStorage)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.GitStorage&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.GitStorage)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.GitStorage&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.GitStorage)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.GitStorage&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.GitStorage)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.GitStorage&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.GitStorage)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.GitStorage&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.GitStorage)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=Hexalith_Hexalith.GitStorage&metric=bugs)](https://sonarcloud.io/summary/new_code?id=Hexalith_Hexalith.GitStorage)

[![Build status](https://github.com/Hexalith/Hexalith.GitStorage/actions/workflows/build-release.yml/badge.svg)](https://github.com/Hexalith/Hexalith.GitStorage/actions)
[![NuGet](https://img.shields.io/nuget/v/Hexalith.GitStorage.svg)](https://www.nuget.org/packages/Hexalith.GitStorage)
[![Latest](https://img.shields.io/github/v/release/Hexalith/Hexalith.GitStorage?include_prereleases&label=latest)](https://github.com/Hexalith/Hexalith.GitStorage/pkgs/nuget/Hexalith.GitStorage)

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
git clone https://github.com/Hexalith/Hexalith.GitStorage.git YourModuleName
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
- Replace all occurrences of `GitStorageAccount` with your package name
- Rename directories and files containing `GitStorageAccount`
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
Hexalith.GitStorage/
├── AspireHost/                          # .NET Aspire orchestration
│   ├── Program.cs                       # Aspire app configuration
│   ├── Components/                      # Aspire component configurations
│   └── appsettings.json                # Application settings
│
├── src/                                 # Source code root
│   ├── HexalithGitStorageApiServerApplication.cs
│   ├── HexalithGitStorageWebAppApplication.cs
│   ├── HexalithGitStorageWebServerApplication.cs
│   │
│   └── libraries/                       # NuGet package projects
│       │
│       ├── Application/                 # Application layer
│       │   ├── Hexalith.GitStorage/              # Main application logic
│       │   ├── Hexalith.GitStorage.Abstractions/ # Interfaces & contracts
│       │   ├── Hexalith.GitStorage.Commands/     # Command definitions
│       │   ├── Hexalith.GitStorage.Projections/  # Read model projections
│       │   └── Hexalith.GitStorage.Requests/     # Query requests
│       │
│       ├── Domain/                      # Domain layer
│       │   ├── Hexalith.GitStorage.Aggregates/             # Domain aggregates
│       │   ├── Hexalith.GitStorage.Aggregates.Abstractions/# Domain helpers
│       │   ├── Hexalith.GitStorage.Events/                 # Domain events
│       │   └── Hexalith.GitStorage.Localizations/          # Resource files
│       │
│       ├── Infrastructure/              # Infrastructure layer
│       │   ├── Hexalith.GitStorage.ApiServer/   # REST API server
│       │   ├── Hexalith.GitStorage.Servers/     # Server helpers
│       │   ├── Hexalith.GitStorage.WebApp/      # WASM client module
│       │   └── Hexalith.GitStorage.WebServer/   # SSR web server module
│       │
│       └── Presentation/                # Presentation layer
│           ├── Hexalith.GitStorage.UI.Components/ # Blazor components
│           └── Hexalith.GitStorage.UI.Pages/      # Blazor pages
│
├── test/                                # Test projects
│   └── Hexalith.GitStorage.Tests/     # Unit & integration tests
│
├── HexalithApp/                         # Hexalith application (submodule)
├── Hexalith.Builds/                     # Build configurations (submodule)
│
├── Directory.Build.props                # MSBuild properties
├── Directory.Packages.props             # Central package management
├── Hexalith.GitStorage.sln           # Solution file
└── initialize.ps1                       # Initialization script
```

## Domain Layer

The domain layer contains the core business logic and is framework-agnostic.

### Aggregates

Aggregates are the core domain entities that encapsulate business rules and state changes.

**Location**: `src/libraries/Domain/Hexalith.GitStorage.Aggregates/`

```csharp
/// <summary>
/// Represents a GitStorageAccount aggregate.
/// </summary>
/// <param name="Id">The GitStorageAccount identifier.</param>
/// <param name="Name">The GitStorageAccount name.</param>
/// <param name="Comments">The GitStorageAccount description.</param>
/// <param name="Disabled">The GitStorageAccount disabled status.</param>
[DataContract]
public sealed record GitStorageAccount(
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

**Location**: `src/libraries/Domain/Hexalith.GitStorage.Events/GitStorageAccount/`

Available events:
- `GitStorageAccountAdded` - When a new module is created
- `GitStorageAccountDescriptionChanged` - When name or description changes
- `GitStorageAccountDisabled` - When the module is disabled
- `GitStorageAccountEnabled` - When the module is enabled

```csharp
/// <summary>
/// Event raised when a new GitStorageAccount is added.
/// </summary>
[PolymorphicSerialization]
public partial record GitStorageAccountAdded(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments)
    : GitStorageAccountEvent(Id);
```

### Value Objects

Value objects are immutable domain concepts with no identity.

**Location**: `src/libraries/Domain/Hexalith.GitStorage.Aggregates.Abstractions/ValueObjects/`

### Localizations

Resource files for internationalization (i18n) support.

**Location**: `src/libraries/Domain/Hexalith.GitStorage.Localizations/`

Files:
- `GitStorageAccount.resx` - English (default)
- `GitStorageAccount.fr.resx` - French
- `GitStorageMenu.resx` - Menu labels

## Application Layer

The application layer coordinates domain operations and implements use cases.

### Abstractions

**Location**: `src/libraries/Application/Hexalith.GitStorage.Abstractions/`

Key files:
- `IGitStorageAccountModule.cs` - Module interface
- `IGitStorageAccountService.cs` - Service interface
- `GitStorageAccountPolicies.cs` - Authorization policies
- `GitStorageAccountRoles.cs` - Security roles

```csharp
/// <summary>
/// Defines the roles for GitStorageAccount security.
/// </summary>
public static class GitStorageAccountRoles
{
    public const string Owner = nameof(GitStorage) + nameof(Owner);
    public const string Contributor = nameof(GitStorage) + nameof(Contributor);
    public const string Reader = nameof(GitStorage) + nameof(Reader);
}
```

### Commands

Commands represent intent to change the system state.

**Location**: `src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/`

Available commands:
- `AddGitStorageAccount` - Create a new module
- `ChangeGitStorageAccountDescription` - Update name/description
- `DisableGitStorageAccount` - Disable a module
- `EnableGitStorageAccount` - Enable a module

```csharp
/// <summary>
/// Command to add a new GitStorageAccount.
/// </summary>
[PolymorphicSerialization]
public partial record AddGitStorageAccount(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments)
    : GitStorageAccountCommand(Id);
```

### Requests (Queries)

Requests represent queries for data retrieval.

**Location**: `src/libraries/Application/Hexalith.GitStorage.Requests/GitStorageAccount/`

Available requests:
- `GetGitStorageAccountDetails` - Get full details of a module
- `GetGitStorageAccountSummaries` - Get list of module summaries
- `GetGitStorageAccountIds` - Get list of all module IDs
- `GetGitStorageAccountExports` - Export module data

### View Models

View models for presenting data to the UI.

- `GitStorageAccountDetailsViewModel` - Full module details
- `GitStorageAccountSummaryViewModel` - Summary for lists

### Projections

Projections handle event processing to update read models.

**Location**: `src/libraries/Application/Hexalith.GitStorage.Projections/`

```csharp
public static class GitStorageAccountProjectionHelper
{
    public static IServiceCollection AddGitStorageAccountProjectionHandlers(
        this IServiceCollection services)
        => services
            .AddScoped<IProjectionUpdateHandler<GitStorageAccountAdded>, 
                GitStorageAccountAddedOnSummaryProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<GitStorageAccountDescriptionChanged>, 
                GitStorageAccountDescriptionChangedOnSummaryProjectionHandler>()
            // ... more handlers
}
```

## Infrastructure Layer

The infrastructure layer contains technical implementations and hosting concerns.

### API Server Module

**Location**: `src/libraries/Infrastructure/Hexalith.GitStorage.ApiServer/`

Provides:
- REST API controllers
- Dapr actor registrations
- Service registrations
- Integration event handling

```csharp
public sealed class HexalithGitStorageApiServerModule : 
    IApiServerApplicationModule, IGitStorageAccountModule
{
    public static void AddServices(IServiceCollection services, 
        IConfiguration configuration)
    {
        // Register serialization mappers
        HexalithGitStorageEventsSerialization.RegisterPolymorphicMappers();
        HexalithGitStorageCommandsSerialization.RegisterPolymorphicMappers();
        
        // Add module services
        services.AddGitStorageAccount();
        services.AddGitStorageAccountProjectionActorFactories();
    }

    public static void RegisterActors(object actorCollection)
    {
        var actorRegistrations = (ActorRegistrationCollection)actorCollection;
        actorRegistrations.RegisterActor<DomainAggregateActor>(
            GitStorageAccountDomainHelper.GitStorageAccountAggregateName.ToAggregateActorName());
        // ... more actor registrations
    }
}
```

### Web Server Module

**Location**: `src/libraries/Infrastructure/Hexalith.GitStorage.WebServer/`

Provides server-side rendering (SSR) support for Blazor.

### Web App Module

**Location**: `src/libraries/Infrastructure/Hexalith.GitStorage.WebApp/`

Provides WebAssembly (WASM) client support for Blazor.

## Presentation Layer

The presentation layer contains Blazor UI components and pages.

### UI Components

**Location**: `src/libraries/Presentation/Hexalith.GitStorage.UI.Components/`

Reusable Blazor components:
- `GitStorageAccountIdField.razor` - ID input field
- `GitStorageAccountSummaryGrid.razor` - Data grid for summaries

### UI Pages

**Location**: `src/libraries/Presentation/Hexalith.GitStorage.UI.Pages/`

Blazor pages:
- `Home.razor` - Module home page
- `GitStorageAccountIndex.razor` - List/index page
- `GitStorageAccountDetails.razor` - Add/edit page

**Index Page Example:**

```razor
@page "/GitStorageAccount/GitStorageAccount"
@rendermode InteractiveAuto

<HexEntityIndexPage 
    OnLoadData="LoadSummaries"
    OnImport="ImportAsync"
    OnExport="ExportAsync"
    AddPagePath="/GitStorageAccount/Add/GitStorageAccount"
    Title="@Labels.ListTitle">
    <GitStorageAccountSummaryGrid Items="_summariesQuery" 
        EntityDetailsPath="/GitStorageAccount/GitStorageAccount" 
        OnDisabledChanged="OnDisabledChangedAsync" />
</HexEntityIndexPage>
```

### Edit View Model

**Location**: `src/libraries/Presentation/Hexalith.GitStorage.UI.Pages/GitStorageAccount/`

```csharp
public sealed class GitStorageAccountEditViewModel : IIdDescription, IEntityViewModel
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

**Location**: `test/Hexalith.GitStorage.Tests/`

The project uses:
- **xUnit** - Testing framework
- **Shouldly** - Assertion library
- **Moq** - Mocking framework

### Project Structure

```
test/
└── Hexalith.GitStorage.Tests/
    ├── Domains/
    │   ├── Aggregates/    # Aggregate tests
    │   ├── Commands/      # Command tests
    │   └── Events/        # Event tests
    └── Hexalith.GitStorage.Tests.csproj
```

### Writing Tests

```csharp
public class GitStorageAccountAggregateTests
{
    [Fact]
    public void Apply_GitStorageAccountAdded_ShouldInitializeAggregate()
    {
        // Arrange
        var aggregate = new GitStorageAccount();
        var added = new GitStorageAccountAdded("test-id", "Test Name", "Comments");

        // Act
        var result = aggregate.Apply(added);

        // Assert
        result.Succeeded.ShouldBeTrue();
        var newAggregate = result.Aggregate as GitStorageAccount;
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
dotnet test test/Hexalith.GitStorage.Tests/
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
    <Product>Hexalith.GitStorage</Product>
    <RepositoryUrl>https://github.com/Hexalith/Hexalith.GitStorage.git</RepositoryUrl>
    <PackageProjectUrl>https://github.com/Hexalith/Hexalith.GitStorage</PackageProjectUrl>
    <PackageTags>hexalith;</PackageTags>
    <Description>Hexalith GitStorage</Description>
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
    app.AddProject<Projects.HexalithApp_WebServer>("GitStorageweb")
        .WithEnvironmentFromConfiguration("APP_API_TOKEN")
        .WithEnvironmentFromConfiguration("Hexalith__IdentityStores__Microsoft__Id")
        // ... more configuration
}

if (app.IsProjectEnabled<Projects.HexalithApp_ApiServer>())
{
    app.AddProject<Projects.HexalithApp_ApiServer>("GitStorageapi")
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
├── GitStorageApi/
│   └── Development/
└── GitStorageWeb/
    └── Development/
```

## Development Workflow

### 1. Create a New Feature

1. **Define domain events** in `Domain/Hexalith.GitStorage.Events/`
2. **Update aggregate** in `Domain/Hexalith.GitStorage.Aggregates/`
3. **Create commands** in `Application/Hexalith.GitStorage.Commands/`
4. **Add request handlers** in `Application/Hexalith.GitStorage.Projections/`
5. **Create/update UI** in `Presentation/Hexalith.GitStorage.UI.Pages/`
6. **Write tests** in `test/Hexalith.GitStorage.Tests/`

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
- **Issues**: [GitHub Issues](https://github.com/Hexalith/Hexalith.GitStorage/issues)
- **Documentation**: [Wiki](https://github.com/Hexalith/Hexalith.GitStorage/wiki)
