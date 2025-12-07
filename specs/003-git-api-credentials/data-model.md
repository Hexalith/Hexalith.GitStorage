# Data Model: Git API Credentials

**Feature**: 003-git-api-credentials
**Date**: 2025-12-07
**Status**: Complete

---

## Entity Extensions

### GitStorageAccount Aggregate (Extended)

**Location**: `src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitStorageAccount.cs`

| Field | Type | DataMember Order | Description | Validation |
|-------|------|------------------|-------------|------------|
| Id | string | 1 | Primary identifier | Required, non-empty |
| Name | string | 2 | Display name | Required, non-empty |
| Comments | string? | 3 | Optional description | Max 1000 chars |
| Disabled | bool | 7 | Soft-delete flag | - |
| **ServerUrl** | string? | 8 | **NEW**: API base URL | Valid HTTPS URL when provided |
| **AccessToken** | string? | 9 | **NEW**: Authentication token | Non-empty when provided |
| **ProviderType** | GitServerProviderType? | 10 | **NEW**: Server platform type | Valid enum value |

**Business Rules**:
- API credentials are optional - accounts can exist without them
- When `AccessToken` is provided, `ServerUrl` must also be provided
- When `ServerUrl` is provided without `AccessToken`, credentials are considered incomplete
- `ProviderType` defaults to `GitHub` when credentials are provided but type is unspecified

---

## New Enumeration

### GitServerProviderType

**Location**: `src/libraries/Domain/Hexalith.GitStorage.Aggregates.Abstractions/Enums/GitServerProviderType.cs`

| Value | Integer | Description |
|-------|---------|-------------|
| GitHub | 0 | GitHub.com or GitHub Enterprise |
| Forgejo | 1 | Forgejo instances |
| Gitea | 2 | Gitea instances |
| Generic | 3 | Other compatible Git servers |

---

## New Domain Events

### GitStorageAccountApiCredentialsChanged

**Location**: `src/libraries/Domain/Hexalith.GitStorage.Events/GitStorageAccount/GitStorageAccountApiCredentialsChanged.cs`

| Field | Type | DataMember Order | Description |
|-------|------|------------------|-------------|
| Id | string | 1 (inherited) | Aggregate identifier |
| ServerUrl | string | 2 | API base URL (validated HTTPS) |
| AccessToken | string | 3 | Authentication token |
| ProviderType | GitServerProviderType | 4 | Server platform type |

**Trigger**: `ChangeGitStorageAccountApiCredentials` command
**Effect**: Updates aggregate's API credential fields

---

### GitStorageAccountApiCredentialsCleared

**Location**: `src/libraries/Domain/Hexalith.GitStorage.Events/GitStorageAccount/GitStorageAccountApiCredentialsCleared.cs`

| Field | Type | DataMember Order | Description |
|-------|------|------------------|-------------|
| Id | string | 1 (inherited) | Aggregate identifier |

**Trigger**: `ClearGitStorageAccountApiCredentials` command
**Effect**: Sets ServerUrl, AccessToken, ProviderType to null

---

## Extended Domain Events

### GitStorageAccountAdded (Extended)

**Location**: `src/libraries/Domain/Hexalith.GitStorage.Events/GitStorageAccount/GitStorageAccountAdded.cs`

| Field | Type | DataMember Order | Description | Status |
|-------|------|------------------|-------------|--------|
| Id | string | 1 (inherited) | Aggregate identifier | Existing |
| Name | string | 2 | Display name | Existing |
| Comments | string? | 3 | Optional description | Existing |
| **ServerUrl** | string? | 4 | **NEW**: Optional API base URL | New |
| **AccessToken** | string? | 5 | **NEW**: Optional authentication token | New |
| **ProviderType** | GitServerProviderType? | 6 | **NEW**: Optional server platform type | New |

---

## New Commands

### ChangeGitStorageAccountApiCredentials

**Location**: `src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/ChangeGitStorageAccountApiCredentials.cs`

| Field | Type | DataMember Order | Description | Validation |
|-------|------|------------------|-------------|------------|
| Id | string | 1 (inherited) | Aggregate identifier | Required |
| ServerUrl | string | 2 | API base URL | Valid HTTPS URL |
| AccessToken | string | 3 | Authentication token | Non-empty |
| ProviderType | GitServerProviderType | 4 | Server platform type | Valid enum |

**Produces**: `GitStorageAccountApiCredentialsChanged` event

---

### ClearGitStorageAccountApiCredentials

**Location**: `src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/ClearGitStorageAccountApiCredentials.cs`

| Field | Type | DataMember Order | Description | Validation |
|-------|------|------------------|-------------|------------|
| Id | string | 1 (inherited) | Aggregate identifier | Required |

**Produces**: `GitStorageAccountApiCredentialsCleared` event

---

## Extended Commands

### AddGitStorageAccount (Extended)

**Location**: `src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/AddGitStorageAccount.cs`

| Field | Type | DataMember Order | Description | Status |
|-------|------|------------------|-------------|--------|
| Id | string | 1 (inherited) | Aggregate identifier | Existing |
| Name | string | 2 | Display name | Existing |
| Comments | string? | 3 | Optional description | Existing |
| **ServerUrl** | string? | 4 | **NEW**: Optional API base URL | New |
| **AccessToken** | string? | 5 | **NEW**: Optional authentication token | New |
| **ProviderType** | GitServerProviderType? | 6 | **NEW**: Optional server platform type | New |

---

## View Model Extensions

### GitStorageAccountDetailsViewModel (Extended)

**Location**: `src/libraries/Application/Hexalith.GitStorage.Requests/GitStorageAccount/GitStorageAccountDetailsViewModel.cs`

| Field | Type | DataMember Order | Description | Status |
|-------|------|------------------|-------------|--------|
| Id | string | 1 | Aggregate identifier | Existing |
| Name | string | 2 | Display name | Existing |
| Comments | string? | 3 | Optional description | Existing |
| Disabled | bool | 5 | Soft-delete flag | Existing |
| **ServerUrl** | string? | 6 | **NEW**: API base URL | New |
| **MaskedAccessToken** | string? | 7 | **NEW**: Masked token (last 4 chars) | New (computed) |
| **ProviderType** | GitServerProviderType? | 8 | **NEW**: Server platform type | New |
| **HasApiCredentials** | bool | - | **NEW**: Computed property | New (computed) |

**Computed Properties**:
```csharp
public string? MaskedAccessToken => AccessToken is { Length: > 4 }
    ? $"••••••••{AccessToken[^4..]}"
    : AccessToken is { Length: > 0 }
        ? "••••"
        : null;

public bool HasApiCredentials => !string.IsNullOrEmpty(ServerUrl) && !string.IsNullOrEmpty(AccessToken);
```

---

## State Transitions

### API Credentials Lifecycle

```
┌─────────────────┐
│   No Credentials │◄──────────────────────────────────┐
│   (Initial)      │                                    │
└────────┬────────┘                                    │
         │                                              │
         │ AddGitStorageAccount (with credentials)      │
         │ OR ChangeGitStorageAccountApiCredentials     │
         ▼                                              │
┌─────────────────┐                                    │
│ Has Credentials  │                                    │
│ (Configured)     │────────────────────────────────────┤
└────────┬────────┘  ClearGitStorageAccountApiCredentials
         │
         │ ChangeGitStorageAccountApiCredentials
         ▼
┌─────────────────┐
│ Has Credentials  │
│ (Updated)        │
└─────────────────┘
```

---

## Validation Rules Summary

| Command | Field | Rule | Error Message |
|---------|-------|------|---------------|
| ChangeGitStorageAccountApiCredentials | ServerUrl | NotEmpty | Server URL is required |
| ChangeGitStorageAccountApiCredentials | ServerUrl | ValidUri | Server URL must be a valid URL |
| ChangeGitStorageAccountApiCredentials | ServerUrl | HTTPS | Server URL must use HTTPS |
| ChangeGitStorageAccountApiCredentials | AccessToken | NotEmpty | Access token is required |
| ChangeGitStorageAccountApiCredentials | ProviderType | IsInEnum | Invalid provider type |
| AddGitStorageAccount | ServerUrl | ValidUri (when provided) | Server URL must be a valid HTTPS URL |
| AddGitStorageAccount | AccessToken | NotEmpty (when ServerUrl provided) | Access token is required when server URL is provided |

---

## Projection Updates

### Handlers Required

| Event | Projection Handler | Description |
|-------|-------------------|-------------|
| GitStorageAccountAdded | Update existing | Add API credential fields mapping |
| GitStorageAccountApiCredentialsChanged | New handler | Update API credential fields |
| GitStorageAccountApiCredentialsCleared | New handler | Clear API credential fields |
