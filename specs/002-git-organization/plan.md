# Implementation Plan: Git Organization Entity

**Branch**: `002-git-organization` | **Date**: 2025-12-01 | **Spec**: [spec.md](spec.md)
**Input**: Feature specification from `/specs/002-git-organization/spec.md`

## Summary

Implement a GitOrganization entity that represents organizations on a git server (GitHub or Forgejo). The entity supports two modes of creation: synchronization from a Git Storage Account (discovering existing remote organizations) and creation via the application API (which provisions the organization on the remote server). All state changes emit domain events, with event handlers responsible for synchronizing local changes to the remote git server. Retry logic is delegated to Dapr messaging infrastructure.

## Technical Context

**Language/Version**: .NET 10 / C# 13 (latest features including primary constructors, file-scoped namespaces)
**Primary Dependencies**: Hexalith framework (DDD/CQRS/Event Sourcing), Dapr, .NET Aspire
**Storage**: Azure Cosmos DB (event store), Redis (projections/cache)
**Testing**: xUnit + Shouldly + Moq + Coverlet
**Target Platform**: Linux containers (via Aspire orchestration), Blazor WebAssembly client
**Project Type**: DDD modular monolith with layered architecture
**Performance Goals**: Sync 100 organizations in <30 seconds, single org create <10 seconds, queries <1 second
**Constraints**: Event handlers fail-fast with infrastructure-managed retry (Dapr), no hard deletes, Admin role required
**Scale/Scope**: Expected 100s of organizations per Git Storage Account, multiple accounts per deployment

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

| Principle | Status | Evidence |
|-----------|--------|----------|
| I. Domain-Driven Design First | ✅ PASS | GitOrganization aggregate with domain events, business logic in domain layer |
| II. CQRS Separation | ✅ PASS | Commands (AddGitOrganization, SyncGitOrganizations), Requests (GetGitOrganizationDetails, GetGitOrganizationSummaries) |
| III. Event Sourcing Compliance | ✅ PASS | Events: GitOrganizationAdded, GitOrganizationSynced, GitOrganizationDescriptionChanged, GitOrganizationMarkedNotFound |
| IV. Clean Architecture Layers | ✅ PASS | Following established layer structure: Events→Aggregates→Commands→Requests→Projections→UI |
| V. Code Quality Standards | ✅ PASS | Will include copyright headers, primary constructors, XML docs, DataContract/DataMember attributes |
| VI. Test-First Development | ✅ PASS | Unit tests for aggregate Apply methods, event serialization, command validation |
| VII. Provider Abstraction | ✅ PASS | IGitProviderAdapter interface for GitHub/Forgejo with adapter implementations in Infrastructure |

**Gate Result**: PASSED - All constitution principles satisfied

## Project Structure

### Documentation (this feature)

```text
specs/002-git-organization/
├── plan.md              # This file
├── research.md          # Phase 0 output
├── data-model.md        # Phase 1 output
├── quickstart.md        # Phase 1 output
├── contracts/           # Phase 1 output (API contracts)
└── tasks.md             # Phase 2 output (/speckit.tasks command)
```

### Source Code (repository root)

```text
src/libraries/
├── Domain/
│   ├── Hexalith.GitStorage.Events/
│   │   └── GitOrganization/           # NEW: Domain events
│   │       ├── GitOrganizationEvent.cs
│   │       ├── GitOrganizationAdded.cs
│   │       ├── GitOrganizationSynced.cs
│   │       ├── GitOrganizationDescriptionChanged.cs
│   │       ├── GitOrganizationMarkedNotFound.cs
│   │       ├── GitOrganizationDisabled.cs
│   │       └── GitOrganizationEnabled.cs
│   ├── Hexalith.GitStorage.Aggregates/
│   │   ├── GitOrganization.cs         # NEW: Aggregate
│   │   └── Validators/
│   │       └── GitOrganizationValidator.cs  # NEW: Validation
│   └── Hexalith.GitStorage.Aggregates.Abstractions/
│       ├── GitOrganizationDomainHelper.cs   # NEW: Constants
│       └── Enums/
│           ├── GitOrganizationOrigin.cs     # NEW: Synced | CreatedViaApplication
│           └── GitOrganizationSyncStatus.cs # NEW: Synced | NotFoundOnRemote | SyncError
├── Application/
│   ├── Hexalith.GitStorage.Commands/
│   │   └── GitOrganization/           # NEW: Commands
│   │       ├── GitOrganizationCommand.cs
│   │       ├── AddGitOrganization.cs
│   │       ├── SyncGitOrganizations.cs
│   │       ├── ChangeGitOrganizationDescription.cs
│   │       ├── DisableGitOrganization.cs
│   │       └── EnableGitOrganization.cs
│   ├── Hexalith.GitStorage.Requests/
│   │   └── GitOrganization/           # NEW: Requests
│   │       ├── GitOrganizationRequest.cs
│   │       ├── GetGitOrganizationDetails.cs
│   │       ├── GetGitOrganizationSummaries.cs
│   │       ├── GitOrganizationDetailsViewModel.cs
│   │       └── GitOrganizationSummaryViewModel.cs
│   ├── Hexalith.GitStorage.Projections/
│   │   └── ProjectionHandlers/
│   │       ├── Details/               # NEW: Detail projection handlers
│   │       └── Summaries/             # NEW: Summary projection handlers
│   └── Hexalith.GitStorage/
│       ├── CommandHandlers/
│       │   └── GitOrganizationCommandHandlerHelper.cs  # NEW
│       └── EventHandlers/
│           └── GitOrganizationEventHandlerHelper.cs    # NEW
├── Infrastructure/
│   ├── Hexalith.GitStorage.Servers/
│   │   ├── Services/
│   │   │   └── GitOrganizationService.cs      # NEW: Service
│   │   └── Helpers/
│   │       └── GitOrganizationWebApiHelpers.cs # NEW: API helpers
│   ├── Hexalith.GitStorage.ApiServer/
│   │   └── Controllers/
│   │       └── GitOrganizationIntegrationEventsController.cs  # NEW
│   └── Hexalith.GitStorage.Abstractions/      # NEW or extend existing
│       └── IGitProviderAdapter.cs             # NEW: Provider interface
└── Presentation/
    ├── Hexalith.GitStorage.UI.Components/
    │   └── GitOrganization/           # NEW: UI components (future)
    └── Hexalith.GitStorage.UI.Pages/
        └── GitOrganization/           # NEW: Pages (future)

tests/
└── Hexalith.GitStorage.UnitTests/
    └── GitOrganization/               # NEW: Unit tests
        ├── GitOrganizationAggregateTests.cs
        ├── GitOrganizationEventsSerializationTests.cs
        └── GitOrganizationCommandsValidationTests.cs
```

**Structure Decision**: Following the established Hexalith.GitStorage architecture with layered DDD approach. New GitOrganization entity follows the same pattern as GitStorageAccount with events in Domain layer, commands/requests in Application layer, and integration handlers in Infrastructure layer.

## Complexity Tracking

> No constitution violations requiring justification. Design follows established patterns.

| Aspect | Decision | Rationale |
|--------|----------|-----------|
| Provider Adapter | Single IGitProviderAdapter interface | Abstracts GitHub/Forgejo differences, enables testing with mocks |
| Sync Strategy | On-demand via command, not scheduled | Per spec assumptions; scheduled sync is future enhancement |
| Event Handlers | Dapr-managed retry | Infrastructure handles failure recovery per clarifications |
