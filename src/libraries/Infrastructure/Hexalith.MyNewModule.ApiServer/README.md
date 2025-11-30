# Hexalith.MyNewModule.ApiServer

This project provides the REST API server infrastructure for the MyToDo bounded context.

## Overview

The API Server module handles:
- REST API endpoints via controllers
- Dapr actor registration for event sourcing
- Integration event processing
- Service composition for the API layer

## Directory Structure

```
Hexalith.MyNewModule.ApiServer/
├── Controllers/
│   ├── MyToDoEventsBusTopicAttribute.cs
│   └── MyToDoIntegrationEventsController.cs
├── Modules/
│   └── HexalithMyNewModuleApiServerModule.cs
├── Properties/
│   └── launchSettings.json
└── Hexalith.MyNewModule.ApiServer.csproj
```

## Module Definition

### HexalithMyNewModuleApiServerModule

The main module class implementing `IApiServerApplicationModule`:

```csharp
public sealed class HexalithMyNewModuleApiServerModule : 
    IApiServerApplicationModule, IMyToDoModule
{
    public IDictionary<string, AuthorizationPolicy> AuthorizationPolicies 
        => MyToDoModulePolicies.AuthorizationPolicies;
    
    public IEnumerable<string> Dependencies => [];
    public string Description => "Hexalith MyToDo API Server module";
    public string Id => "Hexalith.MyNewModule.ApiServer";
    public string Name => "Hexalith MyToDo API Server";
    public int OrderWeight => 0;
    public string Version => "1.0";
}
```

### Service Registration

```csharp
public static void AddServices(IServiceCollection services, IConfiguration configuration)
{
    // Configure settings
    services.ConfigureSettings<CosmosDbSettings>(configuration);

    // Register serialization mappers
    HexalithMyNewModuleEventsSerialization.RegisterPolymorphicMappers();
    HexalithMyNewModuleCommandsSerialization.RegisterPolymorphicMappers();
    HexalithMyNewModuleRequestsSerialization.RegisterPolymorphicMappers();

    // Add application module
    services.TryAddSingleton<IMyToDoModule, HexalithMyNewModuleApiServerModule>();

    // Add command handlers
    services.AddMyToDo();

    // Add projection handlers and actor factories
    services.AddMyToDoProjectionActorFactories();

    // Add controllers
    services.AddControllers()
        .AddApplicationPart(typeof(MyToDoIntegrationEventsController).Assembly);
}
```

### Actor Registration

```csharp
public static void RegisterActors(object actorCollection)
{
    var actorRegistrations = (ActorRegistrationCollection)actorCollection;
    
    // Domain aggregate actor
    actorRegistrations.RegisterActor<DomainAggregateActor>(
        MyToDoDomainHelper.MyToDoAggregateName.ToAggregateActorName());
    
    // Projection actors
    actorRegistrations.RegisterProjectionActor<MyToDo>();
    actorRegistrations.RegisterProjectionActor<MyToDoSummaryViewModel>();
    actorRegistrations.RegisterProjectionActor<MyToDoDetailsViewModel>();
    
    // ID collection actor
    actorRegistrations.RegisterActor<SequentialStringListActor>(
        IIdCollectionFactory.GetAggregateCollectionName(
            MyToDoDomainHelper.MyToDoAggregateName));
}
```

## Controllers

### MyToDoIntegrationEventsController

Handles integration events from the message bus:

```csharp
[ApiController]
[Route("api/v1/[controller]")]
public class MyToDoIntegrationEventsController : ControllerBase
{
    [HttpPost]
    [MyToDoEventsBusTopic]
    public async Task<IActionResult> HandleIntegrationEvent(
        [FromBody] CloudEvent cloudEvent,
        CancellationToken cancellationToken)
    {
        // Process integration event
    }
}
```

### MyToDoEventsBusTopicAttribute

Custom attribute for Dapr pub/sub subscription:

```csharp
[AttributeUsage(AttributeTargets.Method)]
public class MyToDoEventsBusTopicAttribute : TopicAttribute
{
    public MyToDoEventsBusTopicAttribute() 
        : base("pubsub", "mytodo-events")
    {
    }
}
```

## Dapr Integration

### Actors

| Actor Type | Purpose | Naming |
|------------|---------|--------|
| `DomainAggregateActor` | Event sourcing for aggregates | `{AggregateName}Aggregate` |
| `ProjectionActor<T>` | Read model maintenance | `{TypeName}Projection` |
| `SequentialStringListActor` | ID collection management | `{AggregateName}Collection` |

### State Stores

- Event store for domain events
- Projection store for read models

### Pub/Sub

- Topic: `mytodo-events`
- Used for integration events

## Configuration

### appsettings.json

```json
{
  "CosmosDb": {
    "ConnectionString": "your-connection-string",
    "DatabaseName": "MyNewModule"
  },
  "Dapr": {
    "AppId": "MyNewModule-api",
    "HttpPort": 3500,
    "GrpcPort": 50001
  }
}
```

## Dependencies

- `Hexalith.Infrastructure.DaprRuntime` - Dapr integration
- `Hexalith.Infrastructure.CosmosDb.Configurations` - CosmosDB settings
- `Hexalith.UI.ApiServer` - API server framework
- `Hexalith.MyNewModule` - Application layer

## API Endpoints

The module exposes endpoints under `/api/v1/mytodo/`:

| Method | Endpoint | Description |
|--------|----------|-------------|
| POST | `/api/v1/commands` | Submit commands |
| POST | `/api/v1/requests` | Submit queries |
| POST | `/api/v1/integration-events` | Handle integration events |

## Security

API endpoints are secured using:
- JWT authentication
- Role-based authorization policies
- HTTPS enforcement

## Best Practices

1. **Use Dapr abstractions** - Don't couple directly to infrastructure
2. **Register all actors** - Ensure actors are properly registered
3. **Handle errors gracefully** - Return appropriate HTTP status codes
4. **Log requests** - Use structured logging
5. **Validate input** - Use model validation
