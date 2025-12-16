# Feature Specification: GitRepository Aggregate

**Feature Branch**: `004-git-repository`
**Created**: 2025-12-15
**Status**: Draft
**Input**: User description: "add the GitRepository aggregates that manages Git repositories"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Create a Git Repository (Priority: P1)

As a system administrator, I want to create a new Git repository through this application so that the repository is automatically created on the remote Git Server (GitHub or Forgejo) and tracked locally.

**Why this priority**: This is the foundational capability - without the ability to create repositories, no other repository management features can function.

**Independent Test**: Can be fully tested by creating a repository with required details (name, organization, visibility) and verifying it appears both locally and on the remote Git Server.

**Acceptance Scenarios**:

1. **Given** a valid, enabled organization exists, **When** an administrator creates a new repository with name, visibility, and optional description, **Then** the repository is created on the remote Git Server AND stored locally with a unique identifier
2. **Given** a repository creation request, **When** a repository with the same name already exists in the organization on the remote server, **Then** the system rejects the creation with a clear error indicating the conflict
3. **Given** a repository creation request, **When** the remote Git Server is unreachable, **Then** the system returns an error without creating a local record (maintains consistency)
4. **Given** a repository creation request, **When** the organization does not exist or is disabled, **Then** the system rejects the request with an appropriate error

---

### User Story 2 - Update Repository Description (Priority: P2)

As a repository manager, I want to update the description of a registered repository so that I can provide accurate context about the repository's purpose.

**Why this priority**: Repository metadata management is essential for organization and discovery, but secondary to basic registration.

**Independent Test**: Can be fully tested by changing a repository's description and verifying the update persists and is retrievable.

**Acceptance Scenarios**:

1. **Given** an existing repository, **When** a user updates the description, **Then** the new description is saved and visible
2. **Given** an existing repository, **When** the description is cleared (set to empty), **Then** the repository shows no description

---

### User Story 3 - Change Repository Visibility (Priority: P2)

As a repository owner, I want to change the visibility of a repository (Public, Private, or Internal) so that I can control who can see and access the repository.

**Why this priority**: Visibility control is a core security feature that determines repository access, essential for proper repository governance.

**Independent Test**: Can be fully tested by changing a repository's visibility setting and verifying the change is reflected in the system.

**Acceptance Scenarios**:

1. **Given** an existing repository with Public visibility, **When** the owner changes visibility to Private, **Then** the repository visibility is updated to Private
2. **Given** an existing repository, **When** visibility is changed to Internal, **Then** the repository is visible only to organization members
3. **Given** a repository visibility change request, **When** the visibility value is invalid, **Then** the system rejects the request with a validation error

---

### User Story 4 - Enable or Disable a Repository (Priority: P2)

As a system administrator, I want to enable or disable a repository so that I can temporarily restrict access without permanently deleting it.

**Why this priority**: Lifecycle management allows administrators to control repository availability without data loss.

**Independent Test**: Can be fully tested by disabling a repository and verifying it is marked as disabled, then re-enabling it.

**Acceptance Scenarios**:

1. **Given** an enabled repository, **When** an administrator disables it, **Then** the repository is marked as disabled
2. **Given** a disabled repository, **When** an administrator enables it, **Then** the repository is marked as enabled
3. **Given** a repository that is already disabled, **When** a disable request is made, **Then** the system handles idempotently (no error, state unchanged)

---

### User Story 5 - Synchronize Repository Metadata (Priority: P3)

As a system operator, I want to trigger synchronization of repository metadata from the source Git provider so that local records reflect the current state of the remote repository.

**Why this priority**: Sync capability ensures data consistency with external systems but is operational maintenance rather than core functionality.

**Independent Test**: Can be fully tested by triggering a sync operation and verifying metadata (e.g., last sync timestamp) is updated.

**Acceptance Scenarios**:

1. **Given** an existing repository with local changes, **When** a sync operation is triggered, **Then** local metadata changes are pushed to the remote AND remote changes are pulled to local
2. **Given** a sync request for a non-existent repository, **When** the sync is attempted, **Then** the system returns an appropriate error
3. **Given** conflicting changes on local and remote, **When** sync is triggered, **Then** the system resolves conflicts (remote wins for same-field conflicts)

---

### User Story 6 - Change Default Branch (Priority: P3)

As a repository manager, I want to change the default branch of a repository so that the system correctly references the primary branch for operations.

**Why this priority**: Default branch management is important but typically set once during registration and rarely changed.

**Independent Test**: Can be fully tested by changing the default branch name and verifying the update persists.

**Acceptance Scenarios**:

1. **Given** an existing repository with default branch "main", **When** the default branch is changed to "develop", **Then** the repository reflects the new default branch
2. **Given** a request to change default branch, **When** the branch name is empty, **Then** the system rejects the request with a validation error

---

### Edge Cases

- What happens when registering a repository with a URL that already exists in the system? → Reject duplicate URL within the same organization; allow the same URL in different organizations.
- How does the system handle repository synchronization when the external source is unavailable? → Sync fails gracefully with clear error; existing local repositories remain unchanged.
- What happens when attempting to change visibility of a disabled repository? → Allowed; visibility change is persisted but may not propagate to remote until re-enabled.
- How are concurrent update operations on the same repository handled? → Optimistic concurrency; later operation fails if aggregate version mismatch.
- What happens if remote repository creation succeeds but local storage fails? → Succeed silently; repository exists on remote but not tracked locally. User must re-sync to discover it.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST allow creation of a new Git repository via the application API, which creates the repository on the remote Git Server (GitHub or Forgejo) and stores it locally
- **FR-002**: System MUST validate repository URL format during registration
- **FR-003**: System MUST assign a unique identifier to each registered repository
- **FR-004**: System MUST support updating repository description
- **FR-005**: System MUST support changing repository visibility (Public, Private, Internal)
- **FR-006**: System MUST support enabling and disabling repositories
- **FR-007**: System MUST support triggering repository metadata synchronization (two-way: local changes propagate to remote, remote changes update local)
- **FR-008**: System MUST support changing the default branch of a repository
- **FR-009**: System MUST track the enabled/disabled state of each repository
- **FR-010**: System MUST track the visibility setting of each repository
- **FR-011**: System MUST associate each repository with exactly one organization
- **FR-012**: System MUST maintain audit information (creation timestamp, last modified). *Implementation Note*: `CreatedAt` is captured in `GitRepositoryAdded` event timestamp. `LastModifiedAt` is derived from the most recent event timestamp in the aggregate's event stream per Hexalith event sourcing conventions.
- **FR-013**: System MUST validate all command inputs before processing
- **FR-014**: System MUST allow synchronization of repositories from a GitOrganization to the local database
- **FR-015**: System MUST track the origin of each repository (Synced | CreatedViaApplication)
- **FR-016**: System MUST track sync status for each repository (last synced timestamp, sync errors if any)
- **FR-017**: System MUST NOT hard-delete repositories; repositories removed from remote are flagged but retained for audit purposes
- **FR-018**: System MUST provide Blazor UI pages for GitRepository management: list view, details view, create form, and edit form
- **FR-019**: System MUST prevent creation of duplicate repositories (same name or URL within the same GitOrganization)
- **FR-020**: System MUST emit domain events when repositories are synced, created, updated, or flagged as removed from remote
- **FR-021**: System MUST gracefully handle remote Git Server unavailability; event handlers fail and retry is delegated to the messaging infrastructure (Dapr)

### Out of Scope

- Hard deletion of repositories from the remote Git Server via this application
- Automatic scheduled synchronization (deferred to future enhancement)
- Repository content management (commits, branches, pull requests) - this feature only manages repository metadata
- Repository access control / permissions management on the remote server

### Key Entities

- **GitRepository**: The core aggregate representing a Git repository. Key attributes include:
  - **Id**: Composite key `{OrganizationId}-{RepositoryName}` (deterministic, naturally unique within organization context)
  - **Name**: Repository name as it appears on the Git Server
  - **Url**: The repository URL (clone URL)
  - **Description**: Optional description of the repository
  - **Visibility**: Repository visibility level (Public | Private | Internal)
  - **DefaultBranch**: The default branch name (e.g., "main", "master")
  - **OrganizationId**: Reference to the parent GitOrganization
  - **Origin**: Indicator of how the repository was added (Synced | CreatedViaApplication)
  - **RemoteId**: The repository's unique identifier on the remote Git Server (if available)
  - **SyncStatus**: Current synchronization state (Synced | NotFoundOnRemote | SyncError)
  - **LastSyncedAt**: Timestamp of the last successful sync
  - **Disabled**: Boolean flag indicating whether the repository is disabled locally
- **RepositoryVisibility**: A value object representing the visibility level (Public, Private, Internal) that determines access scope.
- **GitOrganization** (existing): The parent organization that owns repositories. Each repository belongs to one organization.

## Clarifications

### Session 2025-12-15

- Q: How should GitRepository Id be generated? → A: Composite key `{OrganizationId}-{RepositoryName}` (deterministic, naturally unique within organization context).
- Q: Should GitRepository support synchronization from the remote Git server? → A: Yes, sync from remote (discover repositories from organization, track Origin: Synced vs CreatedViaApplication).
- Q: Is Blazor UI included in this feature? → A: Yes, include UI (list, details, create, edit pages).
- Q: How should duplicate repository URL registration be handled? → A: Reject duplicate URL within same organization only (allow same URL in different organizations).
- Q: Should creating a repository via the application also create it on the remote Git server? → A: Yes, create on remote (application creates repository on GitHub/Forgejo AND stores locally).
- Q: What happens if remote creation succeeds but local storage fails? → A: Succeed silently (repository exists on remote, not tracked locally - user must re-sync later).
- Q: Is sync one-way or two-way? → A: Two-way sync (local changes propagate to remote, remote changes update local).

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Repository creation API responds within 5 seconds (excluding remote Git server latency); end-to-end user workflow completes within 30 seconds including UI interaction
- **SC-002**: Repository visibility changes take effect immediately upon confirmation
- **SC-003**: All repository operations (register, update, enable/disable) complete within 2 seconds at baseline load (≤10 concurrent requests per second, ≤1000 repositories per organization)
- **SC-004**: 100% of invalid input requests are rejected with clear, actionable error messages
- **SC-005**: Repository metadata accurately reflects the configured values after any update operation
