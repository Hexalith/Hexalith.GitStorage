# Hexalith.MyNewModule.Requests

This project contains the request (query) definitions and view models for the MyToDo bounded context.

## Overview

Requests represent queries for data retrieval in the CQRS pattern. They are handled separately from commands and typically read from optimized read models (projections).

## Request Types

### Query Requests

| Request | Description | Returns |
|---------|-------------|---------|
| `GetMyToDoDetails` | Get full details of a single module | `MyToDoDetailsViewModel` |
| `GetMyToDoSummaries` | Get paginated list of module summaries | `IEnumerable<MyToDoSummaryViewModel>` |
| `GetMyToDoIds` | Get list of all module IDs | `IEnumerable<string>` |
| `GetMyToDoExports` | Export full module data | `IEnumerable<MyToDo>` |
| `GetMyToDoIdDescription` | Get ID/Description pairs for dropdowns | `IEnumerable<IdDescription>` |

## Request Definitions

### MyToDoRequest (Base)

```csharp
[PolymorphicSerialization]
public abstract partial record MyToDoRequest
{
    public static string AggregateName => MyToDoDomainHelper.MyToDoAggregateName;
}
```

### GetMyToDoDetails

```csharp
[PolymorphicSerialization]
public partial record GetMyToDoDetails(string Id) : MyToDoRequest;
```

### GetMyToDoSummaries

```csharp
[PolymorphicSerialization]
public partial record GetMyToDoSummaries() : MyToDoRequest;
```

## View Models

### MyToDoDetailsViewModel

Full details view model for detail pages:

```csharp
[DataContract]
public sealed record MyToDoDetailsViewModel(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments,
    [property: DataMember(Order = 5)] bool Disabled) : IIdDescription
{
    public static MyToDoDetailsViewModel Empty 
        => new(string.Empty, string.Empty, string.Empty, false);
        
    public static MyToDoDetailsViewModel Create(string? id)
        => new(
            string.IsNullOrWhiteSpace(id) ? UniqueIdHelper.GenerateUniqueStringId() : id, 
            string.Empty, 
            string.Empty, 
            false);
}
```

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `string` | Unique identifier |
| `Name` | `string` | Display name |
| `Comments` | `string?` | Optional description |
| `Disabled` | `bool` | Whether the module is disabled |

### MyToDoSummaryViewModel

Lightweight view model for list/grid displays:

```csharp
[DataContract]
public sealed record MyToDoSummaryViewModel(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] bool Disabled) : IIdDescription;
```

## Request Handlers

Request handlers are registered in the Projections project:

```csharp
public static IServiceCollection AddMyToDoRequestHandlers(this IServiceCollection services)
{
    services.TryAddScoped<IRequestHandler<GetMyToDoDetails>, GetMyToDoDetailsHandler>();
    services.TryAddScoped<IRequestHandler<GetMyToDoSummaries>, 
        GetFilteredCollectionHandler<GetMyToDoSummaries, MyToDoSummaryViewModel>>();
    services.TryAddScoped<IRequestHandler<GetMyToDoIds>, 
        GetAggregateIdsRequestHandler<GetMyToDoIds>>();
    return services;
}
```

## Request Flow

```
┌─────────────┐    ┌─────────────────┐    ┌──────────────┐
│   Request   │───▶│ Request Handler │───▶│  Read Model  │
│   (Query)   │    │   (Processor)   │    │ (Projection) │
└─────────────┘    └─────────────────┘    └──────────────┘
                           │
                           ▼
                    ┌──────────────┐
                    │  View Model  │
                    │   (Result)   │
                    └──────────────┘
```

## Dependencies

- `Hexalith.PolymorphicSerializations` - Serialization support
- `Hexalith.Domains.ValueObjects` - Common value objects

## Usage Example

```csharp
// Get single module details
var request = new GetMyToDoDetails("mod-001");
var response = await requestService.SubmitAsync(user, request, cancellationToken);
var details = response.Result;

// Get all summaries
var summariesRequest = new GetMyToDoSummaries();
var summariesResponse = await requestService.SubmitAsync(user, summariesRequest, cancellationToken);
var summaries = summariesResponse.Results;

// Get IDs for dropdown
var idsRequest = new GetMyToDoIds();
var idsResponse = await requestService.SubmitAsync(user, idsRequest, cancellationToken);
var ids = idsResponse.Results;
```

## Best Practices

1. **Keep view models lean** - Only include data needed by the UI
2. **Use appropriate request types** - Details vs Summary vs Export
3. **Support pagination** - For large datasets
4. **Implement caching** - For frequently accessed data
5. **Handle not found** - Return appropriate responses for missing data
