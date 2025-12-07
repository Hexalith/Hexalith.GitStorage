# Data Model: GitOrganization Entity

**Feature**: 002-git-organization
**Date**: 2025-12-07

## Entity Overview

The GitOrganization entity represents an organization on a Git server (GitHub or Forgejo). Organizations can be discovered via synchronization from a connected Git Storage Account or created through the application API.

## Core Entity

### GitOrganization (Aggregate)

| Field | Type | Constraints | Description |
|-------|------|-------------|-------------|
| `Id` | `string` | Required, Composite key | Format: `{GitStorageAccountId}-{OrganizationName}` (lowercase) |
| `Name` | `string` | Required, 1-39 chars, Regex pattern | Organization name as it appears on the Git Server |
| `Description` | `string?` | Optional | Human-readable description of the organization |
| `GitStorageAccountId` | `string` | Required, FK | Reference to parent Git Storage Account |
| `Origin` | `GitOrganizationOrigin` | Required | How the organization was added (Synced/CreatedViaApplication) |
| `RemoteId` | `string?` | Optional | Organization's unique identifier on the remote Git Server |
| `SyncStatus` | `GitOrganizationSyncStatus` | Required | Current synchronization state |
| `LastSyncedAt` | `DateTimeOffset?` | Optional | Timestamp of last successful sync |
| `Disabled` | `bool` | Required, Default: false | Soft-delete flag |

**Validation Rules**:

- `Id`: Non-empty, deterministically generated from GitStorageAccountId + Name
- `Name`: 1-39 characters, matches pattern `^[a-zA-Z0-9][a-zA-Z0-9_-]*[a-zA-Z0-9]$|^[a-zA-Z0-9]$`
- `GitStorageAccountId`: Non-empty, must reference existing Git Storage Account

## Enumerations

### GitOrganizationOrigin

| Value | Code | Description |
|-------|------|-------------|
| Synced | 0 | Discovered from remote during synchronization |
| CreatedViaApplication | 1 | Created through application API |

### GitOrganizationSyncStatus

| Value | Code | Description |
|-------|------|-------------|
| Synced | 0 | Successfully synchronized with remote |
| NotFoundOnRemote | 1 | Previously synced but no longer exists on remote |
| SyncError | 2 | Last sync operation failed |

## Domain Events

### Initialization Events

| Event | Fields | Trigger |
|-------|--------|---------|
| `GitOrganizationAdded` | Id, Name, Description?, GitStorageAccountId | API create operation |
| `GitOrganizationSynced` | Id, Name, Description?, GitStorageAccountId, RemoteId?, SyncedAt | Sync discovery |

### State Change Events

| Event | Fields | Trigger |
|-------|--------|---------|
| `GitOrganizationDescriptionChanged` | Id, Name, Description? | Update operation |
| `GitOrganizationMarkedNotFound` | Id, MarkedAt | Sync detects removal |
| `GitOrganizationDisabled` | Id | Soft-delete operation |
| `GitOrganizationEnabled` | Id | Re-enable operation |

## Commands

| Command | Fields | Description |
|---------|--------|-------------|
| `AddGitOrganization` | Id, Name, Description?, GitStorageAccountId | Create via API (triggers remote creation) |
| `ChangeGitOrganizationDescription` | Id, Name, Description? | Update description (triggers remote update) |
| `DisableGitOrganization` | Id | Soft-delete (local only) |
| `EnableGitOrganization` | Id | Re-enable (local only) |
| `SyncGitOrganizations` | GitStorageAccountId | Trigger bulk sync from remote |

## Requests (Read Operations)

| Request | Parameters | Returns |
|---------|------------|---------|
| `GetGitOrganizationDetails` | Id | `GitOrganizationDetailsViewModel` |
| `GetGitOrganizationSummaries` | Skip, Take, Search?, Ids? | `IEnumerable<GitOrganizationSummaryViewModel>` |

## View Models

### GitOrganizationDetailsViewModel

| Field | Type | Description |
|-------|------|-------------|
| `Id` | `string` | Composite key |
| `Name` | `string` | Organization name |
| `Description` | `string?` | Description |
| `GitStorageAccountId` | `string` | Parent account reference |
| `GitStorageAccountName` | `string` | Denormalized account name for display |
| `Origin` | `GitOrganizationOrigin` | How organization was added |
| `RemoteId` | `string?` | Remote identifier |
| `SyncStatus` | `GitOrganizationSyncStatus` | Current sync state |
| `LastSyncedAt` | `DateTimeOffset?` | Last sync timestamp |
| `Disabled` | `bool` | Soft-delete flag |

### GitOrganizationSummaryViewModel

| Field | Type | Description |
|-------|------|-------------|
| `Id` | `string` | Composite key |
| `Name` | `string` | Organization name |
| `GitStorageAccountId` | `string` | Parent account reference |
| `SyncStatus` | `GitOrganizationSyncStatus` | Current sync state |
| `Disabled` | `bool` | Soft-delete flag |

## Relationships

```text
┌─────────────────────┐       1    ┌─────────────────────┐
│  GitStorageAccount  │───────────*│   GitOrganization   │
└─────────────────────┘            └─────────────────────┘
        │                                    │
        │ Id: string                         │ Id: {AccountId}-{Name}
        │ Name: string                       │ GitStorageAccountId: string (FK)
        │ ServerType: enum                   │ Origin: enum
        │ ...                                │ SyncStatus: enum
        │                                    │ ...
```

**Relationship Details**:

- One Git Storage Account has many Git Organizations (1:N)
- GitOrganization.GitStorageAccountId references GitStorageAccount.Id
- Organization Id is derived from parent account Id

## State Transitions

```text
                    ┌──────────────┐
                    │ Uninitialized│
                    └──────┬───────┘
                           │
         ┌─────────────────┼─────────────────┐
         │                 │                 │
         ▼                 ▼                 │
┌─────────────────┐ ┌─────────────────┐     │
│GitOrganization- │ │GitOrganization- │     │
│     Added       │ │     Synced      │     │
└────────┬────────┘ └────────┬────────┘     │
         │                   │              │
         └─────────┬─────────┘              │
                   │                        │
                   ▼                        │
          ┌────────────────┐                │
          │   Initialized  │◄───────────────┘
          │    (Active)    │
          └───────┬────────┘
                  │
    ┌─────────────┼─────────────┬───────────────┐
    │             │             │               │
    ▼             ▼             ▼               ▼
┌────────┐  ┌──────────┐  ┌──────────┐   ┌──────────┐
│Descrip-│  │ Marked   │  │ Disabled │   │  Synced  │
│tion    │  │NotFound  │  │          │   │  (again) │
│Changed │  │          │  │          │   │          │
└────────┘  └──────────┘  └─────┬────┘   └──────────┘
                                │
                                ▼
                         ┌──────────┐
                         │ Enabled  │
                         │(Re-active)│
                         └──────────┘
```

## Serialization Attributes

All domain types use these attributes for JSON polymorphic serialization:

```csharp
// Events, Commands, Requests
[PolymorphicSerialization]
public partial record GitOrganizationAdded(...)

// Aggregates, ViewModels
[DataContract]
public sealed record GitOrganization(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    ...)
```

**Order Convention**:

- Order 1: Id (primary key)
- Order 2+: Additional fields in logical order
