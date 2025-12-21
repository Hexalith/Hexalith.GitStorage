# Research: GitRepository Aggregate

**Feature**: 004-git-repository
**Date**: 2025-12-15
**Status**: Complete

## Research Tasks

### 1. Composite Key Pattern for GitRepository Id

**Decision**: Use composite key `{OrganizationId}-{RepositoryName}`

**Rationale**:
- Follows the established pattern from GitOrganization (`{GitStorageAccountId}-{OrganizationName}`)
- Ensures natural uniqueness within the organization context
- Enables deterministic ID generation without UUID dependencies
- Supports efficient lookups by organization

**Alternatives Considered**:
- GUID: Rejected because it doesn't provide semantic meaning and requires separate indexes for organization lookups
- Repository URL hash: Rejected because URLs can change and the same repository could have multiple URLs (HTTPS, SSH)
- Remote ID from Git server: Rejected because it's provider-specific and not available until after remote creation

### 2. Event Sourcing Patterns in Existing Codebase

**Decision**: Follow GitOrganization event patterns exactly

**Findings**:
- All events inherit from abstract base class (`GitOrganizationEvent`)
- Events use `[PolymorphicSerialization]` attribute for polymorphic deserialization
- Events use `[DataContract]` and `[DataMember(Order = N)]` for deterministic serialization
- Event validators use FluentValidation with localized messages
- Aggregates have two initialization paths: direct creation (Added) and sync (Synced)

**Patterns to Replicate**:
```csharp
// Base event pattern
[PolymorphicSerialization]
public abstract partial record GitRepositoryEvent([property: DataMember(Order = 1)] string Id)
    : IPolymorphicSerializable
{
    public string AggregateId => Id;
    public static string AggregateName => "GitRepository";
}
```

### 3. Two-Way Sync Implementation Strategy

**Decision**: Implement sync as command-triggered operation with event-based state tracking

**Rationale**:
- `SyncGitRepository` command triggers sync operation
- Sync handler queries remote Git server via provider adapter
- Results emit appropriate events:
  - `GitRepositorySynced` - Successful sync with updated metadata
  - `GitRepositoryMarkedNotFound` - Repository no longer exists on remote
- Remote changes always win for same-field conflicts (as per spec clarification)

**Implementation Approach**:
1. Command handler receives `SyncGitRepository` command
2. Handler retrieves repository details from remote via `IGitProviderAdapter`
3. Handler compares remote state with local state
4. Handler emits appropriate events based on differences
5. For local-to-remote sync: separate handler publishes changes to remote

**Conflict Resolution**:
- Remote wins for metadata fields (name, description, visibility, default branch)
- Local-only fields preserved (Disabled flag, Origin)

### 4. Repository Creation on Remote Git Server

**Decision**: Create on remote first, then store locally on success

**Rationale**:
- Ensures consistency: remote is source of truth
- If remote creation fails, no orphaned local records
- If local storage fails after remote success, repository can be discovered via sync

**Flow**:
1. `AddGitRepository` command received
2. Command handler calls `IGitProviderAdapter.CreateRepositoryAsync()`
3. On success, emit `GitRepositoryAdded` event with RemoteId
4. On failure, throw exception (no local record created)

**Edge Cases**:
- Remote succeeds but local fails: Repository exists on remote, not tracked locally. User must sync to discover.
- Duplicate name on remote: Remote API returns error, command handler rejects with clear message.

### 5. Visibility Enum Mapping

**Decision**: Reuse existing `GitOrganizationVisibility` enum pattern for `GitRepositoryVisibility`

**Mapping**:
| Visibility | GitHub | Forgejo/Gitea |
|------------|--------|---------------|
| Public | public | public |
| Private | private | private |
| Internal | internal | limited |

**Note**: Internal visibility is only supported by GitHub Enterprise and Forgejo. For standard GitHub.com, Internal maps to Private.

### 6. Repository Name Validation Rules

**Decision**: Match GitHub/Forgejo repository naming constraints

**Rules**:
- Length: 1-100 characters
- Allowed characters: alphanumeric, hyphens, underscores, periods
- Cannot start with period
- Cannot end with `.git`
- Cannot contain consecutive periods
- Case-insensitive uniqueness within organization

**Regex Pattern**: `^(?!\.)(?!.*\.\.)[a-zA-Z0-9._-]{1,100}(?<!\.git)$`

### 7. URL Format for Repository

**Decision**: Store canonical HTTPS clone URL

**Format**: `https://{server}/{organization}/{repository}.git`

**Rationale**:
- HTTPS URLs are universally accessible
- SSH URLs require key configuration
- Canonical format enables consistent lookups

**Validation**: Must be valid HTTPS URL with `.git` suffix (optional but recommended)

### 8. Blazor UI Component Patterns

**Decision**: Follow existing `HexEntityIndexPage` and `HexEntityDetailsPage` patterns

**Findings from GitOrganization UI**:
- Index page uses `FluentDataGrid` with summary view models
- Details page uses `FluentStack` layout with `FluentTextField` inputs
- Edit view models implement validation via FluentValidation
- Save operations dispatch appropriate commands via `ICommandBus`

**Components to Create**:
- `GitRepositoryIndex.razor` - List with grid, search, enable/disable toggles
- `GitRepositoryDetails.razor` - Create/edit form with validation

### 9. Projection Handler Pattern

**Decision**: Use generic base handlers with concrete implementations per event

**Pattern**:
```csharp
public abstract class GitRepositorySummaryProjectionHandler<TEvent>
    : KeyValueProjectionUpdateEventHandlerBase<TEvent, GitRepositorySummaryViewModel>
    where TEvent : GitRepositoryEvent
{
    protected abstract ValueTask<GitRepositorySummaryViewModel> ApplyEventAsync(
        TEvent ev,
        GitRepositorySummaryViewModel? currentValue,
        CancellationToken ct);
}
```

**Concrete Handlers Required**:
- `GitRepositoryAddedOnSummaryProjectionHandler`
- `GitRepositorySyncedOnSummaryProjectionHandler`
- `GitRepositoryDescriptionChangedOnSummaryProjectionHandler`
- `GitRepositoryVisibilityChangedOnSummaryProjectionHandler`
- `GitRepositoryDefaultBranchChangedOnSummaryProjectionHandler`
- `GitRepositoryDisabledOnSummaryProjectionHandler`
- `GitRepositoryEnabledOnSummaryProjectionHandler`
- `GitRepositoryMarkedNotFoundOnSummaryProjectionHandler`
- Similar set for Details projections

### 10. API Controller Pattern

**Decision**: Follow existing `EventIntegrationController` pattern with Dapr PubSub

**Configuration**:
- Route: `POST /api/GitStorage/events/GitRepository`
- Dapr Topic: GitRepository events
- Session metadata for ordered processing

## Summary

All technical unknowns have been resolved. The GitRepository implementation will follow established patterns from GitOrganization and GitStorageAccount aggregates, ensuring consistency across the module.

**Key Implementation Decisions**:
1. Composite key: `{OrganizationId}-{RepositoryName}`
2. Remote-first creation with sync-based discovery
3. Two-way sync with remote-wins conflict resolution
4. Reuse visibility enum pattern with provider-specific mapping
5. Follow existing UI component patterns (HexEntityIndexPage, HexEntityDetailsPage)
6. Implement projection handlers per event type
