# Implementation Plan: GitOrganization Entity

**Branch**: `002-git-organization` | **Date**: 2025-12-07 | **Spec**: [spec.md](spec.md)
**Input**: Feature specification from `/specs/002-git-organization/spec.md`

## Summary

Implement the GitOrganization entity following DDD/CQRS/Event Sourcing patterns. Organizations can be synchronized from remote Git servers (GitHub/Forgejo) or created via the application API with automatic provisioning on the remote server. The implementation follows the existing GitStorageAccount patterns with dual initialization events, soft-delete support, and role-based authorization.

## Technical Context

**Language/Version**: .NET 10 / C# 13
**Primary Dependencies**: Hexalith Framework, Dapr, FluentValidation, Blazor InteractiveAuto
**Storage**: Azure Cosmos DB (event store), Redis (state/cache via Dapr)
**Testing**: xUnit + Shouldly + Moq
**Target Platform**: Linux server (Docker/Kubernetes), WebAssembly (Blazor client)
**Project Type**: Web application (Hexalith modular architecture)
**Performance Goals**: Sync 100 organizations < 30 seconds, CRUD operations < 10 seconds (per SC-001, SC-002)
**Constraints**: Event sourced aggregate, immutable events, role-based authorization
**Scale/Scope**: Supports multiple Git Storage Accounts with thousands of organizations each

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

| Principle | Status | Evidence |
|-----------|--------|----------|
| I. DDD First | ✅ PASS | GitOrganization aggregate encapsulates business rules; events represent facts (GitOrganizationAdded, GitOrganizationSynced) |
| II. CQRS Separation | ✅ PASS | Commands (AddGitOrganization) and Requests (GetGitOrganizationDetails) are strictly separated; projections for read models |
| III. Event Sourcing | ✅ PASS | All state changes via immutable events with [PolymorphicSerialization] and DataMember ordering |
| IV. Clean Architecture | ✅ PASS | Layer dependencies flow inward: UI → Infrastructure → Application → Domain |
| V. Code Quality | ✅ PASS | Following copyright headers, file-scoped namespaces, primary constructors, XML docs |
| VI. Test-First | ✅ PASS | Test patterns defined: aggregate tests, serialization tests, handler tests |
| VII. Provider Abstraction | ✅ PASS | IGitProviderAdapter interface for GitHub/Forgejo; no provider code in Domain/Application |

**Gate Result**: PASS - No violations. Proceeding with implementation.

## Project Structure

### Documentation (this feature)

```text
specs/002-git-organization/
├── plan.md              # This file
├── research.md          # Phase 0 output - architecture decisions
├── data-model.md        # Phase 1 output - entity definitions
├── quickstart.md        # Phase 1 output - implementation guide
├── contracts/           # Phase 1 output - OpenAPI specification
│   └── api.yaml         # REST API contract
└── tasks.md             # Phase 2 output (/speckit.tasks)
```

### Source Code (repository root)

```text
src/libraries/
├── Domain/
│   ├── Hexalith.GitStorage.Aggregates/
│   │   └── GitOrganization.cs                    # Aggregate root
│   ├── Hexalith.GitStorage.Aggregates.Abstractions/
│   │   ├── Enums/
│   │   │   ├── GitOrganizationOrigin.cs          # Origin enum
│   │   │   └── GitOrganizationSyncStatus.cs      # Sync status enum
│   │   └── GitOrganizationDomainHelper.cs        # Domain helper
│   └── Hexalith.GitStorage.Events/
│       └── GitOrganization/
│           ├── GitOrganizationEvent.cs           # Base event
│           ├── GitOrganizationAdded.cs           # API creation event
│           ├── GitOrganizationSynced.cs          # Sync discovery event
│           ├── GitOrganizationDescriptionChanged.cs
│           ├── GitOrganizationMarkedNotFound.cs
│           ├── GitOrganizationDisabled.cs
│           └── GitOrganizationEnabled.cs
├── Application/
│   ├── Hexalith.GitStorage.Commands/
│   │   └── GitOrganization/
│   │       ├── GitOrganizationCommand.cs         # Base command
│   │       ├── AddGitOrganization.cs
│   │       ├── ChangeGitOrganizationDescription.cs
│   │       ├── DisableGitOrganization.cs
│   │       ├── EnableGitOrganization.cs
│   │       └── SyncGitOrganizations.cs
│   ├── Hexalith.GitStorage.Requests/
│   │   └── GitOrganization/
│   │       ├── GitOrganizationRequest.cs         # Base request
│   │       ├── GetGitOrganizationDetails.cs
│   │       ├── GetGitOrganizationSummaries.cs
│   │       ├── GitOrganizationDetailsViewModel.cs
│   │       └── GitOrganizationSummaryViewModel.cs
│   └── Hexalith.GitStorage.Projections/
│       └── GitOrganization/
│           ├── GitOrganizationDetailsProjectionHandler.cs
│           └── GitOrganizationSummaryProjectionHandler.cs
└── Presentation/
    ├── Hexalith.GitStorage.UI.Components/
    │   └── GitOrganizations/
    │       ├── GitOrganizationCard.razor
    │       ├── GitOrganizationForm.razor
    │       └── GitOrganizationList.razor
    └── Hexalith.GitStorage.UI.Pages/
        └── GitOrganizations/
            ├── GitOrganizationsPage.razor
            └── GitOrganizationDetailsPage.razor

test/
└── Hexalith.GitStorage.UnitTests/
    └── GitOrganization/
        ├── GitOrganizationAggregateTests.cs
        ├── GitOrganizationEventSerializationTests.cs
        ├── GitOrganizationCommandValidationTests.cs
        └── GitOrganizationProjectionTests.cs
```

**Structure Decision**: Following existing Hexalith module architecture with clean separation between Domain, Application, and Presentation layers. Entity creation follows the order defined in the constitution: Events → Aggregate → Commands → Requests → Projections → UI.

## Complexity Tracking

No complexity violations detected. All patterns follow established Hexalith conventions:

- Single aggregate (GitOrganization) - no unnecessary abstractions
- Standard CQRS/ES event flow - matching GitStorageAccount pattern
- Provider abstraction reuses existing IGitProviderAdapter interface

## Generated Artifacts

| Artifact | Path | Status |
|----------|------|--------|
| Research | [research.md](research.md) | ✅ Complete |
| Data Model | [data-model.md](data-model.md) | ✅ Complete |
| API Contracts | [contracts/api.yaml](contracts/api.yaml) | ✅ Complete |
| Quickstart | [quickstart.md](quickstart.md) | ✅ Complete |

## Next Steps

Run `/speckit.tasks` to generate the implementation task list from this plan.
