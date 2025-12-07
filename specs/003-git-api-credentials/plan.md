# Implementation Plan: Git API Credentials

**Branch**: `003-git-api-credentials` | **Date**: 2025-12-07 | **Spec**: [spec.md](spec.md)
**Input**: Feature specification from `/specs/003-git-api-credentials/spec.md`

## Summary

Extend the GitStorageAccount aggregate with API connection fields (Server URL, Access Token, Provider Type) to enable authentication and communication with GitHub or Forgejo servers. This requires new domain events, commands, projection updates, and UI modifications following the existing CQRS/Event Sourcing patterns.

## Technical Context

**Language/Version**: .NET 10 / C# 13
**Primary Dependencies**: Hexalith Framework (DDD/CQRS), Dapr, .NET Aspire, Blazor InteractiveAuto
**Storage**: Azure Cosmos DB (event store), Event Sourcing
**Testing**: xUnit + Shouldly + Moq
**Target Platform**: ASP.NET Core Web Server + Blazor WebAssembly
**Project Type**: DDD/CQRS Module (multi-project solution)
**Performance Goals**: Account details page loads within 2 seconds (per SC-005)
**Constraints**: Access tokens must never be displayed in full (masked with last 4 chars visible)
**Scale/Scope**: Extension to existing GitStorageAccount aggregate

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

| Principle | Status | Evidence |
|-----------|--------|----------|
| I. Domain-Driven Design First | ✅ PASS | Extending existing aggregate with new domain events |
| II. CQRS Separation | ✅ PASS | Commands: ChangeGitStorageAccountApiCredentials, ClearGitStorageAccountApiCredentials; Events updated via projection handlers |
| III. Event Sourcing Compliance | ✅ PASS | New events: GitStorageAccountApiCredentialsChanged, GitStorageAccountApiCredentialsCleared |
| IV. Clean Architecture Layers | ✅ PASS | Following entity creation order: Events → Aggregate → Commands → Requests → Projections → UI |
| V. Code Quality Standards | ✅ PASS | Will use primary constructors, file-scoped namespaces, XML docs, DataMember attributes |
| VI. Test-First Development | ✅ PASS | Unit tests for aggregate Apply methods and validators |
| VII. Provider Abstraction | ✅ PASS | GitServerProviderType enum abstracts provider specifics; tokens not logged/exposed in events |

## Project Structure

### Documentation (this feature)

```text
specs/003-git-api-credentials/
├── plan.md              # This file (/speckit.plan command output)
├── research.md          # Phase 0 output (/speckit.plan command)
├── data-model.md        # Phase 1 output (/speckit.plan command)
├── quickstart.md        # Phase 1 output (/speckit.plan command)
├── contracts/           # Phase 1 output (/speckit.plan command)
└── tasks.md             # Phase 2 output (/speckit.tasks command - NOT created by /speckit.plan)
```

### Source Code (repository root)

```text
src/libraries/
├── Domain/
│   ├── Hexalith.GitStorage.Aggregates/
│   │   └── GitStorageAccount.cs              # Extended with ApiCredentials fields
│   ├── Hexalith.GitStorage.Aggregates.Abstractions/
│   │   └── Enums/
│   │       └── GitServerProviderType.cs      # NEW: GitHub, Forgejo, Gitea, Generic
│   └── Hexalith.GitStorage.Events/
│       └── GitStorageAccount/
│           ├── GitStorageAccountApiCredentialsChanged.cs  # NEW
│           └── GitStorageAccountApiCredentialsCleared.cs  # NEW
├── Application/
│   ├── Hexalith.GitStorage.Commands/
│   │   └── GitStorageAccount/
│   │       ├── AddGitStorageAccount.cs                    # Extended with optional API credentials
│   │       ├── ChangeGitStorageAccountApiCredentials.cs   # NEW
│   │       └── ClearGitStorageAccountApiCredentials.cs    # NEW
│   ├── Hexalith.GitStorage.Requests/
│   │   └── GitStorageAccount/
│   │       └── GitStorageAccountDetailsViewModel.cs       # Extended with API fields
│   └── Hexalith.GitStorage.Projections/
│       └── ProjectionHandlers/Details/
│           ├── GitStorageAccountApiCredentialsChangedOnDetailsProjectionHandler.cs  # NEW
│           └── GitStorageAccountApiCredentialsClearedOnDetailsProjectionHandler.cs  # NEW
└── Presentation/
    └── Hexalith.GitStorage.UI.Pages/
        └── GitStorageAccount/
            └── GitStorageAccountEditViewModel.cs  # Extended with API credential fields

test/
└── Hexalith.GitStorage.UnitTests/
    ├── Aggregates/
    │   └── GitStorageAccountApiCredentialsTests.cs  # NEW
    └── Commands/
        └── ChangeGitStorageAccountApiCredentialsValidatorTests.cs  # NEW
```

**Structure Decision**: Extending existing DDD/CQRS layer structure. No new projects required - only new files and modifications to existing files.

## Complexity Tracking

> No constitution violations requiring justification. Feature follows established patterns.

| Aspect | Complexity Level | Rationale |
|--------|------------------|-----------|
| Event design | Low | Standard event pattern matching existing events |
| Aggregate extension | Low | Adding fields and Apply overloads to existing aggregate |
| Token masking | Low | Simple string manipulation in ViewModel (last 4 chars) |
| UI changes | Low | Extending existing edit/details forms with 3 fields |

---

## Post-Design Constitution Re-Check

*Re-evaluated after Phase 1 design completion.*

| Principle | Status | Post-Design Evidence |
|-----------|--------|---------------------|
| I. Domain-Driven Design First | ✅ PASS | Business rules encapsulated in aggregate (credential validation, clearing logic) |
| II. CQRS Separation | ✅ PASS | Commands produce events; ViewModels read-only with computed MaskedAccessToken |
| III. Event Sourcing Compliance | ✅ PASS | Events are immutable records with PolymorphicSerialization; Apply methods are pure |
| IV. Clean Architecture Layers | ✅ PASS | Token masking in Presentation layer (ViewModel), not Domain |
| V. Code Quality Standards | ✅ PASS | All contracts use DataMember ordering, primary constructors, XML docs |
| VI. Test-First Development | ✅ PASS | Test files defined for aggregate Apply and command validators |
| VII. Provider Abstraction | ✅ PASS | GitServerProviderType enum enables provider-specific handling without leaking to Domain |

**Gate Status**: ✅ ALL GATES PASSED - Ready for Phase 2 (/speckit.tasks)

---

## Generated Artifacts

| Artifact | Path | Status |
|----------|------|--------|
| Implementation Plan | [plan.md](plan.md) | ✅ Complete |
| Research Decisions | [research.md](research.md) | ✅ Complete |
| Data Model | [data-model.md](data-model.md) | ✅ Complete |
| API Contract | [contracts/git-storage-account-api-credentials.yaml](contracts/git-storage-account-api-credentials.yaml) | ✅ Complete |
| Quickstart Guide | [quickstart.md](quickstart.md) | ✅ Complete |

## Next Steps

Run `/speckit.tasks` to generate actionable implementation tasks based on this plan.
