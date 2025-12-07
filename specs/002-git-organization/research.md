# Research: GitOrganization Entity

**Feature**: 002-git-organization
**Date**: 2025-12-07
**Status**: Complete

## Executive Summary

This document captures architectural decisions and research findings for implementing the GitOrganization entity. All "NEEDS CLARIFICATION" items from the Technical Context have been resolved through spec analysis and codebase exploration.

## Architecture Decisions

### Decision 1: Composite Key Strategy

**Decision**: Use composite key format `{GitStorageAccountId}-{OrganizationName}` (lowercase normalized)

**Rationale**:

- Ensures natural uniqueness within account context (FR-011)
- Deterministic generation enables idempotent operations
- Matches existing pattern from GitStorageAccount implementation
- Lowercase normalization prevents case-sensitivity collisions

**Alternatives Considered**:

- GUID-based keys: Rejected - loses natural uniqueness semantics
- Sequential IDs: Rejected - not suitable for distributed systems
- Name-only keys: Rejected - organizations can have same name across accounts

**Implementation**:

```csharp
public static string GenerateId(string gitStorageAccountId, string organizationName)
    => $"{gitStorageAccountId}-{organizationName.ToLowerInvariant()}";
```

### Decision 2: Dual Initialization Events

**Decision**: Support two initialization events - `GitOrganizationAdded` (API create) and `GitOrganizationSynced` (remote sync)

**Rationale**:

- FR-005 requires tracking origin (synced vs. created via application)
- Different initialization paths carry different data (sync includes RemoteId, SyncedAt)
- Clean separation of concerns in event handlers

**Alternatives Considered**:

- Single event with nullable fields: Rejected - loses semantic clarity
- Separate aggregates per origin: Rejected - over-engineering

**Implementation**:

```csharp
// Applied only for API-created organizations
public partial record GitOrganizationAdded(Id, Name, Description, GitStorageAccountId)

// Applied only for synced organizations
public partial record GitOrganizationSynced(Id, Name, Description, GitStorageAccountId, RemoteId, SyncedAt)
```

### Decision 3: Sync Status Tracking

**Decision**: Use `GitOrganizationSyncStatus` enum with three states: `Synced`, `NotFoundOnRemote`, `SyncError`

**Rationale**:

- FR-006 requires tracking sync status
- Edge case handling for deleted remote organizations (flagged, not deleted per FR-014)
- Clear audit trail for troubleshooting

**Alternatives Considered**:

- Boolean flags (IsSynced, HasError): Rejected - doesn't capture full state machine
- String status: Rejected - loses type safety

### Decision 4: Soft-Delete Implementation

**Decision**: Use `Disabled` boolean flag with `GitOrganizationDisabled`/`GitOrganizationEnabled` events

**Rationale**:

- FR-014 prohibits hard-delete for audit purposes
- FR-015/FR-016 require reversible soft-delete
- Matches existing GitStorageAccount pattern

**Implementation**:

- Disabled organizations skip most event applications (except enable/disable)
- No remote propagation on disable (per FR-015)

### Decision 5: Remote Sync Architecture

**Decision**: Event handlers call `IGitProviderAdapter`; failures handled by Dapr retry infrastructure

**Rationale**:

- FR-013 specifies infrastructure-managed retry
- Decouples domain logic from remote communication concerns
- Enables testability via adapter mocking

**Flow**:

1. Command → Domain Event emitted
2. Event Handler invoked
3. Handler calls `IGitProviderAdapter.CreateOrganizationAsync()` / `UpdateOrganizationAsync()`
4. On failure: Dapr handles retry/dead-letter

### Decision 6: Organization Name Validation

**Decision**: Validate against git server naming rules (alphanumeric, hyphens, underscores; 1-39 chars)

**Rationale**:

- FR-003 requires validation against Git Server conventions
- GitHub/Forgejo share similar naming constraints
- Prevents remote API failures due to invalid names

**Regex Pattern**:

```csharp
// Allows: single char or char-middle-char pattern
// Middle can contain alphanumeric, hyphen, underscore
^[a-zA-Z0-9][a-zA-Z0-9_-]*[a-zA-Z0-9]$|^[a-zA-Z0-9]$
```

### Decision 7: Projection Strategy

**Decision**: Two projection types - Details (full data) and Summary (list view)

**Rationale**:

- CQRS pattern requires optimized read models
- List views need minimal data (performance)
- Detail views need full entity state

**ViewModels**:

- `GitOrganizationDetailsViewModel`: Full entity with denormalized GitStorageAccountName
- `GitOrganizationSummaryViewModel`: Id, Name, GitStorageAccountId, SyncStatus, Disabled

### Decision 8: Authorization Model

**Decision**: Reuse existing GitStorage roles (Owner, Contributor, Reader)

**Rationale**:

- FR-012 requires Admin role for all operations
- GitStorageOwner maps to Admin capability
- Consistent with feature 001 security model

**Mapping**:

- Sync operations: Owner only
- Create/Update/Delete: Owner, Contributor
- Read operations: Owner, Contributor, Reader

## Technology Research

### GitHub Organizations API

**Endpoint**: `GET /user/orgs` (list), `POST /user/orgs` (create)

**Key Findings**:

- Organization names must be 1-39 characters
- Names can contain alphanumeric, hyphen; cannot start/end with hyphen
- Creating organizations requires specific OAuth scopes

### Forgejo Organizations API

**Endpoint**: `GET /api/v1/user/orgs` (list), `POST /api/v1/admin/orgs` (create)

**Key Findings**:

- Similar naming constraints to GitHub
- Admin API required for organization creation
- Compatible with existing adapter interface

### Dapr Pub/Sub for Event Handling

**Pattern**: Topic-based subscription with automatic retry

**Configuration**:

- Dead-letter topic for failed messages
- Configurable retry policy (exponential backoff)
- At-least-once delivery semantics

### Decision 9: Visibility Property

**Decision**: Add `GitOrganizationVisibility` enum with values Public(0), Private(1), Internal(2)

**Rationale**:

- FR-018 requires tracking organization visibility
- GitHub and Forgejo both support visibility settings
- Visibility can be set at creation and changed independently
- Follows existing enum patterns (GitOrganizationOrigin, GitOrganizationSyncStatus)

**API Mapping**:

| GitOrganizationVisibility | GitHub | Forgejo |
|---------------------------|--------|---------|
| Public | `public` | `public` |
| Private | `private` | `private` |
| Internal | `internal` | `limited` |

**Implementation**:

```csharp
public enum GitOrganizationVisibility
{
    Public = 0,
    Private = 1,
    Internal = 2,
}
```

**Event Impact**:

- `GitOrganizationAdded`: Add Visibility parameter (Order = 5)
- `GitOrganizationSynced`: Add Visibility parameter (Order = 5)
- `GitOrganizationVisibilityChanged`: New event for visibility changes

**Default Value**: `Public` for backward compatibility with existing events

### Decision 10: DataMember Order Strategy for Visibility

**Decision**: Insert Visibility at Order = 5 in events, maintaining logical grouping

**Rationale**:

- Logical grouping: Id, Name, Description, GitStorageAccountId, Visibility, then sync-related fields
- Consistent positioning across all affected records
- Higher-order fields shift up (RemoteId, SyncedAt, etc.)

**Affected Records**:

| Record | Visibility Order | Notes |
|--------|------------------|-------|
| GitOrganizationAdded | 5 | New parameter after GitStorageAccountId |
| GitOrganizationSynced | 5 | RemoteId→6, SyncedAt→7 |
| GitOrganization (aggregate) | 5 | RemoteId→7, SyncStatus→8, etc. |
| GitOrganizationDetailsViewModel | 6 | After GitStorageAccountName |

## Open Items

None - all clarifications resolved in spec sessions 2025-12-01 and 2025-12-07.

## References

- [Feature Specification](spec.md)
- [Constitution](../../.specify/memory/constitution.md)
- [GitStorageAccount Implementation](../../src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitStorage.cs)
- [GitHub Organizations API](https://docs.github.com/en/rest/orgs)
- [Forgejo API Documentation](https://forgejo.org/docs/latest/developer/api-usage/)
