# Research: Git Storage Account Entity

**Feature Branch**: `001-git-storage-account`
**Date**: 2025-12-01
**Status**: Complete

## Overview

This document consolidates research findings for implementing the GitStorageAccount entity within the Hexalith.GitStorage module. All technical decisions are aligned with existing codebase patterns and the project constitution.

---

## 1. DDD Aggregate Pattern in Hexalith

### Decision
Use sealed record with primary constructor for the GitStorageAccount aggregate, following the existing pattern in the codebase.

### Rationale
- Records provide immutability and value-based equality
- Primary constructors reduce boilerplate
- `[DataContract]` and `[DataMember(Order = N)]` ensure deterministic serialization
- Sealed prevents inheritance issues with event sourcing

### Implementation Pattern
```csharp
[DataContract]
public sealed record GitStorageAccount(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments,
    [property: DataMember(Order = 4)] bool Disabled) : IDomainAggregate
{
    public ApplyResult Apply([NotNull] object domainEvent) { ... }
}
```

### Alternatives Considered
- **Class-based aggregate**: Rejected because records provide better immutability guarantees and align with existing patterns
- **Open record (non-sealed)**: Rejected to prevent inheritance complications in event sourcing

---

## 2. Event Design for GitStorageAccount

### Decision
Four domain events: `GitStorageAccountAdded`, `GitStorageAccountDescriptionChanged`, `GitStorageAccountEnabled`, `GitStorageAccountDisabled`

### Rationale
- Events represent atomic business facts
- Past-tense naming per constitution (Principle I)
- Each state change has a distinct event for clear audit trail
- Events are immutable records with `[PolymorphicSerialization]`

### Event Structure
| Event | Properties | Purpose |
|-------|------------|---------|
| `GitStorageAccountAdded` | Id, Name, Comments | Initial account creation |
| `GitStorageAccountDescriptionChanged` | Id, Name, Comments | Update display name or comments |
| `GitStorageAccountEnabled` | Id | Re-enable a disabled account |
| `GitStorageAccountDisabled` | Id | Soft-delete by disabling |

### Alternatives Considered
- **Single "AccountUpdated" event**: Rejected because it loses granular audit information
- **"AccountDeleted" event**: Not needed; soft-delete via Disabled state preserves event history

---

## 3. Command Validation Strategy

### Decision
Validation at command handler level using FluentValidation, before aggregate Apply is invoked.

### Rationale
- Per spec clarification: "Validation at command handler level (validate before aggregate apply)"
- Keeps aggregate Apply methods focused on business state transitions
- Provides clear, actionable error messages (SC-005)
- Aligns with CQRS principle: commands are validated before execution

### Validation Rules
| Command | Validation |
|---------|------------|
| `AddGitStorageAccount` | Id required, Name required (non-empty) |
| `ChangeGitStorageAccountDescription` | Id required, Name required (non-empty) |
| `EnableGitStorageAccount` | Id required |
| `DisableGitStorageAccount` | Id required |

### Alternatives Considered
- **Aggregate-only validation**: Rejected because aggregate shouldn't handle input format validation
- **API controller validation**: Rejected because validation logic should be reusable across entry points

---

## 4. Request/Query Design for CQRS

### Decision
Three request types: `GetGitStorageAccountDetails`, `GetGitStorageAccountSummaries`, `GetGitStorageAccountIds`

### Rationale
- Requests read from projections (read models), not aggregates
- `Details` for single-account full view
- `Summaries` for paginated list (FR-010)
- `Ids` for dropdown/selection components
- All implement `IRequest` interface per Hexalith patterns

### Request Structure
```csharp
[PolymorphicSerialization]
public partial record GetGitStorageAccountDetails(
    string Id,
    [property: DataMember(Order = 2)] GitStorageAccountDetailsViewModel? Result = null)
    : GitStorageAccountRequest(Id), IRequest;
```

### Alternatives Considered
- **Generic repository queries**: Rejected to maintain CQRS separation
- **GraphQL**: Not currently in tech stack; REST-style requests align with existing patterns

---

## 5. Projection Handler Pattern

### Decision
Event-specific projection handlers that update read models on each domain event.

### Rationale
- Projections are the only read models for queries (Constitution II)
- Each event type has a dedicated handler for clarity
- Handlers update Dapr state store for fast queries
- Eventual consistency is acceptable for this admin-focused entity

### Handler Structure
| Event | Projection Handler | Read Model Update |
|-------|-------------------|-------------------|
| `GitStorageAccountAdded` | `GitStorageAccountAddedOnDetailsProjectionHandler` | Create new Details record |
| `GitStorageAccountDescriptionChanged` | `GitStorageAccountDescriptionChangedOnDetailsProjectionHandler` | Update Name, Comments |
| `GitStorageAccountEnabled` | `GitStorageAccountEnabledOnDetailsProjectionHandler` | Set Disabled = false |
| `GitStorageAccountDisabled` | `GitStorageAccountDisabledOnDetailsProjectionHandler` | Set Disabled = true |

### Alternatives Considered
- **Single combined handler**: Rejected for maintainability and single-responsibility principle
- **Inline projection in aggregate**: Violates CQRS separation

---

## 6. Authorization Model

### Decision
Role-based authorization requiring Admin role for all GitStorageAccount operations.

### Rationale
- Per spec clarification: "Admin role required for all account operations"
- Implemented via ASP.NET Core authorization attributes on API controllers
- Consistent with administrative nature of account management

### Implementation
```csharp
[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class GitStorageAccountController : ControllerBase { ... }
```

### Alternatives Considered
- **Policy-based authorization**: Considered but role-based is simpler and sufficient for this use case
- **Resource-based authorization**: Not needed; accounts aren't user-scoped

---

## 7. Concurrency Handling

### Decision
Optimistic concurrency using aggregate version from event stream.

### Rationale
- Per spec edge case: "The system should use optimistic concurrency to prevent lost updates"
- Event sourcing naturally supports this via expected version checks
- Dapr Actors handle concurrency at the aggregate instance level

### Implementation
- Aggregate maintains version from event count
- Command handlers check expected version before applying
- Conflict results in retry or error response

### Alternatives Considered
- **Pessimistic locking**: Unnecessary overhead for low-volume admin operations
- **Last-write-wins**: Would violate audit integrity

---

## 8. Default State Behavior

### Decision
New accounts are enabled by default (Disabled = false).

### Rationale
- Per spec clarification: "Enabled by default (account is active immediately upon creation)"
- Simplifies common case (create and use immediately)
- Explicit disable action required to deactivate

### Implementation
- `GitStorageAccountAdded` event initializes `Disabled = false`
- No "enabled" flag in creation command; implicit from event application

---

## 9. ViewModel Design for Projections

### Decision
Two view models: `GitStorageAccountDetailsViewModel` (full) and `GitStorageAccountSummaryViewModel` (list item).

### Rationale
- Details VM for single-account view with all fields
- Summary VM for paginated list with Id, Name, Disabled status only
- Aligns with UI requirements and reduces data transfer

### Structure
| ViewModel | Properties |
|-----------|------------|
| `GitStorageAccountDetailsViewModel` | Id, Name, Comments, Disabled |
| `GitStorageAccountSummaryViewModel` | Id, Name, Disabled |

---

## 10. API Endpoint Design

### Decision
RESTful endpoints under `/api/GitStorageAccount` following existing Hexalith patterns.

### Rationale
- Consistent with existing `GitStorageAccountIntegrationEventsController` pattern
- Commands via POST (create, update operations)
- Queries via GET with appropriate filtering/pagination

### Endpoints
| Method | Path | Purpose |
|--------|------|---------|
| POST | `/api/GitStorageAccount` | Execute command (Add, ChangeDescription, Enable, Disable) |
| GET | `/api/GitStorageAccount/{id}` | Get account details |
| GET | `/api/GitStorageAccount` | Get paginated summaries |

---

## Summary

All research items resolved. The implementation follows established Hexalith patterns with:
- Sealed record aggregate with event sourcing
- Four distinct domain events
- Command-level validation with FluentValidation
- CQRS-compliant requests with projection-backed read models
- Role-based Admin authorization
- Optimistic concurrency via event versioning
- Enabled-by-default account creation

No NEEDS CLARIFICATION items remain. Ready for Phase 1: Design & Contracts.
