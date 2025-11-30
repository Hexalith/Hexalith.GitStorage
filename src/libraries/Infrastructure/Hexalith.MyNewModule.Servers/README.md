# Hexalith.MyNewModule.Servers

This project provides shared server utilities and services for the MyToDo bounded context.

## Overview

The Servers project contains:
- Shared helper classes for server projects
- Common server-side service implementations
- Actor factory registrations
- Server configuration utilities

## Directory Structure

```
Hexalith.MyNewModule.Servers/
├── Helpers/
│   └── MyToDoServerHelper.cs
├── Services/
│   └── (shared services)
└── Hexalith.MyNewModule.Servers.csproj
```

## Helper Classes

### MyToDoServerHelper

Provides extension methods for server-side service registration:

```csharp
public static class MyToDoServerHelper
{
    /// <summary>
    /// Adds MyToDo projection actor factories to the service collection.
    /// </summary>
    public static IServiceCollection AddMyToDoProjectionActorFactories(
        this IServiceCollection services)
    {
        // Register actor factories for projections
        services.AddSingleton<IProjectionActorFactory<MyToDo>, 
            ProjectionActorFactory<MyToDo>>();
        services.AddSingleton<IProjectionActorFactory<MyToDoSummaryViewModel>, 
            ProjectionActorFactory<MyToDoSummaryViewModel>>();
        services.AddSingleton<IProjectionActorFactory<MyToDoDetailsViewModel>, 
            ProjectionActorFactory<MyToDoDetailsViewModel>>();
            
        return services;
    }
}
```

## Actor Factory Registration

Actor factories are registered for:

| Projection Type | Purpose |
|-----------------|---------|
| `MyToDo` | Aggregate snapshots |
| `MyToDoSummaryViewModel` | List/grid displays |
| `MyToDoDetailsViewModel` | Detail views |

## Usage

### In API Server

```csharp
public static void AddServices(IServiceCollection services, IConfiguration configuration)
{
    // ... other registrations

    // Add projection actor factories
    services.AddMyToDoProjectionActorFactories();
}
```

### In Web Server

```csharp
public static void AddServices(IServiceCollection services, IConfiguration configuration)
{
    // ... other registrations

    // Add projection actor factories (if needed)
    services.AddMyToDoProjectionActorFactories();
}
```

## Dependencies

- `Hexalith.Infrastructure.DaprRuntime` - Dapr integration
- `Hexalith.MyNewModule.Projections` - Projection definitions
- `Hexalith.MyNewModule.Requests` - View model definitions

## Design Guidelines

This project should contain:
- ✅ Shared server utilities
- ✅ Actor registration helpers
- ✅ Common service implementations
- ❌ Business logic (belongs in Application layer)
- ❌ UI components (belongs in Presentation layer)
- ❌ Domain code (belongs in Domain layer)
