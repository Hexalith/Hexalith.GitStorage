# Hexalith.GitStorage.Projections

This project contains the projection handlers and request handlers for maintaining read models in the GitStorage bounded context, supporting Git storage accounts, organizations, and repositories.

## Overview

Projections are responsible for:
1. Processing domain events to update read models
2. Maintaining denormalized views optimized for queries
3. Handling request queries against the read models

## Architecture

```
┌───────────────┐    ┌────────────────────┐    ┌──────────────┐
│ Domain Events │───▶│ Projection Handler │───▶│  Read Model  │
│               │    │                    │    │  (Database)  │
└───────────────┘    └────────────────────┘    └──────────────┘
                                                      │
                                                      ▼
┌───────────────┐    ┌────────────────────┐    ┌──────────────┐
│   Requests    │───▶│  Request Handler   │◀──│  View Model  │
│   (Queries)   │    │                    │    │   (Result)   │
└───────────────┘    └────────────────────┘    └──────────────┘
```

## Projection Handlers

### Directory Structure

```
ProjectionHandlers/
├── Details/
│   ├── GitStorageAccountAddedOnDetailsProjectionHandler.cs
│   ├── GitStorageAccountDescriptionChangedOnDetailsProjectionHandler.cs
│   ├── GitStorageAccountDisabledOnDetailsProjectionHandler.cs
│   └── GitStorageAccountEnabledOnDetailsProjectionHandler.cs
│
└── Summaries/
    ├── GitStorageAccountAddedOnSummaryProjectionHandler.cs
    ├── GitStorageAccountDescriptionChangedOnSummaryProjectionHandler.cs
    ├── GitStorageAccountDisabledOnSummaryProjectionHandler.cs
    ├── GitStorageAccountEnabledOnSummaryProjectionHandler.cs
    └── GitStorageAccountSnapshotOnSummaryProjectionHandler.cs
```

### Registration

```csharp
public static IServiceCollection AddGitStorageAccountProjectionHandlers(this IServiceCollection services)
    => services
        // Collection projections
        .AddScoped<IProjectionUpdateHandler<GitStorageAccountAdded>, 
            IdsCollectionProjectionHandler<GitStorageAccountAdded>>()
        
        // Summary projections
        .AddScoped<IProjectionUpdateHandler<GitStorageAccountAdded>, 
            GitStorageAccountAddedOnSummaryProjectionHandler>()
        .AddScoped<IProjectionUpdateHandler<GitStorageAccountDescriptionChanged>, 
            GitStorageAccountDescriptionChangedOnSummaryProjectionHandler>()
        .AddScoped<IProjectionUpdateHandler<GitStorageAccountDisabled>, 
            GitStorageAccountDisabledOnSummaryProjectionHandler>()
        .AddScoped<IProjectionUpdateHandler<GitStorageAccountEnabled>, 
            GitStorageAccountEnabledOnSummaryProjectionHandler>()
        .AddScoped<IProjectionUpdateHandler<SnapshotEvent>, 
            GitStorageAccountSnapshotOnSummaryProjectionHandler>()
        
        // Details projections
        .AddScoped<IProjectionUpdateHandler<GitStorageAccountAdded>, 
            GitStorageAccountAddedOnDetailsProjectionHandler>()
        // ... more handlers
```

### Handler Example

```csharp
public class GitStorageAccountAddedOnSummaryProjectionHandler : 
    IProjectionUpdateHandler<GitStorageAccountAdded>
{
    public async Task<IEnumerable<object>> HandleAsync(
        GitStorageAccountAdded @event, 
        CancellationToken cancellationToken)
    {
        var summary = new GitStorageAccountSummaryViewModel(
            @event.Id,
            @event.Name,
            false);
            
        // Return projection updates
        return [new AddProjection<GitStorageAccountSummaryViewModel>(summary)];
    }
}
```

## Request Handlers

### Available Handlers

| Handler | Request | Description |
|---------|---------|-------------|
| `GetGitStorageAccountDetailsHandler` | `GetGitStorageAccountDetails` | Retrieves full module details |
| `GetFilteredCollectionHandler` | `GetGitStorageAccountSummaries` | Retrieves paginated summaries |
| `GetAggregateIdsRequestHandler` | `GetGitStorageAccountIds` | Retrieves all module IDs |
| `GetExportsRequestHandler` | `GetGitStorageAccountExports` | Exports full aggregate data |

### Request Handler Example

```csharp
public class GetGitStorageAccountDetailsHandler : IRequestHandler<GetGitStorageAccountDetails>
{
    private readonly IProjectionStore<GitStorageAccountDetailsViewModel> _store;

    public GetGitStorageAccountDetailsHandler(
        IProjectionStore<GitStorageAccountDetailsViewModel> store)
    {
        _store = store;
    }

    public async Task<RequestResult<GitStorageAccountDetailsViewModel>> HandleAsync(
        GetGitStorageAccountDetails request, 
        CancellationToken cancellationToken)
    {
        var details = await _store.GetAsync(request.Id, cancellationToken);
        return details is null 
            ? RequestResult<GitStorageAccountDetailsViewModel>.NotFound(request.Id)
            : RequestResult<GitStorageAccountDetailsViewModel>.Success(details);
    }
}
```

## Helper Classes

### GitStorageAccountProjectionHelper

Provides extension methods for service registration:

```csharp
public static class GitStorageAccountProjectionHelper
{
    // Register aggregate providers
    public static IServiceCollection AddGitStorageAccountAggregateProviders(
        this IServiceCollection services);
    
    // Register projection handlers
    public static IServiceCollection AddGitStorageAccountProjectionHandlers(
        this IServiceCollection services);
    
    // Register projections and request handlers
    public static IServiceCollection AddGitStorageAccountProjections(
        this IServiceCollection services);
    
    // Register query services
    public static IServiceCollection AddGitStorageAccountQueryServices(
        this IServiceCollection services);
    
    // Register request handlers
    public static IServiceCollection AddGitStorageAccountRequestHandlers(
        this IServiceCollection services);
}
```

## Services

### GitStorageQuickStartData

Provides sample data for development and demos:

```csharp
public static class GitStorageQuickStartData
{
    public static IEnumerable<object> Data => [
        new AddGitStorageAccount("sample-001", "Sample Module 1", "First sample"),
        new AddGitStorageAccount("sample-002", "Sample Module 2", "Second sample")
    ];
}
```

## Dependencies

- `Hexalith.Application.Projections` - Projection abstractions
- `Hexalith.Application.Requests` - Request handling
- `Hexalith.GitStorage.Events` - Domain events
- `Hexalith.GitStorage.Requests` - Request/view model definitions

## Dapr Actor Integration

Projections are executed via Dapr actors for:
- Reliable event processing
- State persistence
- Scalability

Actor registration:

```csharp
actorRegistrations.RegisterProjectionActor<GitStorageAccount>();
actorRegistrations.RegisterProjectionActor<GitStorageAccountSummaryViewModel>();
actorRegistrations.RegisterProjectionActor<GitStorageAccountDetailsViewModel>();
```

## Best Practices

1. **Keep projections simple** - One projection per read model per event
2. **Handle all relevant events** - Ensure read models stay consistent
3. **Support snapshot events** - For rebuilding projections
4. **Use async/await** - For I/O operations
5. **Log projection failures** - For debugging and monitoring
