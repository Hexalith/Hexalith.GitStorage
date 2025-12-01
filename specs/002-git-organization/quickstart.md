# Quickstart: Git Organization Entity

**Feature**: 002-git-organization
**Date**: 2025-12-01

## Overview

This guide helps you quickly understand and work with the GitOrganization entity implementation.

## Key Concepts

### What is a GitOrganization?

A GitOrganization represents an organization on a remote git server (GitHub or Forgejo). Organizations can be:

1. **Synced** - Discovered from the remote server during a sync operation
2. **Created via Application** - Created through this application's API, which provisions it on the remote server

### Composite ID

Every GitOrganization has a deterministic ID: `{GitStorageAccountId}-{OrganizationName}` (lowercase).

Example: If the GitStorageAccount ID is `github-main` and the organization name is `MyOrg`, the GitOrganization ID is `github-main-myorg`.

## File Locations

### Domain Layer

```
src/libraries/Domain/
├── Hexalith.GitStorage.Events/GitOrganization/     # Domain events
├── Hexalith.GitStorage.Aggregates/                  # GitOrganization aggregate
└── Hexalith.GitStorage.Aggregates.Abstractions/     # Helper and enums
```

### Application Layer

```
src/libraries/Application/
├── Hexalith.GitStorage.Commands/GitOrganization/   # Commands
├── Hexalith.GitStorage.Requests/GitOrganization/   # Requests and ViewModels
└── Hexalith.GitStorage.Projections/                # Projection handlers
```

### Infrastructure Layer

```
src/libraries/Infrastructure/
├── Hexalith.GitStorage.Servers/Services/           # Service implementation
└── Hexalith.GitStorage.ApiServer/Controllers/      # API controllers
```

## Common Operations

### 1. Sync Organizations from Git Storage Account

```http
POST /api/v1/git-storage-accounts/{gitStorageAccountId}/sync-organizations
Authorization: Bearer {token}
```

This triggers discovery of all organizations accessible via the specified Git Storage Account.

### 2. Create New Organization

```http
POST /api/v1/git-organizations
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "my-new-org",
  "description": "My new organization",
  "gitStorageAccountId": "github-main"
}
```

This creates the organization both locally AND on the remote git server.

### 3. List Organizations

```http
GET /api/v1/git-organizations?gitStorageAccountId=github-main&skip=0&take=20
Authorization: Bearer {token}
```

### 4. Get Organization Details

```http
GET /api/v1/git-organizations/{id}
Authorization: Bearer {token}
```

### 5. Update Organization

```http
PATCH /api/v1/git-organizations/{id}
Authorization: Bearer {token}
Content-Type: application/json

{
  "description": "Updated description"
}
```

## Event Flow

### Sync Flow

```
[Trigger Sync] → SyncGitOrganizations command
                     ↓
              [Call IGitProviderAdapter.ListOrganizationsAsync()]
                     ↓
    ┌────────────────┴────────────────┐
    ↓                                 ↓
[New remote org]              [Missing local org]
    ↓                                 ↓
GitOrganizationSynced       GitOrganizationMarkedNotFound
    ↓                                 ↓
[Projection updates]          [Projection updates]
```

### Create Flow

```
[API: Create Org] → AddGitOrganization command
                          ↓
                    [Validate & Apply]
                          ↓
                    GitOrganizationAdded event
                          ↓
              ┌───────────┴───────────┐
              ↓                       ↓
       [Event Handler]          [Projection Handler]
              ↓                       ↓
   [IGitProviderAdapter.        [Update read models]
    CreateOrganizationAsync()]
```

## Testing

### Run Unit Tests

```bash
dotnet test tests/Hexalith.GitStorage.UnitTests/
```

### Key Test Files

- `GitOrganizationAggregateTests.cs` - Aggregate Apply method tests
- `GitOrganizationEventsSerializationTests.cs` - JSON round-trip tests
- `GitOrganizationCommandsValidationTests.cs` - Command validation tests

### Example Test Pattern

```csharp
[Fact]
public void Apply_GitOrganizationAdded_ShouldInitializeAggregate()
{
    // Arrange
    GitOrganization aggregate = new();
    GitOrganizationAdded added = new(
        "github-main-myorg",
        "myorg",
        "Description",
        "github-main");

    // Act
    ApplyResult result = aggregate.Apply(added);

    // Assert
    result.Failed.ShouldBeFalse();
    result.Aggregate.ShouldBeOfType<GitOrganization>()
        .Name.ShouldBe("myorg");
}
```

## Configuration

### Provider Setup

The `IGitProviderAdapter` is configured per Git Storage Account. The adapter type (GitHub or Forgejo) is determined by the account settings.

### Authorization

All GitOrganization operations require the **Admin** role.

## Troubleshooting

### Organization not syncing?

1. Check the Git Storage Account is enabled
2. Verify credentials are valid
3. Check Dapr logs for event handler errors

### Create fails with conflict?

The organization may already exist on the remote server. Try syncing first.

### SyncStatus shows SyncError?

The event handler failed to sync with remote. Dapr will retry automatically. Check dead-letter queue if persistent.

## Related Documentation

- [Specification](spec.md) - Full feature requirements
- [Data Model](data-model.md) - Entity and event definitions
- [API Contract](contracts/api.yaml) - OpenAPI specification
- [Research](research.md) - Design decisions and rationale
