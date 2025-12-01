# Data Model: Git Storage Account Entity

**Feature Branch**: `001-git-storage-account`
**Date**: 2025-12-01

## Entity Overview

The `GitStorageAccount` entity represents a configured connection to a git repository server for storage purposes. It follows DDD aggregate patterns with event sourcing.

---

## Aggregate: GitStorageAccount

### Properties

| Property | Type | DataMember Order | Required | Description |
|----------|------|------------------|----------|-------------|
| `Id` | `string` | 1 | Yes | Unique identifier (user-provided, system-enforced uniqueness) |
| `Name` | `string` | 2 | Yes | Human-readable display name (duplicates allowed) |
| `Comments` | `string?` | 3 | No | Optional description or notes about the account |
| `Disabled` | `bool` | 4 | Yes | Whether the account is disabled (default: false) |

### Computed Properties

| Property | Type | Description |
|----------|------|-------------|
| `AggregateId` | `string` | Returns `Id` |
| `AggregateName` | `string` | Returns `GitStorageAccountDomainHelper.GitStorageAccountAggregateName` |

### State Transitions

```
┌─────────────────┐
│   Uninitialized │
└────────┬────────┘
         │ GitStorageAccountAdded
         ▼
┌─────────────────┐  GitStorageAccountDisabled  ┌──────────────────┐
│     Enabled     │ ──────────────────────────▶ │     Disabled     │
│  (Disabled=false)│ ◀────────────────────────── │  (Disabled=true) │
└─────────────────┘  GitStorageAccountEnabled   └──────────────────┘
         │
         │ GitStorageAccountDescriptionChanged
         ▼
┌─────────────────┐
│   Enabled       │ (Name/Comments updated)
└─────────────────┘
```

### Validation Rules

| Rule | Field(s) | Condition | Error Message |
|------|----------|-----------|---------------|
| VR-001 | Id | Not null or empty | "Account Id is required" |
| VR-002 | Name | Not null or empty | "Account Name is required" |
| VR-003 | (aggregate) | Not already initialized | "The GitStorageAccount already exists" |
| VR-004 | (aggregate) | Must be initialized for updates | "The GitStorageAccount does not exist" |
| VR-005 | (aggregate) | Must not be disabled for DescriptionChanged | "Cannot update a disabled GitStorageAccount" |

---

## Domain Events

### GitStorageAccountEvent (Base)

| Property | Type | DataMember Order | Description |
|----------|------|------------------|-------------|
| `Id` | `string` | 1 | The aggregate identifier |

### GitStorageAccountAdded

| Property | Type | DataMember Order | Description |
|----------|------|------------------|-------------|
| `Id` | `string` | 1 (inherited) | The aggregate identifier |
| `Name` | `string` | 2 | Display name |
| `Comments` | `string?` | 3 | Optional description |

### GitStorageAccountDescriptionChanged

| Property | Type | DataMember Order | Description |
|----------|------|------------------|-------------|
| `Id` | `string` | 1 (inherited) | The aggregate identifier |
| `Name` | `string` | 2 | Updated display name |
| `Comments` | `string?` | 3 | Updated description |

### GitStorageAccountEnabled

| Property | Type | DataMember Order | Description |
|----------|------|------------------|-------------|
| `Id` | `string` | 1 (inherited) | The aggregate identifier |

### GitStorageAccountDisabled

| Property | Type | DataMember Order | Description |
|----------|------|------------------|-------------|
| `Id` | `string` | 1 (inherited) | The aggregate identifier |

---

## Commands

### GitStorageAccountCommand (Base)

| Property | Type | DataMember Order | Description |
|----------|------|------------------|-------------|
| `Id` | `string` | 1 | The aggregate identifier |

### AddGitStorageAccount

| Property | Type | DataMember Order | Required | Validation |
|----------|------|------------------|----------|------------|
| `Id` | `string` | 1 (inherited) | Yes | Not empty |
| `Name` | `string` | 2 | Yes | Not empty |
| `Comments` | `string?` | 3 | No | None |

### ChangeGitStorageAccountDescription

| Property | Type | DataMember Order | Required | Validation |
|----------|------|------------------|----------|------------|
| `Id` | `string` | 1 (inherited) | Yes | Not empty |
| `Name` | `string` | 2 | Yes | Not empty |
| `Comments` | `string?` | 3 | No | None |

### EnableGitStorageAccount

| Property | Type | DataMember Order | Required | Validation |
|----------|------|------------------|----------|------------|
| `Id` | `string` | 1 (inherited) | Yes | Not empty |

### DisableGitStorageAccount

| Property | Type | DataMember Order | Required | Validation |
|----------|------|------------------|----------|------------|
| `Id` | `string` | 1 (inherited) | Yes | Not empty |

---

## Requests (Queries)

### GitStorageAccountRequest (Base)

| Property | Type | DataMember Order | Description |
|----------|------|------------------|-------------|
| `Id` | `string` | 1 | The aggregate identifier |

### GetGitStorageAccountDetails

| Property | Type | DataMember Order | Description |
|----------|------|------------------|-------------|
| `Id` | `string` | 1 (inherited) | The account to retrieve |
| `Result` | `GitStorageAccountDetailsViewModel?` | 2 | The query result |

### GetGitStorageAccountSummaries

| Property | Type | DataMember Order | Description |
|----------|------|------------------|-------------|
| `Skip` | `int` | 1 | Number of records to skip (pagination) |
| `Take` | `int` | 2 | Number of records to return |
| `Result` | `IEnumerable<GitStorageAccountSummaryViewModel>?` | 3 | The query result |

### GetGitStorageAccountIds

| Property | Type | DataMember Order | Description |
|----------|------|------------------|-------------|
| `Result` | `IEnumerable<string>?` | 1 | List of all account IDs |

---

## View Models (Projections)

### GitStorageAccountDetailsViewModel

| Property | Type | DataMember Order | Description |
|----------|------|------------------|-------------|
| `Id` | `string` | 1 | Account identifier |
| `Name` | `string` | 2 | Display name |
| `Comments` | `string?` | 3 | Optional description |
| `Disabled` | `bool` | 4 | Whether account is disabled |

### GitStorageAccountSummaryViewModel

| Property | Type | DataMember Order | Description |
|----------|------|------------------|-------------|
| `Id` | `string` | 1 | Account identifier |
| `Name` | `string` | 2 | Display name |
| `Disabled` | `bool` | 3 | Whether account is disabled |

---

## Relationships

```
                                    ┌─────────────────────────────┐
                                    │      Event Store            │
                                    │  (Azure Cosmos DB)          │
                                    └──────────────┬──────────────┘
                                                   │
                                                   │ Event Stream
                                                   ▼
┌─────────────────┐   sends    ┌─────────────────────────────────────┐   emits    ┌─────────────────┐
│    Commands     │ ─────────▶ │        GitStorageAccount            │ ─────────▶ │  Domain Events  │
│  (Add, Change,  │            │           Aggregate                 │            │ (Added, Changed,│
│  Enable,Disable)│            │                                     │            │ Enabled,Disabled)│
└─────────────────┘            └─────────────────────────────────────┘            └────────┬────────┘
                                                                                           │
                                                                                           │ Projects to
                                                                                           ▼
┌─────────────────┐   reads    ┌─────────────────────────────────────┐   updates  ┌─────────────────┐
│    Requests     │ ◀───────── │         Projection Handlers         │ ◀───────── │   Read Models   │
│  (GetDetails,   │            │                                     │            │  (Dapr State)   │
│  GetSummaries)  │            │                                     │            │                 │
└─────────────────┘            └─────────────────────────────────────┘            └─────────────────┘
```

---

## Event-to-Projection Mapping

| Domain Event | Projection Handler | Read Model Effect |
|--------------|-------------------|-------------------|
| `GitStorageAccountAdded` | `GitStorageAccountAddedOnDetailsProjectionHandler` | Create `DetailsViewModel` |
| `GitStorageAccountDescriptionChanged` | `GitStorageAccountDescriptionChangedOnDetailsProjectionHandler` | Update `Name`, `Comments` |
| `GitStorageAccountEnabled` | `GitStorageAccountEnabledOnDetailsProjectionHandler` | Set `Disabled = false` |
| `GitStorageAccountDisabled` | `GitStorageAccountDisabledOnDetailsProjectionHandler` | Set `Disabled = true` |

---

## Serialization Notes

- All aggregates, events, commands, requests use `[DataContract]` and `[DataMember(Order = N)]`
- Events and commands require `[PolymorphicSerialization]` for JSON polymorphic deserialization
- `DataMember` order starts at 1 for `Id`, increments sequentially
- Nullable properties (`string?`) serialize as `null` when not provided
