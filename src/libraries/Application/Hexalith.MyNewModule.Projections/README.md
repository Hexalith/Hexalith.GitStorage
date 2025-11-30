# Hexalith.MyNewModule.Projections

This project contains the projection handlers and request handlers for maintaining read models in the MyToDo bounded context.

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
│   ├── MyToDoAddedOnDetailsProjectionHandler.cs
│   ├── MyToDoDescriptionChangedOnDetailsProjectionHandler.cs
│   ├── MyToDoDisabledOnDetailsProjectionHandler.cs
│   └── MyToDoEnabledOnDetailsProjectionHandler.cs
│
└── Summaries/
    ├── MyToDoAddedOnSummaryProjectionHandler.cs
    ├── MyToDoDescriptionChangedOnSummaryProjectionHandler.cs
    ├── MyToDoDisabledOnSummaryProjectionHandler.cs
    ├── MyToDoEnabledOnSummaryProjectionHandler.cs
    └── MyToDoSnapshotOnSummaryProjectionHandler.cs
```

### Registration

```csharp
public static IServiceCollection AddMyToDoProjectionHandlers(this IServiceCollection services)
    => services
        // Collection projections
        .AddScoped<IProjectionUpdateHandler<MyToDoAdded>, 
            IdsCollectionProjectionHandler<MyToDoAdded>>()
        
        // Summary projections
        .AddScoped<IProjectionUpdateHandler<MyToDoAdded>, 
            MyToDoAddedOnSummaryProjectionHandler>()
        .AddScoped<IProjectionUpdateHandler<MyToDoDescriptionChanged>, 
            MyToDoDescriptionChangedOnSummaryProjectionHandler>()
        .AddScoped<IProjectionUpdateHandler<MyToDoDisabled>, 
            MyToDoDisabledOnSummaryProjectionHandler>()
        .AddScoped<IProjectionUpdateHandler<MyToDoEnabled>, 
            MyToDoEnabledOnSummaryProjectionHandler>()
        .AddScoped<IProjectionUpdateHandler<SnapshotEvent>, 
            MyToDoSnapshotOnSummaryProjectionHandler>()
        
        // Details projections
        .AddScoped<IProjectionUpdateHandler<MyToDoAdded>, 
            MyToDoAddedOnDetailsProjectionHandler>()
        // ... more handlers
```

### Handler Example

```csharp
public class MyToDoAddedOnSummaryProjectionHandler : 
    IProjectionUpdateHandler<MyToDoAdded>
{
    public async Task<IEnumerable<object>> HandleAsync(
        MyToDoAdded @event, 
        CancellationToken cancellationToken)
    {
        var summary = new MyToDoSummaryViewModel(
            @event.Id,
            @event.Name,
            false);
            
        // Return projection updates
        return [new AddProjection<MyToDoSummaryViewModel>(summary)];
    }
}
```

## Request Handlers

### Available Handlers

| Handler | Request | Description |
|---------|---------|-------------|
| `GetMyToDoDetailsHandler` | `GetMyToDoDetails` | Retrieves full module details |
| `GetFilteredCollectionHandler` | `GetMyToDoSummaries` | Retrieves paginated summaries |
| `GetAggregateIdsRequestHandler` | `GetMyToDoIds` | Retrieves all module IDs |
| `GetExportsRequestHandler` | `GetMyToDoExports` | Exports full aggregate data |

### Request Handler Example

```csharp
public class GetMyToDoDetailsHandler : IRequestHandler<GetMyToDoDetails>
{
    private readonly IProjectionStore<MyToDoDetailsViewModel> _store;

    public GetMyToDoDetailsHandler(
        IProjectionStore<MyToDoDetailsViewModel> store)
    {
        _store = store;
    }

    public async Task<RequestResult<MyToDoDetailsViewModel>> HandleAsync(
        GetMyToDoDetails request, 
        CancellationToken cancellationToken)
    {
        var details = await _store.GetAsync(request.Id, cancellationToken);
        return details is null 
            ? RequestResult<MyToDoDetailsViewModel>.NotFound(request.Id)
            : RequestResult<MyToDoDetailsViewModel>.Success(details);
    }
}
```

## Helper Classes

### MyToDoProjectionHelper

Provides extension methods for service registration:

```csharp
public static class MyToDoProjectionHelper
{
    // Register aggregate providers
    public static IServiceCollection AddMyToDoAggregateProviders(
        this IServiceCollection services);
    
    // Register projection handlers
    public static IServiceCollection AddMyToDoProjectionHandlers(
        this IServiceCollection services);
    
    // Register projections and request handlers
    public static IServiceCollection AddMyToDoProjections(
        this IServiceCollection services);
    
    // Register query services
    public static IServiceCollection AddMyToDoQueryServices(
        this IServiceCollection services);
    
    // Register request handlers
    public static IServiceCollection AddMyToDoRequestHandlers(
        this IServiceCollection services);
}
```

## Services

### MyNewModuleQuickStartData

Provides sample data for development and demos:

```csharp
public static class MyNewModuleQuickStartData
{
    public static IEnumerable<object> Data => [
        new AddMyToDo("sample-001", "Sample Module 1", "First sample"),
        new AddMyToDo("sample-002", "Sample Module 2", "Second sample")
    ];
}
```

## Dependencies

- `Hexalith.Application.Projections` - Projection abstractions
- `Hexalith.Application.Requests` - Request handling
- `Hexalith.MyNewModule.Events` - Domain events
- `Hexalith.MyNewModule.Requests` - Request/view model definitions

## Dapr Actor Integration

Projections are executed via Dapr actors for:
- Reliable event processing
- State persistence
- Scalability

Actor registration:

```csharp
actorRegistrations.RegisterProjectionActor<MyToDo>();
actorRegistrations.RegisterProjectionActor<MyToDoSummaryViewModel>();
actorRegistrations.RegisterProjectionActor<MyToDoDetailsViewModel>();
```

## Best Practices

1. **Keep projections simple** - One projection per read model per event
2. **Handle all relevant events** - Ensure read models stay consistent
3. **Support snapshot events** - For rebuilding projections
4. **Use async/await** - For I/O operations
5. **Log projection failures** - For debugging and monitoring
