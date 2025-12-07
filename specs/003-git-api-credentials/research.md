# Research: Git API Credentials

**Feature**: 003-git-api-credentials
**Date**: 2025-12-07
**Status**: Complete

## Overview

This document captures research decisions for implementing Git API credentials support in GitStorageAccount.

---

## Decision 1: Provider Type Enumeration

**Question**: What Git server providers should be supported and how should they be represented?

### Decision

Create a `GitServerProviderType` enum with four values:
- `GitHub = 0` (default)
- `Forgejo = 1`
- `Gitea = 2`
- `Generic = 3`

### Rationale

1. **GitHub as default**: Most common use case, aligns with spec assumption
2. **Forgejo and Gitea separated**: While Gitea-derived, Forgejo may diverge in API; explicit types enable future differentiation
3. **Generic fallback**: Allows connecting to compatible servers not explicitly listed
4. **Zero-based enum**: Follows existing pattern (e.g., `GitOrganizationVisibility`)

### Alternatives Considered

| Alternative | Rejected Because |
|-------------|------------------|
| String-based provider type | Loses type safety, allows typos, harder to validate |
| Single "Gitea-compatible" value | Loses granularity if APIs diverge |
| Extensible interface pattern | Over-engineering for current scope |

---

## Decision 2: Access Token Storage Strategy

**Question**: How should access tokens be stored and protected?

### Decision

Store access tokens as plain strings in the aggregate/events. Security is delegated to infrastructure layer (Azure Cosmos DB encryption at rest).

### Rationale

1. **Spec assumption**: "Encryption at rest is handled by the underlying infrastructure/storage layer"
2. **Event sourcing constraint**: Events must be immutable and reconstructable
3. **Constitution compliance**: Principle VII states tokens must not be logged/exposed in events - this means no logging, but storage is acceptable
4. **Consistency**: Same pattern as any other sensitive field (authentication tokens are common in Dapr state stores)

### Alternatives Considered

| Alternative | Rejected Because |
|-------------|------------------|
| Application-level encryption | Complicates event replay, key management out of scope |
| Separate secrets service | Over-engineering, breaks aggregate cohesion |
| Token hashing | Tokens need to be used for API calls, hashing destroys utility |

---

## Decision 3: Server URL Validation Approach

**Question**: How should server URL validation be implemented?

### Decision

Validate at command level using FluentValidation (consistent with existing validators):
1. Must be a valid URI format
2. Must use HTTPS scheme
3. Must have a host component

### Rationale

1. **FR-004**: "System MUST validate that the server URL is a valid HTTPS URL format"
2. **Spec assumption**: "Format validation rather than connectivity testing"
3. **Existing pattern**: `AddGitStorageAccountValidator` uses FluentValidation

### Alternatives Considered

| Alternative | Rejected Because |
|-------------|------------------|
| Connectivity testing | Spec explicitly excludes this; would be separate feature |
| URL normalization (auto-add https://) | Spec says validate, not fix; keeps user responsibility clear |
| Regex-only validation | Less maintainable than Uri.TryCreate approach |

---

## Decision 4: Event Design - Single vs Multiple Events

**Question**: Should API credential changes emit one event or separate events for each field?

### Decision

Use two events:
1. `GitStorageAccountApiCredentialsChanged` - Contains all three fields (ServerUrl, AccessToken, ProviderType)
2. `GitStorageAccountApiCredentialsCleared` - No payload beyond aggregate ID

### Rationale

1. **FR-006**: "Unified command that updates server URL, access token, and provider type together"
2. **Atomicity**: Credentials are cohesive - partial updates could leave invalid state
3. **FR-007**: "Allow clearing/removing API credentials" - distinct action deserves distinct event
4. **Pattern matching**: `GitStorageAccountDisabled`/`Enabled` shows binary state events are acceptable

### Alternatives Considered

| Alternative | Rejected Because |
|-------------|------------------|
| Separate events per field | Forces sequential changes, complicates validation |
| Single event with nullable fields | Ambiguous semantics (null = no change vs null = clear) |
| Reuse existing events | Violates single responsibility, events become overloaded |

---

## Decision 5: AddGitStorageAccount Extension

**Question**: Should initial API credentials be set via AddGitStorageAccount or require separate command?

### Decision

Extend `AddGitStorageAccount` command and `GitStorageAccountAdded` event with optional API credential fields:
- `ServerUrl` (string?, optional)
- `AccessToken` (string?, optional)
- `ProviderType` (GitServerProviderType?, optional - defaults to GitHub when credentials provided)

### Rationale

1. **Spec clarification**: "Initial values can be set via AddGitStorageAccount command"
2. **User experience**: Single action to create fully configured account
3. **Event sourcing**: Account history is complete from first event
4. **Backward compatibility**: Nullable fields, existing deserialization unaffected

### Alternatives Considered

| Alternative | Rejected Because |
|-------------|------------------|
| Mandatory separate command | Poor UX, requires two operations for common case |
| Required fields on Add | Breaks existing accounts, spec says credentials are optional |

---

## Decision 6: Token Masking Implementation

**Question**: Where and how should token masking occur?

### Decision

Implement masking in `GitStorageAccountDetailsViewModel`:
- Store full token in aggregate/events (needed for API calls)
- ViewModel exposes `MaskedAccessToken` computed property: `••••••••{last4}`
- UI binds to masked version for display
- Edit ViewModel handles full token for input

### Rationale

1. **FR-005**: "Display access tokens partially masked, showing only the last 4 characters"
2. **Separation of concerns**: Presentation logic in ViewModel, not Domain
3. **Security boundary**: Full token never reaches browser except during input

### Alternatives Considered

| Alternative | Rejected Because |
|-------------|------------------|
| Mask in aggregate | Violates DDD - presentation concern in domain |
| Mask in API response | Breaks single-responsibility, harder to test |
| Two-way masked binding | Complex, error-prone for token updates |

---

## Decision 7: DataMember Order Assignments

**Question**: What DataMember Order values should be used for new fields?

### Decision

For `GitStorageAccount` aggregate (current max Order = 7):
- `ServerUrl`: Order = 8
- `AccessToken`: Order = 9
- `ProviderType`: Order = 10

For `GitStorageAccountAdded` event (current max Order = 3):
- `ServerUrl`: Order = 4
- `AccessToken`: Order = 5
- `ProviderType`: Order = 6

### Rationale

1. **Convention**: Continue sequential ordering from existing maximum
2. **Stability**: Existing field orders unchanged, backward compatible
3. **Pattern**: Matches GitOrganization which uses Orders 1-10

### Alternatives Considered

| Alternative | Rejected Because |
|-------------|------------------|
| Skip numbers (e.g., 10, 20, 30) | Project doesn't use this pattern |
| Group by concern (100+) | Inconsistent with existing code |

---

## Technology Compatibility Notes

### GitHub API
- Base URL: `https://api.github.com`
- Token header: `Authorization: Bearer {token}` or `Authorization: token {token}`

### Forgejo/Gitea API
- Base URL: `{server}/api/v1`
- Token header: `Authorization: token {token}`

### Common Patterns
- Both support similar REST endpoints for organizations, repositories
- Provider type enables selecting correct authentication header format in future integration

---

## Summary

All research questions have been resolved. The design follows existing codebase patterns with minimal complexity while satisfying all functional requirements from the specification.
