# Implementation Plan: GitOrganization Entity

**Branch**: `002-git-organization` | **Date**: 2025-12-07 | **Spec**: [spec.md](spec.md)
**Input**: Feature specification from `/specs/002-git-organization/spec.md`

## Summary

Implement a GitOrganization entity following DDD/CQRS/Event Sourcing patterns to manage organizations on Git servers (GitHub, Forgejo). The entity supports synchronization from remote Git Storage Accounts, creation via application API (with remote provisioning), CRUD operations, and Blazor UI. Organizations have a composite key `{GitStorageAccountId}-{OrganizationName}`, track origin (synced vs. created), sync status, and support soft-delete.

## Technical Context

**Language/Version**: .NET 10 / C# 13 (use latest language features)
**Primary Dependencies**: Hexalith framework, Dapr, FluentValidation, System.Text.Json
**Storage**: Azure Cosmos DB (event store), Redis (state/cache via Dapr)
**Testing**: xUnit + Shouldly + Moq
**Target Platform**: Linux server (containerized), Blazor InteractiveAuto (SSR + WebAssembly)
**Project Type**: DDD/CQRS/Event Sourcing modular architecture
**Performance Goals**: Sync ≤30s for 100 orgs, CRUD ≤10s, reads ≤1s (per spec SC-001 to SC-003)
**Constraints**: Event immutability, no hard-delete, soft-delete only, infrastructure-managed retry (Dapr)
**Scale/Scope**: Multiple Git Storage Accounts with 100+ organizations each

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

| Principle | Status | Notes |
|-----------|--------|-------|
| I. Domain-Driven Design First | ✅ PASS | Business logic in aggregate, domain events, value objects (enums) |
| II. CQRS Separation (NON-NEGOTIABLE) | ✅ PASS | Separate Commands/Requests, projections for reads |
| III. Event Sourcing Compliance | ✅ PASS | Immutable events with `[PolymorphicSerialization]`, deterministic Apply |
| IV. Clean Architecture Layers | ✅ PASS | Domain → Application → Infrastructure → Presentation flow |
| V. Code Quality Standards (NON-NEGOTIABLE) | ✅ PASS | Copyright headers, file-scoped namespaces, primary constructors, XML docs |
| VI. Test-First Development | ✅ PASS | Unit tests for aggregate, event serialization |
| VII. Provider Abstraction | ✅ PASS | `IGitProviderAdapter` interface abstracts GitHub/Forgejo |

**Pre-Design Gate Result**: ✅ ALL PASSED - Proceed to Phase 0

## Project Structure

### Documentation (this feature)

```text
specs/002-git-organization/
├── plan.md              # This file
├── research.md          # Phase 0 output - Architecture decisions
├── data-model.md        # Phase 1 output - Entity definitions
├── quickstart.md        # Phase 1 output - Implementation guide
├── contracts/           # Phase 1 output - API contracts
│   └── api.yaml         # OpenAPI 3.0 specification
└── tasks.md             # Phase 2 output (/speckit.tasks command)
```

### Source Code (repository root)

```text
src/libraries/
├── Domain/
│   ├── Hexalith.GitStorage.Events/
│   │   └── GitOrganization/
│   │       ├── GitOrganizationEvent.cs           # Base event
│   │       ├── GitOrganizationAdded.cs           # Initialization (API create)
│   │       ├── GitOrganizationSynced.cs          # Initialization (sync)
│   │       ├── GitOrganizationDescriptionChanged.cs
│   │       ├── GitOrganizationDisabled.cs
│   │       ├── GitOrganizationEnabled.cs
│   │       └── GitOrganizationMarkedNotFound.cs
│   ├── Hexalith.GitStorage.Aggregates/
│   │   ├── GitOrganization.cs                    # Aggregate
│   │   └── GitOrganizationValidator.cs
│   └── Hexalith.GitStorage.Aggregates.Abstractions/
│       ├── GitOrganizationDomainHelper.cs
│       └── Enums/
│           ├── GitOrganizationOrigin.cs
│           └── GitOrganizationSyncStatus.cs
├── Application/
│   ├── Hexalith.GitStorage.Commands/
│   │   └── GitOrganization/
│   │       ├── GitOrganizationCommand.cs         # Base command
│   │       ├── AddGitOrganization.cs
│   │       ├── ChangeGitOrganizationDescription.cs
│   │       ├── DisableGitOrganization.cs
│   │       ├── EnableGitOrganization.cs
│   │       └── SyncGitOrganizations.cs           # Bulk sync trigger
│   ├── Hexalith.GitStorage.Requests/
│   │   └── GitOrganization/
│   │       ├── GitOrganizationRequest.cs         # Base request
│   │       ├── GetGitOrganizationDetails.cs
│   │       ├── GetGitOrganizationSummaries.cs
│   │       └── ViewModels/
│   │           ├── GitOrganizationDetailsViewModel.cs
│   │           └── GitOrganizationSummaryViewModel.cs
│   ├── Hexalith.GitStorage.Projections/
│   │   ├── ProjectionHandlers/
│   │   │   ├── GitOrganizationDetailsProjectionHandler.cs
│   │   │   └── GitOrganizationSummaryProjectionHandler.cs
│   │   └── RequestHandlers/
│   │       └── GetGitOrganizationDetailsHandler.cs
│   ├── Hexalith.GitStorage.Abstractions/
│   │   ├── Services/
│   │   │   └── IGitProviderAdapter.cs
│   │   └── Models/
│   │       └── GitOrganizationDto.cs
│   └── Hexalith.GitStorage/
│       ├── CommandHandlers/
│       │   └── GitOrganizationCommandHandlerHelper.cs
│       └── EventHandlers/
│           └── GitOrganizationEventHandlerHelper.cs
└── Presentation/
    ├── Hexalith.GitStorage.UI.Components/
    │   └── GitOrganization/
    │       ├── GitOrganizationCard.razor
    │       └── GitOrganizationForm.razor
    └── Hexalith.GitStorage.UI.Pages/
        └── GitOrganization/
            ├── GitOrganizationEditViewModel.cs
            ├── GitOrganizationEditValidation.cs
            ├── GitOrganizationListPage.razor
            ├── GitOrganizationDetailsPage.razor
            ├── GitOrganizationCreatePage.razor
            └── GitOrganizationEditPage.razor

tests/
└── Hexalith.GitStorage.UnitTests/
    ├── Aggregates/
    │   └── GitOrganizationTests.cs
    ├── Events/
    │   └── GitOrganizationEventsSerializationTests.cs
    └── Commands/
        └── GitOrganizationCommandsTests.cs
```

**Structure Decision**: Follows established Hexalith.GitStorage layer structure from feature 001 (GitStorageAccount). Domain events → Aggregate → Commands → Requests → Projections → UI Pages.

## Complexity Tracking

> No constitution violations. Standard DDD/CQRS/ES implementation following existing patterns.

| Aspect | Complexity | Justification |
|--------|------------|---------------|
| Composite Key | LOW | `{GitStorageAccountId}-{OrganizationName}` pattern from spec |
| Dual Initialization | MEDIUM | Two init events (Added vs Synced) - required by spec FR-005 |
| Provider Abstraction | LOW | Reuses existing `IGitProviderAdapter` interface |
| Sync Logic | MEDIUM | Event handler calls remote, infrastructure retry |

## Implementation Phases

### Phase 0: Research (Completed)

- Architecture decisions documented in [research.md](research.md)
- All "NEEDS CLARIFICATION" items resolved via spec clarifications

### Phase 1: Design & Contracts (Current)

- Entity model in [data-model.md](data-model.md)
- API contracts in [contracts/api.yaml](contracts/api.yaml)
- Implementation guide in [quickstart.md](quickstart.md)

### Phase 2: Task Generation

- Run `/speckit.tasks` to generate [tasks.md](tasks.md)
