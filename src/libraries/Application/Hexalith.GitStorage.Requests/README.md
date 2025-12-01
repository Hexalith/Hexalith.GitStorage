# Hexalith.GitStorage.Requests

This project contains the request (query) definitions and view models for the GitStorage bounded context, enabling queries for Git storage accounts, organizations, and repositories.

## Overview

Requests represent queries for data retrieval in the CQRS pattern. They are handled separately from commands and typically read from optimized read models (projections).

## Request Types

### Query Requests

| Request | Description | Returns |
|---------|-------------|---------|
| `GetGitStorageAccountDetails` | Get full details of a storage account | `GitStorageAccountDetailsViewModel` |
| `GetGitStorageAccountSummaries` | Get paginated list of account summaries | `IEnumerable<GitStorageAccountSummaryViewModel>` |
| `GetGitStorageAccountIds` | Get list of all account IDs | `IEnumerable<string>` |
| `GetOrganizations` | Get organizations for an account | `IEnumerable<OrganizationViewModel>` |
| `GetRepositories` | Get repositories for an organization | `IEnumerable<RepositoryViewModel>` |

## Request Definitions

### GitStorageAccountRequest (Base)

```csharp
[PolymorphicSerialization]
public abstract partial record GitStorageAccountRequest
{
    public static string AggregateName => GitStorageAccountDomainHelper.GitStorageAccountAggregateName;
}
```

### GetGitStorageAccountDetails

```csharp
[PolymorphicSerialization]
public partial record GetGitStorageAccountDetails(string Id) : GitStorageAccountRequest;
```

### GetGitStorageAccountSummaries

```csharp
[PolymorphicSerialization]
public partial record GetGitStorageAccountSummaries() : GitStorageAccountRequest;
```

## View Models

### GitStorageAccountDetailsViewModel

Full details view model for account detail pages:

```csharp
[DataContract]
public sealed record GitStorageAccountDetailsViewModel(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] GitProviderType ProviderType,
    [property: DataMember(Order = 4)] string BaseUrl,
    [property: DataMember(Order = 5)] string? Comments,
    [property: DataMember(Order = 6)] bool Disabled) : IIdDescription;
```

| Property | Type | Description |
|----------|------|-------------|
| `Id` | `string` | Unique identifier |
| `Name` | `string` | Display name |
| `ProviderType` | `GitProviderType` | GitHub or Forgejo |
| `BaseUrl` | `string` | API base URL |
| `Comments` | `string?` | Optional description |
| `Disabled` | `bool` | Whether the account is disabled |

### GitStorageAccountSummaryViewModel

Lightweight view model for list/grid displays:

```csharp
[DataContract]
public sealed record GitStorageAccountSummaryViewModel(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] GitProviderType ProviderType,
    [property: DataMember(Order = 4)] bool Disabled) : IIdDescription;
```

### OrganizationViewModel

View model for organization data:

```csharp
[DataContract]
public sealed record OrganizationViewModel(
    [property: DataMember(Order = 1)] string Name,
    [property: DataMember(Order = 2)] string? Description,
    [property: DataMember(Order = 3)] string AvatarUrl);
```

### RepositoryViewModel

View model for repository data:

```csharp
[DataContract]
public sealed record RepositoryViewModel(
    [property: DataMember(Order = 1)] string Name,
    [property: DataMember(Order = 2)] string? Description,
    [property: DataMember(Order = 3)] bool IsPrivate,
    [property: DataMember(Order = 4)] string DefaultBranch,
    [property: DataMember(Order = 5)] string CloneUrl);
```

## Request Handlers

Request handlers are registered in the Projections project:

```csharp
public static IServiceCollection AddGitStorageAccountRequestHandlers(this IServiceCollection services)
{
    services.TryAddScoped<IRequestHandler<GetGitStorageAccountDetails>, GetGitStorageAccountDetailsHandler>();
    services.TryAddScoped<IRequestHandler<GetGitStorageAccountSummaries>, 
        GetFilteredCollectionHandler<GetGitStorageAccountSummaries, GitStorageAccountSummaryViewModel>>();
    services.TryAddScoped<IRequestHandler<GetGitStorageAccountIds>, 
        GetAggregateIdsRequestHandler<GetGitStorageAccountIds>>();
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
var request = new GetGitStorageAccountDetails("mod-001");
var response = await requestService.SubmitAsync(user, request, cancellationToken);
var details = response.Result;

// Get all summaries
var summariesRequest = new GetGitStorageAccountSummaries();
var summariesResponse = await requestService.SubmitAsync(user, summariesRequest, cancellationToken);
var summaries = summariesResponse.Results;

// Get IDs for dropdown
var idsRequest = new GetGitStorageAccountIds();
var idsResponse = await requestService.SubmitAsync(user, idsRequest, cancellationToken);
var ids = idsResponse.Results;
```

## Best Practices

1. **Keep view models lean** - Only include data needed by the UI
2. **Use appropriate request types** - Details vs Summary vs Export
3. **Support pagination** - For large datasets
4. **Implement caching** - For frequently accessed data
5. **Handle not found** - Return appropriate responses for missing data
