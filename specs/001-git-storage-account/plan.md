# Implementation Plan: Git Storage Account Entity

**Branch**: `001-git-storage-account` | **Date**: 2025-12-01 | **Spec**: [spec.md](spec.md)
**Input**: Feature specification from `/specs/001-git-storage-account/spec.md`

## Summary

Implement the GitStorageAccount entity following DDD/CQRS/Event Sourcing patterns to manage connection settings for external git repository servers. The entity supports CRUD operations (Create, Read, Update) plus Enable/Disable state management, with no hard-delete capability. All state changes are captured as domain events, enabling full audit history and temporal queries.

## Technical Context

**Language/Version**: .NET 10 / C# 13 (latest features enabled)
**Primary Dependencies**: Hexalith.Application v1.71.1, Hexalith.Domains v1.2.0, Hexalith.PolymorphicSerializations v1.9.0, Dapr.AspNetCore v1.15.2
**Storage**: Azure Cosmos DB (event store) + Dapr State Store (projections/read models)
**Testing**: xUnit v2.9.3 + Shouldly v4.3.0 + Moq v4.20.72
**Target Platform**: Linux server (containerized), Windows development
**Project Type**: Multi-project DDD layered architecture (Domain, Application, Infrastructure, Presentation)
**Performance Goals**: Account retrieval within 1 second (SC-002), account creation under 30 seconds (SC-001)
**Constraints**: Role-based authorization (Admin role required), optimistic concurrency for updates
**Scale/Scope**: Administrative accounts (low volume, 10s-100s of accounts expected)

## Constitution Check

*GATE: Must pass before Phase 0 research. Re-check after Phase 1 design.*

### Principle Compliance Matrix (Pre-Phase 0)

| Principle | Status | Evidence |
|-----------|--------|----------|
| **I. DDD First** | ✅ PASS | Business logic in GitStorageAccount aggregate; Events represent facts (GitStorageAccountAdded, etc.) |
| **II. CQRS Separation** | ✅ PASS | Commands: AddGitStorageAccount, etc.; Requests: GetGitStorageAccountDetails, etc. |
| **III. Event Sourcing** | ✅ PASS | State changes via immutable events; Aggregate reconstructable from event history |
| **IV. Clean Architecture** | ✅ PASS | Layers: Domain → Application → Infrastructure → Presentation |
| **V. Code Quality** | ✅ PASS | Copyright headers, file-scoped namespaces, primary constructors, XML docs, DataContract/DataMember |
| **VI. Test-First** | ✅ PASS | Tests for aggregate Apply methods, event serialization, command validation |
| **VII. Provider Abstraction** | N/A | No provider-specific integration in this entity (future feature) |

### NON-NEGOTIABLE Gates

- [x] CQRS: Commands and Requests strictly separated
- [x] Code Quality: All standards enforced (headers, namespaces, docs, serialization attributes)

**Gate Status**: ✅ PASSED - Ready for Phase 0

### Post-Design Re-evaluation (Phase 1 Complete)

| Principle | Status | Design Artifact Evidence |
|-----------|--------|--------------------------|
| **I. DDD First** | ✅ PASS | [data-model.md](data-model.md): Aggregate with Apply methods, 4 domain events |
| **II. CQRS Separation** | ✅ PASS | [contracts/openapi.yaml](contracts/openapi.yaml): POST for commands, GET for queries |
| **III. Event Sourcing** | ✅ PASS | [contracts/domain-events.json](contracts/domain-events.json): Immutable event schemas |
| **IV. Clean Architecture** | ✅ PASS | Project Structure section: Domain → Application → Infrastructure → Presentation |
| **V. Code Quality** | ✅ PASS | [research.md](research.md): All serialization attributes specified per pattern |
| **VI. Test-First** | ✅ PASS | [quickstart.md](quickstart.md): Test examples and coverage guidance |
| **VII. Provider Abstraction** | N/A | Not applicable for this entity |

**Post-Design Gate Status**: ✅ PASSED - Ready for Phase 2 (Task Generation)

## Project Structure

### Documentation (this feature)

```text
specs/001-git-storage-account/
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
│   ├── Hexalith.GitStorage.Events/
│   │   └── GitStorageAccount/           # Domain events (Added, DescriptionChanged, Enabled, Disabled)
│   ├── Hexalith.GitStorage.Aggregates/
│   │   └── GitStorageAccount.cs         # Aggregate with Apply methods
│   └── Hexalith.GitStorage.Aggregates.Abstractions/
│       ├── GitStorageAccountDomainHelper.cs
│       └── Enums/GitStorageAccountStatus.cs
│
├── Application/
│   ├── Hexalith.GitStorage.Commands/
│   │   └── GitStorageAccount/           # Commands (Add, ChangeDescription, Enable, Disable)
│   ├── Hexalith.GitStorage.Requests/
│   │   └── GitStorageAccount/           # Requests (GetDetails, GetSummaries, GetIds)
│   ├── Hexalith.GitStorage.Projections/
│   │   └── ProjectionHandlers/Details/  # Event handlers updating read models
│   └── Hexalith.GitStorage/
│       └── CommandHandlers/             # Command handler registration
│
├── Infrastructure/
│   ├── Hexalith.GitStorage.ApiServer/   # API controllers, module configuration
│   ├── Hexalith.GitStorage.WebServer/   # Web server module
│   └── Hexalith.GitStorage.Servers/     # API helpers
│
└── Presentation/
    ├── Hexalith.GitStorage.UI.Components/  # Blazor components
    └── Hexalith.GitStorage.UI.Pages/       # Blazor pages

test/
└── Hexalith.GitStorage.Tests/
    └── Domains/
        ├── Aggregates/GitStorageAccountTests.cs
        ├── Commands/GitStorageAccountCommandTests.cs
        └── Events/GitStorageAccountEventTests.cs
```

**Structure Decision**: Hexalith layered DDD architecture - Domain/Application/Infrastructure/Presentation layers following the Entity Creation Order defined in the constitution.

## Complexity Tracking

> No violations identified. Implementation follows established patterns with no additional complexity required.

| Violation | Why Needed | Simpler Alternative Rejected Because |
|-----------|------------|-------------------------------------|
| *None* | N/A | N/A |
