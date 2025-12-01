# Data Model: Git Organization Entity

**Feature**: 002-git-organization
**Date**: 2025-12-01

## Entities

### GitOrganization (Aggregate Root)

The primary entity representing an organization on a git server (GitHub or Forgejo).

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| Id | string | Yes | Composite key: `{GitStorageAccountId}-{OrganizationName}` (lowercase) |
| Name | string | Yes | Organization name as it appears on the Git Server |
| Description | string | No | Optional description of the organization |
| GitStorageAccountId | string | Yes | Reference to the parent GitStorageAccount entity |
| Origin | GitOrganizationOrigin | Yes | How the organization was added: `Synced` or `CreatedViaApplication` |
| RemoteId | string | No | The organization's unique identifier on the remote Git Server (if available) |
| SyncStatus | GitOrganizationSyncStatus | Yes | Current synchronization state |
| LastSyncedAt | DateTimeOffset | No | Timestamp of the last successful sync |
| Disabled | bool | Yes | Whether the organization is suspended locally (default: false) |

**Validation Rules**:
- Id: Auto-generated from GitStorageAccountId and Name; immutable after creation
- Name: 1-39 characters, alphanumeric/hyphens/underscores only, cannot start/end with hyphen
- GitStorageAccountId: Must reference an existing, enabled GitStorageAccount
- Origin: Set on creation, immutable thereafter

**State Transitions**:
```
[New] --GitOrganizationAdded--> [Active, Origin=CreatedViaApplication, SyncStatus=Synced]
[New] --GitOrganizationSynced--> [Active, Origin=Synced, SyncStatus=Synced]
[Active] --GitOrganizationDescriptionChanged--> [Active]
[Active] --GitOrganizationMarkedNotFound--> [Active, SyncStatus=NotFoundOnRemote]
[Active] --GitOrganizationSynced--> [Active, SyncStatus=Synced]
[Active] --GitOrganizationDisabled--> [Disabled]
[Disabled] --GitOrganizationEnabled--> [Active]
```

---

## Enumerations

### GitOrganizationOrigin

Indicates how the organization was added to the system.

| Value | Description |
|-------|-------------|
| Synced | Discovered from remote Git Server during sync operation |
| CreatedViaApplication | Created through the application API and provisioned on remote |

### GitOrganizationSyncStatus

Indicates the current synchronization state with the remote Git Server.

| Value | Description |
|-------|-------------|
| Synced | Successfully synchronized with remote; organization exists on both local and remote |
| NotFoundOnRemote | Organization exists locally but was not found during last sync (may have been deleted remotely) |
| SyncError | Remote operation failed; awaiting retry (set by event handler failure) |

---

## Domain Events

All events inherit from `GitOrganizationEvent` base record.

### GitOrganizationEvent (Base)

| Field | Type | Order | Description |
|-------|------|-------|-------------|
| Id | string | 1 | The GitOrganization aggregate Id |

### GitOrganizationAdded

Emitted when a new organization is created via the application API.

| Field | Type | Order | Description |
|-------|------|-------|-------------|
| Id | string | 1 | Composite key |
| Name | string | 2 | Organization name |
| Description | string? | 3 | Optional description |
| GitStorageAccountId | string | 4 | Parent account reference |

### GitOrganizationSynced

Emitted when an organization is discovered or re-synced from the remote server.

| Field | Type | Order | Description |
|-------|------|-------|-------------|
| Id | string | 1 | Composite key |
| Name | string | 2 | Organization name from remote |
| Description | string? | 3 | Description from remote |
| GitStorageAccountId | string | 4 | Parent account reference |
| RemoteId | string? | 5 | Remote server's unique identifier |
| SyncedAt | DateTimeOffset | 6 | Timestamp of sync operation |

### GitOrganizationDescriptionChanged

Emitted when the organization description is updated.

| Field | Type | Order | Description |
|-------|------|-------|-------------|
| Id | string | 1 | Composite key |
| Name | string | 2 | Updated name (or same) |
| Description | string? | 3 | New description |

### GitOrganizationMarkedNotFound

Emitted when an organization is not found on the remote during sync.

| Field | Type | Order | Description |
|-------|------|-------|-------------|
| Id | string | 1 | Composite key |
| MarkedAt | DateTimeOffset | 2 | When the org was flagged |

### GitOrganizationDisabled

Emitted when an organization is disabled locally.

| Field | Type | Order | Description |
|-------|------|-------|-------------|
| Id | string | 1 | Composite key |

### GitOrganizationEnabled

Emitted when a disabled organization is re-enabled.

| Field | Type | Order | Description |
|-------|------|-------|-------------|
| Id | string | 1 | Composite key |

---

## Commands

All commands inherit from `GitOrganizationCommand` base record.

### AddGitOrganization

Creates a new organization locally AND on the remote Git Server.

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| Id | string | Yes | Composite key (pre-generated by caller) |
| Name | string | Yes | Organization name |
| Description | string | No | Optional description |
| GitStorageAccountId | string | Yes | Parent account reference |

**Validation**: Name format, GitStorageAccountId exists and is enabled

### SyncGitOrganizations

Triggers synchronization of organizations from a GitStorageAccount.

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| GitStorageAccountId | string | Yes | Account to sync from |

**Note**: This is a bulk operation command. Handler retrieves all remote orgs and emits individual events.

### ChangeGitOrganizationDescription

Updates the organization description.

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| Id | string | Yes | Composite key |
| Name | string | Yes | Organization name (may change) |
| Description | string | No | New description |

### DisableGitOrganization

Disables an organization locally.

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| Id | string | Yes | Composite key |

### EnableGitOrganization

Enables a previously disabled organization.

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| Id | string | Yes | Composite key |

---

## Requests (Queries)

### GetGitOrganizationDetails

Retrieves full details for a single organization.

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| Id | string | Yes | Composite key |
| Result | GitOrganizationDetailsViewModel? | No | Populated by handler |

### GetGitOrganizationSummaries

Retrieves a paginated list of organization summaries.

| Field | Type | Required | Description |
|-------|------|----------|-------------|
| GitStorageAccountId | string | No | Filter by account (optional) |
| Skip | int | No | Pagination offset |
| Take | int | No | Page size |
| Result | IEnumerable<GitOrganizationSummaryViewModel>? | No | Populated by handler |

---

## View Models (Projections)

### GitOrganizationDetailsViewModel

| Field | Type | Description |
|-------|------|-------------|
| Id | string | Composite key |
| Name | string | Organization name |
| Description | string? | Optional description |
| GitStorageAccountId | string | Parent account reference |
| GitStorageAccountName | string | Parent account name (denormalized) |
| Origin | GitOrganizationOrigin | How added |
| RemoteId | string? | Remote identifier |
| SyncStatus | GitOrganizationSyncStatus | Current sync state |
| LastSyncedAt | DateTimeOffset? | Last sync timestamp |
| Disabled | bool | Whether disabled |

### GitOrganizationSummaryViewModel

| Field | Type | Description |
|-------|------|-------------|
| Id | string | Composite key |
| Name | string | Organization name |
| GitStorageAccountId | string | Parent account reference |
| SyncStatus | GitOrganizationSyncStatus | Current sync state |
| Disabled | bool | Whether disabled |

---

## Relationships

```
GitStorageAccount (1) ----< GitOrganization (*)
     │
     │ GitStorageAccountId
     ▼
GitOrganization
```

- Each GitOrganization belongs to exactly one GitStorageAccount
- A GitStorageAccount can have many GitOrganizations
- Deleting/disabling a GitStorageAccount does not cascade to GitOrganizations (they become orphaned but preserved)
