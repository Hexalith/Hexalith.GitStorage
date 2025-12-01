# Research: Git Organization Entity

**Feature**: 002-git-organization
**Date**: 2025-12-01

## Research Areas

### 1. GitHub Organizations API

**Decision**: Use GitHub REST API v3 for organization management

**Rationale**:
- Well-documented, stable API with comprehensive organization endpoints
- Supports both listing user's organizations and creating new organizations
- REST API is simpler for basic CRUD operations than GraphQL
- Octokit.NET library available for .NET integration

**Key Endpoints**:
- `GET /user/orgs` - List organizations for authenticated user
- `GET /orgs/{org}` - Get organization details
- `POST /orgs` - Create organization (requires admin/owner permissions)
- `PATCH /orgs/{org}` - Update organization

**Alternatives Considered**:
- GraphQL API: More flexible but adds complexity for simple operations
- Direct HTTP: Octokit handles auth, rate limiting, pagination

**Constraints**:
- Organization creation requires GitHub Enterprise or special permissions
- Rate limiting: 5,000 requests/hour for authenticated requests
- Some operations require organization owner/admin role

---

### 2. Forgejo Organizations API

**Decision**: Use Forgejo/Gitea REST API v1 for organization management

**Rationale**:
- Forgejo is a Gitea fork with compatible API
- Self-hosted instances have no rate limits
- API structure similar to GitHub for easier adapter development

**Key Endpoints**:
- `GET /api/v1/user/orgs` - List user's organizations
- `GET /api/v1/orgs/{org}` - Get organization details
- `POST /api/v1/orgs` - Create organization
- `PATCH /api/v1/orgs/{org}` - Update organization

**Alternatives Considered**:
- Direct database access: Not portable, violates provider abstraction
- GraphQL: Not available in Forgejo

**Constraints**:
- API authentication via token or basic auth
- Organization names must be valid (alphanumeric, hyphens, underscores)

---

### 3. Provider Adapter Pattern

**Decision**: Implement `IGitProviderAdapter` interface with GitHub and Forgejo implementations

**Rationale**:
- Constitution Principle VII requires provider abstraction
- Enables testing with mock implementations
- Supports multiple accounts with different provider types
- Clean separation between domain logic and external API calls

**Interface Design**:
```csharp
public interface IGitProviderAdapter
{
    Task<IEnumerable<GitOrganizationDto>> ListOrganizationsAsync(CancellationToken ct);
    Task<GitOrganizationDto> GetOrganizationAsync(string name, CancellationToken ct);
    Task<GitOrganizationDto> CreateOrganizationAsync(string name, string? description, CancellationToken ct);
    Task<GitOrganizationDto> UpdateOrganizationAsync(string name, string? description, CancellationToken ct);
}
```

**Alternatives Considered**:
- Single generic HTTP client: Loses type safety, harder to test
- Strategy pattern per operation: Over-engineered for current scope

---

### 4. Composite ID Generation

**Decision**: Generate ID as `{GitStorageAccountId}-{OrganizationName}` (lowercase, normalized)

**Rationale**:
- Per clarification session: deterministic, naturally unique within account context
- No collision between organizations with same name on different accounts
- Easy to construct from available data during sync
- Human-readable for debugging

**Implementation**:
```csharp
public static string GenerateId(string gitStorageAccountId, string organizationName)
    => $"{gitStorageAccountId}-{organizationName.ToLowerInvariant()}";
```

**Alternatives Considered**:
- GUID: Not deterministic, harder to correlate across syncs
- User-provided: Requires additional uniqueness validation

---

### 5. Sync Strategy

**Decision**: On-demand sync via `SyncGitOrganizations` command targeting a single GitStorageAccount

**Rationale**:
- Per spec assumptions: manual trigger, not scheduled
- Command retrieves all orgs from remote, compares with local state
- Emits events for new orgs (GitOrganizationSynced), flags missing orgs (GitOrganizationMarkedNotFound)
- Batch processing within single command execution

**Flow**:
1. Command handler calls `IGitProviderAdapter.ListOrganizationsAsync()`
2. For each remote org not in local: emit `GitOrganizationSynced` event
3. For each local org not in remote: emit `GitOrganizationMarkedNotFound` event
4. Update `LastSyncedAt` timestamp

**Alternatives Considered**:
- Individual commands per org: Too many events, performance issue
- Background job: Not requested in current scope

---

### 6. Event-Driven Remote Sync (Create/Update)

**Decision**: Event handlers listen for domain events and sync to remote via adapter

**Rationale**:
- Per clarification: events trigger handlers that sync to remote
- Decouples domain logic from infrastructure concerns
- Dapr handles retry on failure (per clarification)

**Event Handler Mapping**:
- `GitOrganizationAdded` → Create on remote (if origin = CreatedViaApplication)
- `GitOrganizationDescriptionChanged` → Update on remote

**Alternatives Considered**:
- Synchronous API calls in command handler: Violates CQRS, blocks command execution
- Saga pattern: Over-engineered for current requirements

---

### 7. Organization Name Validation

**Decision**: Validate against common Git server naming rules before accepting commands

**Rationale**:
- GitHub and Forgejo have similar naming constraints
- Fail fast before attempting remote API call
- Consistent validation across providers

**Rules**:
- Alphanumeric characters, hyphens, underscores only
- Cannot start or end with hyphen
- Length: 1-39 characters (GitHub limit)
- Case-insensitive uniqueness

**Implementation**: `GitOrganizationValidator` in Domain layer

---

### 8. Error Handling Strategy

**Decision**: Event handlers throw on failure; Dapr messaging retries automatically

**Rationale**:
- Per clarification: infrastructure-managed retry
- Dapr pub/sub has built-in retry policies and dead-letter topics
- No application-level retry logic needed

**SyncStatus States**:
- `Synced`: Successfully synced with remote
- `NotFoundOnRemote`: Exists locally but not found during sync
- `SyncError`: Remote operation failed (set by projection based on event pattern)

---

## Dependencies

| Dependency | Purpose | NuGet Package |
|------------|---------|---------------|
| Octokit | GitHub API client | Octokit |
| HttpClient | Forgejo API calls | Built-in |
| System.Text.Json | API response parsing | Built-in |

## Resolved Unknowns

All NEEDS CLARIFICATION items from Technical Context have been resolved through this research:

1. ✅ Provider abstraction pattern defined
2. ✅ API endpoints for both GitHub and Forgejo identified
3. ✅ Composite ID generation strategy confirmed
4. ✅ Sync strategy determined (on-demand, batch)
5. ✅ Error handling delegated to infrastructure
