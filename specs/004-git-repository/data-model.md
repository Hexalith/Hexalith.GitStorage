# Data Model: GitRepository Aggregate

**Feature**: 004-git-repository
**Date**: 2025-12-15

## Entity: GitRepository

### Aggregate Root

```csharp
public sealed record GitRepository : IDomainAggregate
{
    // Identity
    string Id;                      // Composite: {OrganizationId}-{RepositoryName}

    // Core Properties
    string Name;                    // Repository name as it appears on Git Server
    string? Description;            // Optional description
    string? Url;                    // HTTPS clone URL
    string DefaultBranch;           // Default branch name (e.g., "main")

    // Relationships
    string OrganizationId;          // Reference to parent GitOrganization

    // Visibility & Access
    GitRepositoryVisibility Visibility;  // Public | Private | Internal

    // Origin & Sync
    GitRepositoryOrigin Origin;          // Synced | CreatedViaApplication
    string? RemoteId;                    // Repository's unique ID on remote Git Server
    GitRepositorySyncStatus SyncStatus;  // Synced | NotFoundOnRemote | SyncError
    DateTimeOffset? LastSyncedAt;        // Timestamp of last successful sync

    // Lifecycle
    bool Disabled;                       // Local suspension flag
}
```

### State Transitions

```
[Not Initialized] ──► GitRepositoryAdded ──► [Active]
[Not Initialized] ──► GitRepositorySynced ──► [Active]

[Active] ──► GitRepositoryDescriptionChanged ──► [Active]
[Active] ──► GitRepositoryVisibilityChanged ──► [Active]
[Active] ──► GitRepositoryDefaultBranchChanged ──► [Active]
[Active] ──► GitRepositorySynced ──► [Active]
[Active] ──► GitRepositoryMarkedNotFound ──► [Active] (SyncStatus = NotFoundOnRemote)
[Active] ──► GitRepositoryDisabled ──► [Disabled]

[Disabled] ──► GitRepositoryEnabled ──► [Active]
[Disabled] ──► (No other transitions allowed)
```

### Validation Rules

| Field | Rule | Source |
|-------|------|--------|
| Id | Required, non-empty | FR-003 |
| Name | Required, 1-100 chars, alphanumeric with `-_.`, no leading `.`, no trailing `.git` | FR-002, GitHub/Forgejo constraints |
| OrganizationId | Required, non-empty | FR-011 |
| Visibility | Valid enum value (Public, Private, Internal) | FR-010 |
| DefaultBranch | Required when set, non-empty | FR-008 |
| Url | Valid HTTPS URL when provided | FR-002 |

## Enums

### GitRepositoryVisibility

```csharp
public enum GitRepositoryVisibility
{
    Public = 0,     // Visible to everyone
    Private = 1,    // Visible only to authorized users
    Internal = 2    // Visible to organization members (GitHub Enterprise/Forgejo)
}
```

### GitRepositoryOrigin

```csharp
public enum GitRepositoryOrigin
{
    Synced = 0,                // Discovered from remote Git Server
    CreatedViaApplication = 1  // Created via this application's API
}
```

### GitRepositorySyncStatus

```csharp
public enum GitRepositorySyncStatus
{
    Synced = 0,           // Successfully synchronized with remote
    NotFoundOnRemote = 1, // Exists locally but not found on remote
    SyncError = 2         // Remote operation failed
}
```

## Events

### GitRepositoryAdded

Created when a repository is added via the application API (also creates on remote).

```csharp
public sealed record GitRepositoryAdded(
    string Id,                              // Order = 1
    string Name,                            // Order = 2
    string? Description,                    // Order = 3
    string OrganizationId,                  // Order = 4
    GitRepositoryVisibility Visibility,     // Order = 5
    string? DefaultBranch,                  // Order = 6
    string? RemoteId,                       // Order = 7
    string? Url                             // Order = 8
) : GitRepositoryEvent(Id);
```

### GitRepositorySynced

Created when a repository is discovered or re-synced from the remote Git Server.

```csharp
public sealed record GitRepositorySynced(
    string Id,                              // Order = 1
    string Name,                            // Order = 2
    string? Description,                    // Order = 3
    string OrganizationId,                  // Order = 4
    GitRepositoryVisibility Visibility,     // Order = 5
    string? DefaultBranch,                  // Order = 6
    string? RemoteId,                       // Order = 7
    string? Url,                            // Order = 8
    DateTimeOffset SyncedAt                 // Order = 9
) : GitRepositoryEvent(Id);
```

### GitRepositoryDescriptionChanged

Created when the repository description is updated.

```csharp
public sealed record GitRepositoryDescriptionChanged(
    string Id,              // Order = 1
    string? Description     // Order = 2
) : GitRepositoryEvent(Id);
```

### GitRepositoryVisibilityChanged

Created when the repository visibility setting is changed.

```csharp
public sealed record GitRepositoryVisibilityChanged(
    string Id,                              // Order = 1
    GitRepositoryVisibility Visibility      // Order = 2
) : GitRepositoryEvent(Id);
```

### GitRepositoryDefaultBranchChanged

Created when the default branch is changed.

```csharp
public sealed record GitRepositoryDefaultBranchChanged(
    string Id,              // Order = 1
    string DefaultBranch    // Order = 2
) : GitRepositoryEvent(Id);
```

### GitRepositoryDisabled

Created when the repository is disabled locally.

```csharp
public sealed record GitRepositoryDisabled(
    string Id               // Order = 1
) : GitRepositoryEvent(Id);
```

### GitRepositoryEnabled

Created when the repository is re-enabled.

```csharp
public sealed record GitRepositoryEnabled(
    string Id               // Order = 1
) : GitRepositoryEvent(Id);
```

### GitRepositoryMarkedNotFound

Created when sync determines the repository no longer exists on the remote.

```csharp
public sealed record GitRepositoryMarkedNotFound(
    string Id,                      // Order = 1
    DateTimeOffset MarkedAt         // Order = 2
) : GitRepositoryEvent(Id);
```

## Commands

### AddGitRepository

```csharp
public sealed record AddGitRepository(
    string Id,                              // Composite key
    string Name,                            // Repository name
    string? Description,                    // Optional description
    string OrganizationId,                  // Parent organization
    GitRepositoryVisibility Visibility,     // Visibility setting
    string? DefaultBranch                   // Optional default branch (defaults to "main")
) : GitRepositoryCommand(Id);
```

### SyncGitRepository

```csharp
public sealed record SyncGitRepository(
    string Id                               // Repository to sync
) : GitRepositoryCommand(Id);
```

### ChangeGitRepositoryDescription

```csharp
public sealed record ChangeGitRepositoryDescription(
    string Id,                              // Repository ID
    string? Description                     // New description (can be null to clear)
) : GitRepositoryCommand(Id);
```

### ChangeGitRepositoryVisibility

```csharp
public sealed record ChangeGitRepositoryVisibility(
    string Id,                              // Repository ID
    GitRepositoryVisibility Visibility      // New visibility
) : GitRepositoryCommand(Id);
```

### ChangeGitRepositoryDefaultBranch

```csharp
public sealed record ChangeGitRepositoryDefaultBranch(
    string Id,                              // Repository ID
    string DefaultBranch                    // New default branch name
) : GitRepositoryCommand(Id);
```

### DisableGitRepository

```csharp
public sealed record DisableGitRepository(
    string Id                               // Repository to disable
) : GitRepositoryCommand(Id);
```

### EnableGitRepository

```csharp
public sealed record EnableGitRepository(
    string Id                               // Repository to enable
) : GitRepositoryCommand(Id);
```

## View Models

### GitRepositorySummaryViewModel

For list/grid display.

```csharp
public sealed record GitRepositorySummaryViewModel(
    string Id,
    string Name,
    string OrganizationId,
    GitRepositoryVisibility Visibility,
    GitRepositorySyncStatus SyncStatus,
    bool Disabled
) : IIdDescription
{
    public string Description => Name;
}
```

### GitRepositoryDetailsViewModel

For details/edit display.

```csharp
public sealed record GitRepositoryDetailsViewModel(
    string Id,
    string Name,
    string? Description,
    string? Url,
    string? DefaultBranch,
    string OrganizationId,
    string? OrganizationName,               // Denormalized for display
    GitRepositoryVisibility Visibility,
    GitRepositoryOrigin Origin,
    string? RemoteId,
    GitRepositorySyncStatus SyncStatus,
    DateTimeOffset? LastSyncedAt,
    bool Disabled
);
```

## Relationships

```
┌─────────────────────┐
│  GitStorageAccount  │
│  (Parent)           │
└──────────┬──────────┘
           │ 1
           │
           │ *
┌──────────▼──────────┐
│   GitOrganization   │
│   (Parent)          │
└──────────┬──────────┘
           │ 1
           │
           │ *
┌──────────▼──────────┐
│    GitRepository    │
│    (This Feature)   │
└─────────────────────┘
```

**Cardinality**:
- One GitStorageAccount has many GitOrganizations
- One GitOrganization has many GitRepositories
- Each GitRepository belongs to exactly one GitOrganization
