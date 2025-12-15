# Implementation Plan: GitRepository Aggregate

**Branch**: `004-git-repository` | **Date**: 2025-12-15 | **Spec**: [spec.md](spec.md)
**Input**: Feature specification from `/specs/004-git-repository/spec.md`

## Summary

Implement the GitRepository aggregate to manage Git repository metadata within the Hexalith.GitStorage module. This aggregate enables creating repositories on remote Git servers (GitHub/Forgejo), synchronizing repository metadata bi-directionally, and managing repository lifecycle (visibility, enable/disable, default branch). The implementation follows existing DDD/CQRS/Event Sourcing patterns established by GitOrganization and GitStorageAccount aggregates.

## Technical Context

**Language/Version**: C# 13 / .NET 10
**Primary Dependencies**: Hexalith Framework (DDD/CQRS/Event Sourcing), FluentValidation, Dapr, Microsoft Fluent UI Blazor
**Storage**: Azure Cosmos DB (event store), Redis (state/cache) via Dapr abstraction
**Testing**: xUnit + Shouldly + Moq
**Target Platform**: Linux/Windows server (Aspire-orchestrated microservices)
**Project Type**: Modular .NET library with Blazor UI
**Performance Goals**: Repository operations complete within 2 seconds under normal load
**Constraints**: Two-way sync with remote Git servers; no hard deletes; offline resilience via Dapr retry
**Scale/Scope**: Per-organization repository management; typical 100s of repositories per organization

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

| Principle | Status | Evidence |
|-----------|--------|----------|
| I. Domain-Driven Design First | ✅ PASS | GitRepository aggregate encapsulates all business rules; domain events represent facts (past tense naming); no infrastructure dependencies in domain layer |
| II. CQRS Separation (NON-NEGOTIABLE) | ✅ PASS | Commands follow `{Verb}{Entity}` pattern (AddGitRepository, ChangeGitRepositoryDescription); Requests follow `Get{Entity}{Details\|Summaries}` pattern; projections as read models |
| III. Event Sourcing Compliance | ✅ PASS | All state changes via immutable events with `[PolymorphicSerialization]` and `[DataMember(Order = N)]`; Apply methods return new state |
| IV. Clean Architecture Layers | ✅ PASS | Layer structure matches existing aggregates: Domain → Application → Infrastructure → Presentation |
| V. Code Quality Standards (NON-NEGOTIABLE) | ✅ PASS | Will include copyright headers, file-scoped namespaces, primary constructors, XML documentation, DataContract attributes |
| VI. Test-First Development | ✅ PASS | Tests planned for validators, aggregate behavior, event serialization using xUnit + Shouldly |
| VII. Provider Abstraction | ✅ PASS | Git provider operations abstracted via existing `IGitProviderAdapter` interface pattern |

**Pre-Phase 0 Gate**: ✅ PASSED - All principles satisfied

**Post-Phase 1 Re-evaluation**: ✅ PASSED - Design artifacts comply with all constitution principles

## Project Structure

### Documentation (this feature)

```text
specs/004-git-repository/
├── plan.md              # This file
├── research.md          # Phase 0 output
├── data-model.md        # Phase 1 output
├── quickstart.md        # Phase 1 output
├── contracts/           # Phase 1 output (OpenAPI schemas)
└── tasks.md             # Phase 2 output (/speckit.tasks command)
```

### Source Code (repository root)

```text
src/libraries/
├── Domain/
│   ├── Hexalith.GitStorage.Aggregates/
│   │   └── GitRepository.cs                        # Aggregate root
│   ├── Hexalith.GitStorage.Aggregates.Abstractions/
│   │   └── Enums/
│   │       └── GitRepositoryVisibility.cs          # Reuse or extend existing enum
│   │       └── GitRepositoryOrigin.cs              # Enum: Synced | CreatedViaApplication
│   │       └── GitRepositorySyncStatus.cs          # Enum: Synced | NotFoundOnRemote | SyncError
│   └── Hexalith.GitStorage.Events/
│       └── GitRepository/
│           ├── GitRepositoryEvent.cs               # Abstract base
│           ├── GitRepositoryAdded.cs               # Created via application
│           ├── GitRepositorySynced.cs              # Discovered from remote
│           ├── GitRepositoryDescriptionChanged.cs  # Metadata update
│           ├── GitRepositoryVisibilityChanged.cs   # Visibility change
│           ├── GitRepositoryDefaultBranchChanged.cs # Default branch change
│           ├── GitRepositoryDisabled.cs            # Lifecycle
│           ├── GitRepositoryEnabled.cs             # Lifecycle
│           └── GitRepositoryMarkedNotFound.cs      # Sync status update
├── Application/
│   ├── Hexalith.GitStorage.Commands/
│   │   └── GitRepository/
│   │       ├── GitRepositoryCommand.cs             # Abstract base
│   │       ├── AddGitRepository.cs                 # Create new
│   │       ├── SyncGitRepository.cs                # Sync operation
│   │       ├── ChangeGitRepositoryDescription.cs   # Update description
│   │       ├── ChangeGitRepositoryVisibility.cs    # Update visibility
│   │       ├── ChangeGitRepositoryDefaultBranch.cs # Update default branch
│   │       ├── DisableGitRepository.cs             # Disable
│   │       └── EnableGitRepository.cs              # Enable
│   ├── Hexalith.GitStorage.Requests/
│   │   └── GitRepository/
│   │       ├── GitRepositoryRequest.cs             # Abstract base
│   │       ├── GetGitRepositorySummaries.cs        # List query
│   │       ├── GetGitRepositoryDetails.cs          # Details query
│   │       ├── GitRepositorySummaryViewModel.cs    # List view model
│   │       └── GitRepositoryDetailsViewModel.cs    # Details view model
│   └── Hexalith.GitStorage.Projections/
│       └── GitRepository/
│           └── ProjectionHandlers/
│               ├── GitRepositorySummaryProjectionHandler.cs
│               └── GitRepositoryDetailsProjectionHandler.cs
├── Infrastructure/
│   └── Hexalith.GitStorage.ApiServer/
│       └── Controllers/
│           └── GitRepositoryIntegrationEventsController.cs
└── Presentation/
    ├── Hexalith.GitStorage.UI.Components/
    │   └── GitRepository/
    │       ├── GitRepositoryEditViewModel.cs
    │       └── GitRepositoryEditValidation.cs
    └── Hexalith.GitStorage.UI.Pages/
        └── GitRepository/
            ├── GitRepositoryIndex.razor
            └── GitRepositoryDetails.razor

test/
└── Hexalith.GitStorage.Tests/
    └── GitRepository/
        ├── AddGitRepositoryValidatorTests.cs
        ├── GitRepositoryTests.cs
        └── GitRepositoryEventTests.cs
```

**Structure Decision**: Following the established vertical slice architecture from GitOrganization and GitStorageAccount aggregates. Each layer is a separate NuGet package with clear responsibilities.

## Complexity Tracking

> No constitution violations requiring justification. Implementation follows existing patterns.

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| N/A | - | - |
