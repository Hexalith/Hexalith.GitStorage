# Tasks: Git Storage Account Entity

**Input**: Design documents from `/specs/001-git-storage-account/`
**Prerequisites**: plan.md ✓, spec.md ✓, research.md ✓, data-model.md ✓, contracts/ ✓

**Tests**: Included per CLAUDE.md testing requirements and quickstart.md guidance.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3, US4)
- Include exact file paths in descriptions

## Path Conventions

Based on plan.md project structure:
- **Domain Events**: `src/libraries/Domain/Hexalith.GitStorage.Events/GitStorageAccount/`
- **Aggregates**: `src/libraries/Domain/Hexalith.GitStorage.Aggregates/`
- **Aggregate Abstractions**: `src/libraries/Domain/Hexalith.GitStorage.Aggregates.Abstractions/`
- **Commands**: `src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/`
- **Requests**: `src/libraries/Application/Hexalith.GitStorage.Requests/GitStorageAccount/`
- **Projections**: `src/libraries/Application/Hexalith.GitStorage.Projections/`
- **API Server**: `src/libraries/Infrastructure/Hexalith.GitStorage.ApiServer/`
- **Tests**: `test/Hexalith.GitStorage.Tests/`

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and domain helper setup

- [X] T001 Create GitStorageAccountDomainHelper.cs with aggregate name constant in src/libraries/Domain/Hexalith.GitStorage.Aggregates.Abstractions/GitStorageAccountDomainHelper.cs
- [X] T002 [P] Verify Directory.Packages.props has Hexalith.Application v1.71.1, Hexalith.Domains v1.2.0, Hexalith.PolymorphicSerializations v1.9.0

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core domain infrastructure that MUST be complete before ANY user story can be implemented

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [X] T003 Create GitStorageAccountEvent base record in src/libraries/Domain/Hexalith.GitStorage.Events/GitStorageAccount/GitStorageAccountEvent.cs
- [X] T004 [P] Create GitStorageAccountAdded event in src/libraries/Domain/Hexalith.GitStorage.Events/GitStorageAccount/GitStorageAccountAdded.cs
- [X] T005 [P] Create GitStorageAccountDescriptionChanged event in src/libraries/Domain/Hexalith.GitStorage.Events/GitStorageAccount/GitStorageAccountDescriptionChanged.cs
- [X] T006 [P] Create GitStorageAccountEnabled event in src/libraries/Domain/Hexalith.GitStorage.Events/GitStorageAccount/GitStorageAccountEnabled.cs
- [X] T007 [P] Create GitStorageAccountDisabled event in src/libraries/Domain/Hexalith.GitStorage.Events/GitStorageAccount/GitStorageAccountDisabled.cs
- [X] T008 Create GitStorageAccount aggregate record with Apply methods in src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitStorageAccount.cs
- [X] T009 Create GitStorageAccountCommand base record in src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/GitStorageAccountCommand.cs
- [X] T010 Create GitStorageAccountRequest base record in src/libraries/Application/Hexalith.GitStorage.Requests/GitStorageAccount/GitStorageAccountRequest.cs
- [X] T011 [P] Create GitStorageAccountDetailsViewModel in src/libraries/Application/Hexalith.GitStorage.Requests/GitStorageAccount/ViewModels/GitStorageAccountDetailsViewModel.cs
- [X] T012 [P] Create GitStorageAccountSummaryViewModel in src/libraries/Application/Hexalith.GitStorage.Requests/GitStorageAccount/ViewModels/GitStorageAccountSummaryViewModel.cs
- [X] T013 Create GitStorageAccountEventTests for event serialization in test/Hexalith.GitStorage.Tests/Domains/Events/GitStorageAccountEventTests.cs

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel

---

## Phase 3: User Story 1 - Create Git Storage Account (Priority: P1) 🎯 MVP

**Goal**: Enable administrators to create a new Git Storage Account with valid connection settings

**Independent Test**: Create a new account with Id, Name, Comments and verify it is persisted and retrievable via aggregate state

### Tests for User Story 1

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [X] T014 [P] [US1] Create aggregate Apply test for GitStorageAccountAdded in test/Hexalith.GitStorage.Tests/Domains/Aggregates/GitStorageAccountTests.cs
- [X] T015 [P] [US1] Create command serialization test for AddGitStorageAccount in test/Hexalith.GitStorage.Tests/Domains/Commands/GitStorageAccountCommandTests.cs

### Implementation for User Story 1

- [X] T016 [US1] Create AddGitStorageAccount command in src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/AddGitStorageAccount.cs
- [X] T017 [US1] Implement ApplyEvent(GitStorageAccountAdded) method in GitStorageAccount aggregate in src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitStorageAccount.cs
- [X] T018 [US1] Create GitStorageAccountAddedOnDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Details/GitStorageAccountAddedOnDetailsProjectionHandler.cs
- [X] T019 [US1] Create AddGitStorageAccountHandler in src/libraries/Application/Hexalith.GitStorage/CommandHandlers/AddGitStorageAccountHandler.cs (with validation: Id required, Name required)
- [X] T020 [US1] Add POST endpoint for AddGitStorageAccount command in API controller in src/libraries/Infrastructure/Hexalith.GitStorage.ApiServer/Controllers/GitStorageAccountController.cs

**Checkpoint**: User Story 1 complete - can create Git Storage Accounts via API

---

## Phase 4: User Story 2 - View Git Storage Account Details (Priority: P2)

**Goal**: Enable administrators to view details of existing Git Storage Accounts for verification and auditing

**Independent Test**: Retrieve an existing account by Id and verify all stored details (Name, Comments, Disabled status) are displayed correctly

### Tests for User Story 2

- [X] T021 [P] [US2] Create request serialization test for GetGitStorageAccountDetails in test/Hexalith.GitStorage.Tests/Domains/Requests/GitStorageAccountRequestTests.cs

### Implementation for User Story 2

- [X] T022 [US2] Create GetGitStorageAccountDetails request in src/libraries/Application/Hexalith.GitStorage.Requests/GitStorageAccount/GetGitStorageAccountDetails.cs
- [X] T023 [US2] Create GetGitStorageAccountSummaries request in src/libraries/Application/Hexalith.GitStorage.Requests/GitStorageAccount/GetGitStorageAccountSummaries.cs
- [X] T024 [US2] Create GetGitStorageAccountIds request in src/libraries/Application/Hexalith.GitStorage.Requests/GitStorageAccount/GetGitStorageAccountIds.cs
- [X] T025 [US2] Create GetGitStorageAccountDetailsHandler in src/libraries/Application/Hexalith.GitStorage/RequestHandlers/GetGitStorageAccountDetailsHandler.cs
- [X] T026 [US2] Create GetGitStorageAccountSummariesHandler in src/libraries/Application/Hexalith.GitStorage/RequestHandlers/GetGitStorageAccountSummariesHandler.cs
- [X] T027 [US2] Create GetGitStorageAccountIdsHandler in src/libraries/Application/Hexalith.GitStorage/RequestHandlers/GetGitStorageAccountIdsHandler.cs
- [X] T028 [US2] Add GET endpoint /api/GitStorageAccount/{id} for details in src/libraries/Infrastructure/Hexalith.GitStorage.ApiServer/Controllers/GitStorageAccountController.cs
- [X] T029 [US2] Add GET endpoint /api/GitStorageAccount for summaries (paginated) in src/libraries/Infrastructure/Hexalith.GitStorage.ApiServer/Controllers/GitStorageAccountController.cs
- [X] T030 [US2] Add GET endpoint /api/GitStorageAccount/ids for ID list in src/libraries/Infrastructure/Hexalith.GitStorage.ApiServer/Controllers/GitStorageAccountController.cs

**Checkpoint**: User Story 2 complete - can view account details, summaries, and IDs via API

---

## Phase 5: User Story 3 - Update Git Storage Account (Priority: P3)

**Goal**: Enable administrators to update Git Storage Account description/comments when configurations change

**Independent Test**: Modify an existing account's Name and Comments, then verify changes are persisted and reflected when viewing

### Tests for User Story 3

- [X] T031 [P] [US3] Create aggregate Apply test for GitStorageAccountDescriptionChanged in test/Hexalith.GitStorage.Tests/Domains/Aggregates/GitStorageAccountTests.cs
- [X] T032 [P] [US3] Create command serialization test for ChangeGitStorageAccountDescription in test/Hexalith.GitStorage.Tests/Domains/Commands/GitStorageAccountCommandTests.cs

### Implementation for User Story 3

- [X] T033 [US3] Create ChangeGitStorageAccountDescription command in src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/ChangeGitStorageAccountDescription.cs
- [X] T034 [US3] Implement ApplyEvent(GitStorageAccountDescriptionChanged) method in GitStorageAccount aggregate in src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitStorageAccount.cs
- [X] T035 [US3] Create GitStorageAccountDescriptionChangedOnDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Details/GitStorageAccountDescriptionChangedOnDetailsProjectionHandler.cs
- [X] T036 [US3] Create ChangeGitStorageAccountDescriptionHandler in src/libraries/Application/Hexalith.GitStorage/CommandHandlers/ChangeGitStorageAccountDescriptionHandler.cs (with validation: Id required, Name required, aggregate must exist and not be disabled)
- [X] T037 [US3] Add POST endpoint for ChangeGitStorageAccountDescription command in src/libraries/Infrastructure/Hexalith.GitStorage.ApiServer/Controllers/GitStorageAccountController.cs

**Checkpoint**: User Story 3 complete - can update account descriptions via API

---

## Phase 6: User Story 4 - Enable/Disable Git Storage Account (Priority: P4)

**Goal**: Enable administrators to enable or disable accounts to temporarily suspend access without deleting configuration

**Independent Test**: Disable an enabled account and verify status changes to disabled; re-enable and verify status changes back to enabled

### Tests for User Story 4

- [X] T038 [P] [US4] Create aggregate Apply test for GitStorageAccountEnabled in test/Hexalith.GitStorage.Tests/Domains/Aggregates/GitStorageAccountTests.cs
- [X] T039 [P] [US4] Create aggregate Apply test for GitStorageAccountDisabled in test/Hexalith.GitStorage.Tests/Domains/Aggregates/GitStorageAccountTests.cs
- [X] T040 [P] [US4] Create command serialization test for EnableGitStorageAccount in test/Hexalith.GitStorage.Tests/Domains/Commands/GitStorageAccountCommandTests.cs
- [X] T041 [P] [US4] Create command serialization test for DisableGitStorageAccount in test/Hexalith.GitStorage.Tests/Domains/Commands/GitStorageAccountCommandTests.cs

### Implementation for User Story 4

- [X] T042 [P] [US4] Create EnableGitStorageAccount command in src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/EnableGitStorageAccount.cs
- [X] T043 [P] [US4] Create DisableGitStorageAccount command in src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/DisableGitStorageAccount.cs
- [X] T044 [US4] Implement ApplyEvent(GitStorageAccountEnabled) method in GitStorageAccount aggregate in src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitStorageAccount.cs
- [X] T045 [US4] Implement ApplyEvent(GitStorageAccountDisabled) method in GitStorageAccount aggregate in src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitStorageAccount.cs
- [X] T046 [P] [US4] Create GitStorageAccountEnabledOnDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Details/GitStorageAccountEnabledOnDetailsProjectionHandler.cs
- [X] T047 [P] [US4] Create GitStorageAccountDisabledOnDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Details/GitStorageAccountDisabledOnDetailsProjectionHandler.cs
- [X] T048 [US4] Create EnableGitStorageAccountHandler in src/libraries/Application/Hexalith.GitStorage/CommandHandlers/EnableGitStorageAccountHandler.cs (with validation: Id required, aggregate must exist)
- [X] T049 [US4] Create DisableGitStorageAccountHandler in src/libraries/Application/Hexalith.GitStorage/CommandHandlers/DisableGitStorageAccountHandler.cs (with validation: Id required, aggregate must exist)
- [X] T050 [US4] Add POST endpoint for EnableGitStorageAccount command in src/libraries/Infrastructure/Hexalith.GitStorage.ApiServer/Controllers/GitStorageAccountController.cs
- [X] T051 [US4] Add POST endpoint for DisableGitStorageAccount command in src/libraries/Infrastructure/Hexalith.GitStorage.ApiServer/Controllers/GitStorageAccountController.cs

**Checkpoint**: User Story 4 complete - can enable/disable accounts via API

---

## Phase 7: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories and final validation

- [X] T052 [P] Add [Authorize(Roles = "Admin")] attribute to GitStorageAccountController in src/libraries/Infrastructure/Hexalith.GitStorage.ApiServer/Controllers/GitStorageAccountController.cs
- [X] T053 [P] Register all command handlers in module configuration in src/libraries/Infrastructure/Hexalith.GitStorage.ApiServer/
- [X] T054 [P] Register all request handlers in module configuration in src/libraries/Infrastructure/Hexalith.GitStorage.ApiServer/
- [X] T055 [P] Register all projection handlers in module configuration in src/libraries/Application/Hexalith.GitStorage.Projections/
- [X] T056 Run all unit tests to verify serialization and aggregate behavior (dotnet test)
- [X] T057 Run quickstart.md API validation scenarios (create, view, update, enable, disable)
- [X] T058 Verify build succeeds (dotnet build)

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Stories (Phase 3-6)**: All depend on Foundational phase completion
  - User stories can proceed in parallel (if staffed)
  - Or sequentially in priority order (P1 → P2 → P3 → P4)
- **Polish (Phase 7)**: Depends on all desired user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational (Phase 2) - No dependencies on other stories
- **User Story 2 (P2)**: Can start after Foundational (Phase 2) - Uses ViewModels created in Phase 2; independently testable
- **User Story 3 (P3)**: Can start after Foundational (Phase 2) - Independently testable (assumes account created)
- **User Story 4 (P4)**: Can start after Foundational (Phase 2) - Independently testable (assumes account created)

### Within Each User Story

- Tests MUST be written and FAIL before implementation
- Events before aggregate methods
- Commands before handlers
- Handlers before API endpoints
- Projections can be parallel with handlers (different files)

### Parallel Opportunities

- All Foundational tasks T004-T007 (events) can run in parallel
- T011, T012 (ViewModels) can run in parallel
- Within US1: T014, T015 tests can run in parallel
- Within US3: T031, T032 tests can run in parallel
- Within US4: T038-T041 tests can run in parallel; T042, T043 commands can run in parallel; T046, T047 projections can run in parallel
- Different user stories can be worked on in parallel by different team members after Foundational phase

---

## Parallel Example: User Story 4

```bash
# Launch all tests for User Story 4 together:
Task: "Create aggregate Apply test for GitStorageAccountEnabled in test/Hexalith.GitStorage.Tests/Domains/Aggregates/GitStorageAccountTests.cs"
Task: "Create aggregate Apply test for GitStorageAccountDisabled in test/Hexalith.GitStorage.Tests/Domains/Aggregates/GitStorageAccountTests.cs"
Task: "Create command serialization test for EnableGitStorageAccount in test/Hexalith.GitStorage.Tests/Domains/Commands/GitStorageAccountCommandTests.cs"
Task: "Create command serialization test for DisableGitStorageAccount in test/Hexalith.GitStorage.Tests/Domains/Commands/GitStorageAccountCommandTests.cs"

# Launch both commands in parallel:
Task: "Create EnableGitStorageAccount command in src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/EnableGitStorageAccount.cs"
Task: "Create DisableGitStorageAccount command in src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/DisableGitStorageAccount.cs"

# Launch both projection handlers in parallel:
Task: "Create GitStorageAccountEnabledOnDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Details/GitStorageAccountEnabledOnDetailsProjectionHandler.cs"
Task: "Create GitStorageAccountDisabledOnDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Details/GitStorageAccountDisabledOnDetailsProjectionHandler.cs"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational (CRITICAL - blocks all stories)
3. Complete Phase 3: User Story 1
4. **STOP and VALIDATE**: Test User Story 1 independently
5. Deploy/demo if ready - can create Git Storage Accounts

### Incremental Delivery

1. Complete Setup + Foundational → Foundation ready
2. Add User Story 1 → Test independently → Deploy/Demo (MVP! - Create accounts)
3. Add User Story 2 → Test independently → Deploy/Demo (View accounts)
4. Add User Story 3 → Test independently → Deploy/Demo (Update accounts)
5. Add User Story 4 → Test independently → Deploy/Demo (Enable/Disable accounts)
6. Each story adds value without breaking previous stories

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup + Foundational together
2. Once Foundational is done:
   - Developer A: User Story 1 (Create)
   - Developer B: User Story 2 (View)
   - Developer C: User Story 3 (Update)
   - Developer D: User Story 4 (Enable/Disable)
3. Stories complete and integrate independently

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- Verify tests fail before implementing
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
- All public members require XML documentation per CLAUDE.md
- All .cs files require copyright header per CLAUDE.md
- Use primary constructors and file-scoped namespaces per CLAUDE.md
- Use [PolymorphicSerialization] on events, commands, requests per CLAUDE.md
- Use [DataContract] and [DataMember(Order = N)] on aggregates and DTOs per CLAUDE.md
