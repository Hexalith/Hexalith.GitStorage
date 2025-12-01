# Hexalith.GitStorage.Servers

This project provides shared server utilities and services for the GitStorage bounded context, including integrations with GitHub and Forgejo APIs.

## Overview

The Servers project contains:
- Shared helper classes for server projects
- Common server-side service implementations
- Actor factory registrations
- Server configuration utilities

## Directory Structure

```
Hexalith.GitStorage.Servers/
├── Helpers/
│   └── GitStorageAccountServerHelper.cs
├── Services/
│   └── (shared services)
└── Hexalith.GitStorage.Servers.csproj
```

## Helper Classes

### GitStorageAccountServerHelper

Provides extension methods for server-side service registration:

```csharp
public static class GitStorageAccountServerHelper
{
    /// <summary>
    /// Adds GitStorageAccount projection actor factories to the service collection.
    /// </summary>
    public static IServiceCollection AddGitStorageAccountProjectionActorFactories(
        this IServiceCollection services)
    {
        // Register actor factories for projections
        services.AddSingleton<IProjectionActorFactory<GitStorageAccount>, 
            ProjectionActorFactory<GitStorageAccount>>();
        services.AddSingleton<IProjectionActorFactory<GitStorageAccountSummaryViewModel>, 
            ProjectionActorFactory<GitStorageAccountSummaryViewModel>>();
        services.AddSingleton<IProjectionActorFactory<GitStorageAccountDetailsViewModel>, 
            ProjectionActorFactory<GitStorageAccountDetailsViewModel>>();
            
        return services;
    }
}
```

## Actor Factory Registration

Actor factories are registered for:

| Projection Type | Purpose |
|-----------------|---------|
| `GitStorageAccount` | Aggregate snapshots |
| `GitStorageAccountSummaryViewModel` | List/grid displays |
| `GitStorageAccountDetailsViewModel` | Detail views |

## Usage

### In API Server

```csharp
public static void AddServices(IServiceCollection services, IConfiguration configuration)
{
    // ... other registrations

    // Add projection actor factories
    services.AddGitStorageAccountProjectionActorFactories();
}
```

### In Web Server

```csharp
public static void AddServices(IServiceCollection services, IConfiguration configuration)
{
    // ... other registrations

    // Add projection actor factories (if needed)
    services.AddGitStorageAccountProjectionActorFactories();
}
```

## Dependencies

- `Hexalith.Infrastructure.DaprRuntime` - Dapr integration
- `Hexalith.GitStorage.Projections` - Projection definitions
- `Hexalith.GitStorage.Requests` - View model definitions

## Design Guidelines

This project should contain:
- ✅ Shared server utilities
- ✅ Actor registration helpers
- ✅ Common service implementations
- ❌ Business logic (belongs in Application layer)
- ❌ UI components (belongs in Presentation layer)
- ❌ Domain code (belongs in Domain layer)
