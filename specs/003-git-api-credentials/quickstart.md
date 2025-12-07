# Quickstart: Git API Credentials Implementation

**Feature**: 003-git-api-credentials
**Date**: 2025-12-07

## Prerequisites

- Completed review of [spec.md](spec.md), [research.md](research.md), and [data-model.md](data-model.md)
- Understanding of existing GitStorageAccount and GitOrganization patterns in codebase
- .NET 10 SDK installed

---

## Implementation Order

Follow this sequence to maintain compile-time correctness:

### Phase 1: Domain Layer

#### 1.1 Create GitServerProviderType Enum

**File**: `src/libraries/Domain/Hexalith.GitStorage.Aggregates.Abstractions/Enums/GitServerProviderType.cs`

```csharp
// <copyright file="GitServerProviderType.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Aggregates.Enums;

/// <summary>
/// Represents the type of Git server platform.
/// </summary>
public enum GitServerProviderType
{
    /// <summary>
    /// GitHub.com or GitHub Enterprise Server.
    /// </summary>
    GitHub = 0,

    /// <summary>
    /// Forgejo Git server instances.
    /// </summary>
    Forgejo = 1,

    /// <summary>
    /// Gitea Git server instances.
    /// </summary>
    Gitea = 2,

    /// <summary>
    /// Generic Git server with compatible API.
    /// </summary>
    Generic = 3,
}
```

#### 1.2 Create New Domain Events

**File**: `src/libraries/Domain/Hexalith.GitStorage.Events/GitStorageAccount/GitStorageAccountApiCredentialsChanged.cs`

Pattern reference: See `GitOrganizationVisibilityChanged.cs` for enum usage in events.

**File**: `src/libraries/Domain/Hexalith.GitStorage.Events/GitStorageAccount/GitStorageAccountApiCredentialsCleared.cs`

Pattern reference: See `GitStorageAccountDisabled.cs` for minimal events.

#### 1.3 Extend GitStorageAccountAdded Event

Add optional parameters: `ServerUrl`, `AccessToken`, `ProviderType` with DataMember Orders 4, 5, 6.

#### 1.4 Update GitStorageAccount Aggregate

Add new fields and `ApplyEvent` overloads for new events.

---

### Phase 2: Application Layer

#### 2.1 Create New Commands

**Files**:
- `src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/ChangeGitStorageAccountApiCredentials.cs`
- `src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/ClearGitStorageAccountApiCredentials.cs`

Pattern reference: See `ChangeGitOrganizationVisibility.cs`.

#### 2.2 Create Command Validators

**Files**:
- `src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/ChangeGitStorageAccountApiCredentialsValidator.cs`

Key validations:
```csharp
RuleFor(x => x.ServerUrl)
    .NotEmpty()
    .Must(BeValidHttpsUrl).WithMessage("Server URL must be a valid HTTPS URL");

RuleFor(x => x.AccessToken)
    .NotEmpty();

RuleFor(x => x.ProviderType)
    .IsInEnum();
```

#### 2.3 Extend AddGitStorageAccount Command

Add optional API credential parameters.

#### 2.4 Update AddGitStorageAccountValidator

Add conditional validation for API credentials.

---

### Phase 3: Projections

#### 3.1 Update GitStorageAccountDetailsViewModel

Add properties: `ServerUrl`, `MaskedAccessToken` (computed), `ProviderType`, `HasApiCredentials` (computed).

#### 3.2 Create Projection Handlers

**Files**:
- `GitStorageAccountApiCredentialsChangedOnDetailsProjectionHandler.cs`
- `GitStorageAccountApiCredentialsClearedOnDetailsProjectionHandler.cs`

Pattern reference: See `GitOrganizationVisibilityChangedOnDetailsProjectionHandler.cs`.

#### 3.3 Update Existing Projection Handler

Update `GitStorageAccountAddedOnDetailsProjectionHandler` to map new optional fields.

---

### Phase 4: Presentation Layer

#### 4.1 Update GitStorageAccountEditViewModel

Add properties and change tracking for API credentials.

Key additions:
```csharp
public string? ServerUrl { get; set; }
public string? AccessToken { get; set; }
public GitServerProviderType? ProviderType { get; set; }

public bool ApiCredentialsChanged =>
    ServerUrl != Original.ServerUrl ||
    AccessToken != Original.AccessToken ||
    ProviderType != Original.ProviderType;
```

Update `SaveAsync` to dispatch `ChangeGitStorageAccountApiCredentials` when credentials changed.

#### 4.2 Update UI Forms

Add form fields for:
- Server URL (text input)
- Access Token (password input)
- Provider Type (dropdown)

---

### Phase 5: Testing

#### 5.1 Unit Tests

**Files**:
- `test/Hexalith.GitStorage.UnitTests/Aggregates/GitStorageAccountApiCredentialsTests.cs`
- `test/Hexalith.GitStorage.UnitTests/Commands/ChangeGitStorageAccountApiCredentialsValidatorTests.cs`

Key test cases:
- Apply GitStorageAccountApiCredentialsChanged updates aggregate fields
- Apply GitStorageAccountApiCredentialsCleared clears aggregate fields
- Validator rejects invalid HTTPS URLs
- Validator rejects empty access tokens
- Masked token displays correctly

---

## Validation Patterns

### HTTPS URL Validation

```csharp
private static bool BeValidHttpsUrl(string url)
{
    return Uri.TryCreate(url, UriKind.Absolute, out Uri? uri)
           && uri.Scheme == Uri.UriSchemeHttps;
}
```

### Token Masking

```csharp
public string? MaskedAccessToken => AccessToken is { Length: > 4 }
    ? $"••••••••{AccessToken[^4..]}"
    : AccessToken is { Length: > 0 }
        ? "••••"
        : null;
```

---

## Build Verification

After each phase, verify compilation:

```bash
dotnet build Hexalith.GitStorage.sln
```

After Phase 5, run tests:

```bash
dotnet test Hexalith.GitStorage.sln
```

---

## Common Pitfalls

1. **DataMember Order conflicts**: Verify Order values don't conflict with existing fields
2. **Nullable handling**: Use `GitServerProviderType?` for optional fields, validate before use
3. **Event replay**: Ensure aggregate Apply methods handle null for optional fields from older events
4. **Projection handler registration**: Add new handlers to DI container in module setup
