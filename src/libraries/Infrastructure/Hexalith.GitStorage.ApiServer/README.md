# Hexalith.GitStorage.ApiServer

This project provides the REST API server infrastructure for the GitStorageAccount bounded context.

## Overview

The API Server module handles:
- REST API endpoints via controllers
- Dapr actor registration for event sourcing
- Integration event processing
- Service composition for the API layer

## Directory Structure

```
Hexalith.GitStorage.ApiServer/
├── Controllers/
│   ├── GitStorageAccountEventsBusTopicAttribute.cs
│   └── GitStorageAccountIntegrationEventsController.cs
├── Modules/
│   └── HexalithGitStorageApiServerModule.cs
├── Properties/
│   └── launchSettings.json
└── Hexalith.GitStorage.ApiServer.csproj
```

## Module Definition

### HexalithGitStorageApiServerModule

The main module class implementing `IApiServerApplicationModule`:

```csharp
public sealed class HexalithGitStorageApiServerModule : 
    IApiServerApplicationModule, IGitStorageAccountModule
{
    public IDictionary<string, AuthorizationPolicy> AuthorizationPolicies 
        => GitStorageAccountModulePolicies.AuthorizationPolicies;
    
    public IEnumerable<string> Dependencies => [];
    public string Description => "Hexalith GitStorageAccount API Server module";
    public string Id => "Hexalith.GitStorage.ApiServer";
    public string Name => "Hexalith GitStorageAccount API Server";
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
    HexalithGitStorageEventsSerialization.RegisterPolymorphicMappers();
    HexalithGitStorageCommandsSerialization.RegisterPolymorphicMappers();
    HexalithGitStorageRequestsSerialization.RegisterPolymorphicMappers();

    // Add application module
    services.TryAddSingleton<IGitStorageAccountModule, HexalithGitStorageApiServerModule>();

    // Add command handlers
    services.AddGitStorageAccount();

    // Add projection handlers and actor factories
    services.AddGitStorageAccountProjectionActorFactories();

    // Add controllers
    services.AddControllers()
        .AddApplicationPart(typeof(GitStorageAccountIntegrationEventsController).Assembly);
}
```

### Actor Registration

```csharp
public static void RegisterActors(object actorCollection)
{
    var actorRegistrations = (ActorRegistrationCollection)actorCollection;
    
    // Domain aggregate actor
    actorRegistrations.RegisterActor<DomainAggregateActor>(
        GitStorageAccountDomainHelper.GitStorageAccountAggregateName.ToAggregateActorName());
    
    // Projection actors
    actorRegistrations.RegisterProjectionActor<GitStorageAccount>();
    actorRegistrations.RegisterProjectionActor<GitStorageAccountSummaryViewModel>();
    actorRegistrations.RegisterProjectionActor<GitStorageAccountDetailsViewModel>();
    
    // ID collection actor
    actorRegistrations.RegisterActor<SequentialStringListActor>(
        IIdCollectionFactory.GetAggregateCollectionName(
            GitStorageAccountDomainHelper.GitStorageAccountAggregateName));
}
```

## Controllers

### GitStorageAccountIntegrationEventsController

Handles integration events from the message bus:

```csharp
[ApiController]
[Route("api/v1/[controller]")]
public class GitStorageAccountIntegrationEventsController : ControllerBase
{
    [HttpPost]
    [GitStorageAccountEventsBusTopic]
    public async Task<IActionResult> HandleIntegrationEvent(
        [FromBody] CloudEvent cloudEvent,
        CancellationToken cancellationToken)
    {
        // Process integration event
    }
}
```

### GitStorageAccountEventsBusTopicAttribute

Custom attribute for Dapr pub/sub subscription:

```csharp
[AttributeUsage(AttributeTargets.Method)]
public class GitStorageAccountEventsBusTopicAttribute : TopicAttribute
{
    public GitStorageAccountEventsBusTopicAttribute() 
        : base("pubsub", "GitStorageAccount-events")
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

- Topic: `GitStorageAccount-events`
- Used for integration events

## Configuration

### appsettings.json

```json
{
  "CosmosDb": {
    "ConnectionString": "your-connection-string",
    "DatabaseName": "GitStorage"
  },
  "Dapr": {
    "AppId": "GitStorage-api",
    "HttpPort": 3500,
    "GrpcPort": 50001
  }
}
```

## Dependencies

- `Hexalith.Infrastructure.DaprRuntime` - Dapr integration
- `Hexalith.Infrastructure.CosmosDb.Configurations` - CosmosDB settings
- `Hexalith.UI.ApiServer` - API server framework
- `Hexalith.GitStorage` - Application layer

## API Endpoints

The module exposes endpoints under `/api/v1/GitStorageAccount/`:

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
