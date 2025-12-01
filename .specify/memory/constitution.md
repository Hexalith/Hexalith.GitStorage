<!--
SYNC IMPACT REPORT
==================
Version change: 0.0.0 → 1.0.0 (MAJOR - Initial constitution creation)

Modified Principles: N/A (new document)

Added Sections:
- Core Principles (7 principles)
- Architecture Standards
- Development Workflow
- Governance

Removed Sections: N/A

Templates Requiring Updates:
- ✅ plan-template.md - Constitution Check section compatible
- ✅ spec-template.md - Requirements alignment verified
- ✅ tasks-template.md - Task categorization compatible

Follow-up TODOs: None
-->

# Hexalith.GitStorage Constitution

## Core Principles

### I. Domain-Driven Design First

All business logic MUST reside in the Domain layer through aggregates and domain events. Domain entities are the single source of truth for business rules.

- Aggregates MUST encapsulate all invariants and business rules
- Domain events MUST represent facts that have occurred (past tense naming: `{Entity}{PastTenseVerb}`)
- Value objects MUST be immutable and equality-based
- The Domain layer MUST have zero dependencies on Infrastructure or Presentation layers

**Rationale**: DDD ensures business logic remains testable, portable, and decoupled from technical concerns.

### II. CQRS Separation (NON-NEGOTIABLE)

Commands (writes) and Requests (reads) MUST be strictly separated. No query operation may modify state; no command operation may return domain data beyond identifiers.

- Commands MUST follow `{Verb}{Entity}` naming pattern
- Requests MUST follow `Get{Entity}{Details|Summaries}` naming pattern
- Command handlers produce events; Request handlers read projections
- Projections MUST be the only read models for queries

**Rationale**: CQRS enables independent scaling, optimized read models, and clear audit trails through event sourcing.

### III. Event Sourcing Compliance

State changes MUST be captured as an immutable sequence of domain events. Aggregates MUST be reconstructable from their event history.

- Events MUST be immutable records with `[PolymorphicSerialization]` attribute
- Events MUST include `[DataMember(Order = N)]` for deterministic serialization
- Aggregate `Apply` methods MUST be pure functions returning new state
- Event stores MUST never delete or modify existing events

**Rationale**: Event sourcing provides complete audit history, temporal queries, and reliable state reconstruction.

### IV. Clean Architecture Layers

The codebase MUST maintain strict layer boundaries with dependencies flowing inward only: Presentation → Infrastructure → Application → Domain.

| Layer | Contains | Dependencies |
|-------|----------|--------------|
| Domain | Aggregates, Events, Value Objects | None |
| Application | Commands, Requests, Projections, Handlers | Domain |
| Infrastructure | API Server, Web Server, WebApp, Dapr Integration | Application, Domain |
| Presentation | UI Components, Pages | Application (via contracts) |

**Rationale**: Clean Architecture ensures testability, maintainability, and the ability to swap technical implementations.

### V. Code Quality Standards (NON-NEGOTIABLE)

All C# code MUST adhere to project coding standards without exception.

- MUST include copyright header on all `.cs` files (ITANEO MIT license)
- MUST use file-scoped namespaces (`namespace X;`)
- MUST use primary constructors for records and classes
- MUST include XML documentation on all public/protected/internal members
- MUST use `[DataContract]` and `[DataMember(Order = N)]` for serialization
- MUST follow naming conventions: `_camelCase` for private fields, PascalCase for public members

**Rationale**: Consistent code style reduces cognitive load, prevents serialization bugs, and enables automated tooling.

### VI. Test-First Development

Tests MUST be written for domain logic, aggregate behavior, and event serialization. The testing stack is xUnit + Shouldly + Moq.

- Test naming: `{Method}_{Scenario}_{ExpectedResult}`
- Tests MUST follow Arrange-Act-Assert pattern
- Aggregate tests MUST verify event application and state transitions
- Serialization tests MUST verify round-trip JSON compatibility

**Rationale**: Tests document behavior, prevent regressions, and validate event sourcing correctness.

### VII. Provider Abstraction

Git provider integrations (GitHub, Forgejo) MUST be abstracted behind interfaces. No provider-specific code may leak into Domain or Application layers.

- Provider adapters MUST implement `IGitProviderAdapter` interface
- Configuration MUST support multiple provider instances
- Authentication tokens MUST never be logged or exposed in events

**Rationale**: Provider abstraction enables multi-provider support and simplifies testing with mocks.

## Architecture Standards

### Technology Stack

- **Runtime**: .NET 10 / C# 13 (use latest language features)
- **UI**: Blazor InteractiveAuto (SSR + WebAssembly)
- **Orchestration**: Dapr with .NET Aspire
- **Persistence**: Azure Cosmos DB (event store), Redis (state/cache)
- **Testing**: xUnit, Shouldly, Moq, Coverlet

### Entity Creation Order

When adding new domain entities, follow this sequence:

1. Events → `src/libraries/Domain/Hexalith.GitStorage.Events/{Entity}/`
2. Aggregate → `src/libraries/Domain/Hexalith.GitStorage.Aggregates/`
3. Commands → `src/libraries/Application/Hexalith.GitStorage.Commands/{Entity}/`
4. Requests → `src/libraries/Application/Hexalith.GitStorage.Requests/{Entity}/`
5. Projections → `src/libraries/Application/Hexalith.GitStorage.Projections/`
6. UI Components → `src/libraries/Presentation/Hexalith.GitStorage.UI.Components/`
7. UI Pages → `src/libraries/Presentation/Hexalith.GitStorage.UI.Pages/`

### Protected Artifacts

The following MUST NOT be modified without explicit approval:

- `Hexalith.Builds/` - Shared build configurations (submodule)
- `HexalithApp/` - Base application framework (submodule)
- `DOCUMENTATION.ai.md` - AI prompt configurations

## Development Workflow

### Pull Request Requirements

- All PRs MUST pass automated builds and tests
- All PRs MUST include appropriate test coverage for new functionality
- All PRs MUST follow commit message conventions
- Constitution compliance MUST be verified before merge

### Quality Gates

1. **Build**: Solution MUST compile without warnings
2. **Tests**: All tests MUST pass
3. **Style**: StyleCop and global analyzers MUST report no violations
4. **Documentation**: Public APIs MUST have XML documentation

## Governance

This constitution supersedes all other development practices for Hexalith.GitStorage. Amendments require:

1. Documented rationale for the change
2. Impact analysis on existing code and dependent templates
3. Version increment following semantic versioning:
   - MAJOR: Breaking changes to principles or governance
   - MINOR: New principles or materially expanded guidance
   - PATCH: Clarifications, wording, non-semantic refinements
4. Update to all dependent templates and documentation

All code reviews MUST verify compliance with these principles. Complexity beyond these standards MUST be justified in the implementation plan's "Complexity Tracking" section.

For runtime development guidance, refer to `CLAUDE.md`.

**Version**: 1.0.0 | **Ratified**: 2025-12-01 | **Last Amended**: 2025-12-01
