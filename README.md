# Hexalith.GitStorage

A comprehensive module for managing Git storage providers including GitHub and Forgejo. This module enables organizations and repositories to be created, updated, and managed through a unified API following Domain-Driven Design (DDD), CQRS (Command Query Responsibility Segregation), and Event Sourcing architectural patterns.

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

Hexalith.GitStorage provides a unified service layer for managing Git storage providers. It abstracts the differences between GitHub and Forgejo APIs, allowing applications to:

- **Manage Git Storage Accounts**: Configure and manage connections to GitHub and Forgejo instances
- **Manage Organizations**: Create, update, and configure organizations across providers
- **Manage Repositories**: Create, update, configure, and manage repositories with full lifecycle support

The module implements a clean architecture with clear separation of concerns:

- **Domain Layer**: Contains domain aggregates, events, and value objects for Git entities
- **Application Layer**: Contains commands, command handlers, requests, and projections
- **Infrastructure Layer**: Contains API servers, web servers, and provider integrations
- **Presentation Layer**: Contains Blazor UI components and pages

The module follows CQRS and Event Sourcing patterns, using Dapr for distributed application runtime and Azure Cosmos DB for persistence.

### Supported Providers

| Provider | Features | Authentication |
|----------|----------|----------------|
| **GitHub** | Full API support for organizations and repositories | Personal Access Token, OAuth App, GitHub App |
| **Forgejo** | Full API support for organizations and repositories | Personal Access Token, OAuth2 |

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

### Domain Entities

The module manages three main domain entities:

| Entity | Description |
|--------|-------------|
| **GitStorageAccount** | Represents a connection to a Git provider (GitHub or Forgejo) |
| **Organization** | Represents an organization within a Git provider |
| **Repository** | Represents a Git repository within an organization |

### Aggregates

Aggregates are the core domain entities that encapsulate business rules and state changes.

**Location**: `src/libraries/Domain/Hexalith.GitStorage.Aggregates/`

```csharp
/// <summary>
/// Represents a Git storage account aggregate for managing provider connections.
/// </summary>
/// <param name="Id">The account identifier.</param>
/// <param name="Name">The account display name.</param>
/// <param name="ProviderType">The provider type (GitHub or Forgejo).</param>
/// <param name="BaseUrl">The base URL for the provider API.</param>
/// <param name="Comments">Optional description.</param>
/// <param name="Disabled">Whether the account is disabled.</param>
[DataContract]
public sealed record GitStorageAccount(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] GitProviderType ProviderType,
    [property: DataMember(Order = 4)] string BaseUrl,
    [property: DataMember(Order = 5)] string? Comments,
    [property: DataMember(Order = 6)] bool Disabled) : IDomainAggregate
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

**Location**: `src/libraries/Domain/Hexalith.GitStorage.Events/`

#### GitStorageAccount Events

- `GitStorageAccountAdded` - When a new provider connection is created
- `GitStorageAccountDescriptionChanged` - When account details are updated
- `GitStorageAccountDisabled` - When an account is disabled
- `GitStorageAccountEnabled` - When an account is enabled

#### Organization Events

- `OrganizationAdded` - When a new organization is registered
- `OrganizationUpdated` - When organization details are updated
- `OrganizationRemoved` - When an organization is removed

#### Repository Events

- `RepositoryCreated` - When a new repository is created
- `RepositoryUpdated` - When repository settings are changed
- `RepositoryArchived` - When a repository is archived
- `RepositoryDeleted` - When a repository is deleted

```csharp
/// <summary>
/// Event raised when a new Git storage account is added.
/// </summary>
[PolymorphicSerialization]
public partial record GitStorageAccountAdded(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] GitProviderType ProviderType,
    [property: DataMember(Order = 4)] string BaseUrl,
    [property: DataMember(Order = 5)] string? Comments)
    : GitStorageAccountEvent(Id);
```

### Value Objects

Value objects are immutable domain concepts with no identity.

**Location**: `src/libraries/Domain/Hexalith.GitStorage.Aggregates.Abstractions/ValueObjects/`

- `GitProviderType` - Enum for provider types (GitHub, Forgejo)
- `RepositoryVisibility` - Enum for repository visibility (Public, Private)
- `OrganizationSettings` - Configuration for organizations

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

**Location**: `src/libraries/Application/Hexalith.GitStorage.Commands/`

#### GitStorageAccount Commands

- `AddGitStorageAccount` - Create a new provider connection
- `ChangeGitStorageAccountDescription` - Update account details
- `DisableGitStorageAccount` - Disable an account
- `EnableGitStorageAccount` - Enable an account

#### Organization Commands

- `CreateOrganization` - Create a new organization
- `UpdateOrganization` - Update organization settings
- `DeleteOrganization` - Remove an organization

#### Repository Commands

- `CreateRepository` - Create a new repository
- `UpdateRepository` - Update repository settings
- `ArchiveRepository` - Archive a repository
- `DeleteRepository` - Delete a repository

```csharp
/// <summary>
/// Command to add a new Git storage account.
/// </summary>
[PolymorphicSerialization]
public partial record AddGitStorageAccount(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] GitProviderType ProviderType,
    [property: DataMember(Order = 4)] string BaseUrl,
    [property: DataMember(Order = 5)] string? Comments)
    : GitStorageAccountCommand(Id);
```

### Requests (Queries)

Requests represent queries for data retrieval.

**Location**: `src/libraries/Application/Hexalith.GitStorage.Requests/`

#### GitStorageAccount Requests

- `GetGitStorageAccountDetails` - Get full details of an account
- `GetGitStorageAccountSummaries` - Get list of account summaries
- `GetGitStorageAccountIds` - Get list of all account IDs

#### Organization Requests

- `GetOrganizations` - List organizations for an account
- `GetOrganizationDetails` - Get organization details

#### Repository Requests

- `GetRepositories` - List repositories for an organization
- `GetRepositoryDetails` - Get repository details

### View Models

View models for presenting data to the UI.

- `GitStorageAccountDetailsViewModel` - Full account details
- `GitStorageAccountSummaryViewModel` - Account summary for lists
- `OrganizationViewModel` - Organization details
- `RepositoryViewModel` - Repository details

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

- REST API controllers for Git storage operations
- Dapr actor registrations for event sourcing
- GitHub and Forgejo API integrations
- Service registrations and configuration

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

- `GitStorageAccountIdField.razor` - Account ID input field
- `GitStorageAccountSummaryGrid.razor` - Data grid for accounts
- `OrganizationSelector.razor` - Organization selection component
- `RepositoryList.razor` - Repository listing component
- `ProviderTypeSelector.razor` - GitHub/Forgejo provider selector

### UI Pages

**Location**: `src/libraries/Presentation/Hexalith.GitStorage.UI.Pages/`

Blazor pages:

- `Home.razor` - Module home page
- `GitStorageAccountIndex.razor` - Account list page
- `GitStorageAccountDetails.razor` - Account add/edit page
- `OrganizationIndex.razor` - Organization list page
- `RepositoryIndex.razor` - Repository list page

```razor
@page "/GitStorage/Accounts"
@rendermode InteractiveAuto

<HexEntityIndexPage
    OnLoadData="LoadSummaries"
    OnImport="ImportAsync"
    OnExport="ExportAsync"
    AddPagePath="/GitStorage/Add/Account"
    Title="@Labels.ListTitle">
    <GitStorageAccountSummaryGrid Items="_summariesQuery"
        EntityDetailsPath="/GitStorage/Account"
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
    public GitProviderType ProviderType { get; set; }
    public string BaseUrl { get; set; }
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
        var added = new GitStorageAccountAdded(
            "github-main",
            "GitHub Main",
            GitProviderType.GitHub,
            "https://api.github.com",
            "Primary GitHub account");

        // Act
        var result = aggregate.Apply(added);

        // Assert
        result.Succeeded.ShouldBeTrue();
        var newAggregate = result.Aggregate as GitStorageAccount;
        newAggregate.ShouldNotBeNull();
        newAggregate.Id.ShouldBe("github-main");
        newAggregate.ProviderType.ShouldBe(GitProviderType.GitHub);
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

### Application Settings

```json
{
  "GitStorage": {
    "GitHub": {
      "BaseUrl": "https://api.github.com",
      "Token": "your-github-token"
    },
    "Forgejo": {
      "BaseUrl": "https://your-forgejo-instance.com/api/v1",
      "Token": "your-forgejo-token"
    }
  }
}
```

### User Secrets

For local development, use user secrets to store sensitive configuration:

```bash
cd AspireHost
dotnet user-secrets set "GitStorage:GitHub:Token" "your-github-token"
dotnet user-secrets set "GitStorage:Forgejo:Url" "https://your-forgejo-instance.com"
dotnet user-secrets set "GitStorage:Forgejo:Token" "your-forgejo-token"
```

### Central Package Management

Package versions are managed centrally in `Directory.Packages.props`.

## Running with .NET Aspire

### Start the Application

```bash
cd AspireHost
dotnet run
```

### Access the Dashboard

Open the Aspire dashboard URL shown in the console (typically `https://localhost:17225`).

### Access the Application

- Web Server: `https://localhost:5001`
- API Server: `https://localhost:5002`

## Development Workflow

### Adding a New Provider

1. Implement the provider adapter in Infrastructure layer
1. Add provider-specific configuration
1. Register the provider in service collection
1. Add UI components for provider-specific features

### Adding a New Entity

1. Create the aggregate record
1. Define domain events
1. Create commands for each operation
1. Add request definitions and view models
1. Create projection handlers
1. Create UI components and pages
1. Add localization resources

## Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for detailed guidelines.

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
