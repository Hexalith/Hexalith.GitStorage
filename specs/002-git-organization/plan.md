# Implementation Plan: Git Organization Entity

**Branch**: `002-git-organization` | **Date**: 2025-12-07 | **Spec**: [spec.md](./spec.md)
**Input**: Feature specification from `/specs/002-git-organization/spec.md`

## Summary

Add a GitOrganization entity that can be synchronized from remote Git servers (GitHub/Forgejo) or created via the application API. Organizations support CRUD operations with soft-delete, visibility settings (Public/Private/Internal), and bidirectional sync with remote servers via Dapr-managed event handlers.

**Key Enhancement**: Add `Visibility` property (Public | Private | Internal) to the existing GitOrganization implementation, updating all affected events, commands, view models, and UI.

## Technical Context

**Language/Version**: .NET 10 / C# 13
**Primary Dependencies**: Hexalith DDD/CQRS framework, Dapr, FluentValidation, Blazor InteractiveAuto
**Storage**: Azure Cosmos DB (event store via Hexalith), Redis (projections/cache)
**Testing**: xUnit + Shouldly + Moq
**Target Platform**: Linux containers / Windows server via .NET Aspire
**Project Type**: Modular monolith with clean architecture layers
**Performance Goals**: Sync ≤30s for 100 orgs; CRUD ≤10s; Read ≤1s (per spec SC-001..SC-003)
**Constraints**: Standard application logging only (NFR-001); soft-delete only (no hard delete)
**Scale/Scope**: Feature adds ~15-20 files across Domain, Application, Infrastructure, Presentation layers

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

| Principle | Status | Notes |
|-----------|--------|-------|
| I. Domain-Driven Design First | ✅ PASS | GitOrganization aggregate with events, business rules in domain layer |
| II. CQRS Separation | ✅ PASS | Commands (Add, ChangeDescription, Disable, Enable, Sync) separate from Requests (GetDetails, GetSummaries) |
| III. Event Sourcing Compliance | ✅ PASS | All state changes via immutable events with [PolymorphicSerialization] |
| IV. Clean Architecture Layers | ✅ PASS | Domain → Application → Infrastructure → Presentation hierarchy respected |
| V. Code Quality Standards | ✅ PASS | File headers, file-scoped namespaces, primary constructors, XML docs required |
| VI. Test-First Development | ✅ PASS | Tests for aggregate behavior, event serialization, validators |
| VII. Provider Abstraction | ✅ PASS | Git provider operations abstracted; tokens not logged |

**Gate Result**: PASS - No violations requiring justification.

## Project Structure

### Documentation (this feature)

```text
specs/002-git-organization/
├── plan.md              # This file
├── research.md          # Phase 0 output
├── data-model.md        # Phase 1 output
├── quickstart.md        # Phase 1 output
├── contracts/           # Phase 1 output (OpenAPI schemas)
└── tasks.md             # Phase 2 output (via /speckit.tasks)
```

### Source Code (repository root)

```text
src/libraries/
├── Domain/
│   ├── Hexalith.GitStorage.Aggregates/
│   │   └── GitOrganization.cs                    # Aggregate (UPDATE: add Visibility)
│   ├── Hexalith.GitStorage.Aggregates.Abstractions/
│   │   ├── GitOrganizationDomainHelper.cs        # Domain constants
│   │   └── Enums/
│   │       ├── GitOrganizationOrigin.cs          # Synced | CreatedViaApplication
│   │       ├── GitOrganizationSyncStatus.cs      # Synced | NotFoundOnRemote | SyncError
│   │       └── GitOrganizationVisibility.cs      # NEW: Public | Private | Internal
│   ├── Hexalith.GitStorage.Events/GitOrganization/
│   │   ├── GitOrganizationEvent.cs               # Base event class
│   │   ├── GitOrganizationAdded.cs               # UPDATE: add Visibility
│   │   ├── GitOrganizationSynced.cs              # UPDATE: add Visibility
│   │   ├── GitOrganizationDescriptionChanged.cs  # OK (rename might update visibility)
│   │   ├── GitOrganizationVisibilityChanged.cs   # NEW: event for visibility changes
│   │   ├── GitOrganizationDisabled.cs            # EXISTS
│   │   ├── GitOrganizationEnabled.cs             # EXISTS
│   │   └── GitOrganizationMarkedNotFound.cs      # EXISTS
│   └── Hexalith.GitStorage.Localizations/
│       ├── GitOrganization.resx                  # UPDATE: add visibility labels
│       └── GitOrganization.fr.resx               # UPDATE: French labels
├── Application/
│   ├── Hexalith.GitStorage.Commands/GitOrganization/
│   │   ├── GitOrganizationCommand.cs             # Base command class
│   │   ├── AddGitOrganization.cs                 # UPDATE: add Visibility
│   │   ├── AddGitOrganizationValidator.cs        # UPDATE: validate Visibility
│   │   ├── ChangeGitOrganizationDescription.cs   # EXISTS
│   │   ├── ChangeGitOrganizationVisibility.cs    # NEW: command for visibility change
│   │   ├── ChangeGitOrganizationVisibilityValidator.cs # NEW: validator
│   │   ├── SyncGitOrganizations.cs               # EXISTS (visibility comes from remote)
│   │   ├── DisableGitOrganization.cs             # EXISTS
│   │   └── EnableGitOrganization.cs              # EXISTS
│   ├── Hexalith.GitStorage.Requests/GitOrganization/
│   │   ├── GitOrganizationRequest.cs             # Base request class
│   │   ├── GetGitOrganizationDetails.cs          # EXISTS
│   │   ├── GetGitOrganizationSummaries.cs        # EXISTS
│   │   ├── GitOrganizationDetailsViewModel.cs    # UPDATE: add Visibility
│   │   └── GitOrganizationSummaryViewModel.cs    # UPDATE: add Visibility
│   ├── Hexalith.GitStorage.Projections/
│   │   └── ProjectionHandlers/                   # UPDATE existing + NEW visibility handler
│   └── Hexalith.GitStorage/
│       ├── CommandHandlers/                      # UPDATE: handle visibility in AddGitOrganization
│       └── EventHandlers/                        # Sync to remote on visibility change
├── Infrastructure/
│   ├── Hexalith.GitStorage.ApiServer/Controllers/
│   │   └── GitOrganizationIntegrationEventsController.cs  # EXISTS
│   ├── Hexalith.GitStorage.Servers/Helpers/
│   │   └── GitOrganizationWebApiHelpers.cs       # EXISTS
│   └── Hexalith.GitStorage.WebServer/Controllers/
│       └── GitOrganizationsController.cs         # EXISTS (API endpoints)
└── Presentation/
    └── Hexalith.GitStorage.UI.Pages/GitOrganization/
        ├── GitOrganizationIndex.razor            # EXISTS (list view)
        ├── GitOrganizationDetails.razor          # UPDATE: show Visibility
        ├── GitOrganizationEditViewModel.cs       # UPDATE: add Visibility
        └── GitOrganizationEditValidation.cs      # UPDATE: validate Visibility

test/Hexalith.GitStorage.Tests/Domains/
├── Aggregates/GitOrganizationTests.cs            # UPDATE: test Visibility
├── Commands/GitOrganizationCommandTests.cs       # UPDATE: test Visibility
├── Commands/GitOrganizationValidatorTests.cs     # UPDATE: test Visibility validation
└── Events/GitOrganizationEventTests.cs           # UPDATE: test Visibility serialization
```

**Structure Decision**: Existing clean architecture pattern with Domain, Application, Infrastructure, Presentation layers. Feature extends existing GitOrganization entity with Visibility property across all layers.

## Complexity Tracking

> No Constitution violations requiring justification.

| Item | Justification |
|------|---------------|
| N/A | All changes follow established patterns |
