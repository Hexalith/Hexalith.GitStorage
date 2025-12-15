# Tasks: GitRepository Aggregate

**Input**: Design documents from `/specs/004-git-repository/`
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/git-repository-api.yaml

**Tests**: Tests are included as requested in the feature specification (FR-013, Constitution VI).

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization and enums required by all components

- [ ] T001 [P] Create GitRepositoryVisibility enum in src/libraries/Domain/Hexalith.GitStorage.Aggregates.Abstractions/Enums/GitRepositoryVisibility.cs
- [ ] T002 [P] Create GitRepositoryOrigin enum in src/libraries/Domain/Hexalith.GitStorage.Aggregates.Abstractions/Enums/GitRepositoryOrigin.cs
- [ ] T003 [P] Create GitRepositorySyncStatus enum in src/libraries/Domain/Hexalith.GitStorage.Aggregates.Abstractions/Enums/GitRepositorySyncStatus.cs

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core domain events, aggregate, and base classes that MUST be complete before ANY user story can be implemented

**CRITICAL**: No user story work can begin until this phase is complete

### Domain Events (Base)

- [ ] T004 Create GitRepositoryEvent abstract base class in src/libraries/Domain/Hexalith.GitStorage.Events/GitRepository/GitRepositoryEvent.cs

### Aggregate Root

- [ ] T005 Create GitRepository aggregate root in src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitRepository.cs (depends on T004, T001-T003)

### Base Commands & Requests

- [ ] T006 [P] Create GitRepositoryCommand abstract base class in src/libraries/Application/Hexalith.GitStorage.Commands/GitRepository/GitRepositoryCommand.cs
- [ ] T007 [P] Create GitRepositoryRequest abstract base class in src/libraries/Application/Hexalith.GitStorage.Requests/GitRepository/GitRepositoryRequest.cs

### View Models

- [ ] T008 [P] Create GitRepositorySummaryViewModel in src/libraries/Application/Hexalith.GitStorage.Requests/GitRepository/GitRepositorySummaryViewModel.cs
- [ ] T009 [P] Create GitRepositoryDetailsViewModel in src/libraries/Application/Hexalith.GitStorage.Requests/GitRepository/GitRepositoryDetailsViewModel.cs

### Localization Labels

- [ ] T010 Add InvalidRepositoryNameFormat label to Hexalith.GitStorage.Localizations if not already present

**Checkpoint**: Foundation ready - user story implementation can now begin

---

## Phase 3: User Story 1 - Create a Git Repository (Priority: P1) MVP

**Goal**: Enable administrators to create new Git repositories through the application, automatically creating them on the remote Git Server (GitHub or Forgejo) and tracking them locally.

**Independent Test**: Create a repository with required details (name, organization, visibility) and verify it appears both locally and via GetGitRepositoryDetails query.

### Tests for User Story 1

- [ ] T011 [P] [US1] Create AddGitRepositoryValidatorTests in test/Hexalith.GitStorage.Tests/GitRepository/AddGitRepositoryValidatorTests.cs
- [ ] T012 [P] [US1] Create GitRepositoryAddedEventTests in test/Hexalith.GitStorage.Tests/GitRepository/GitRepositoryEventTests.cs

### Implementation for User Story 1

#### Events

- [ ] T013 [P] [US1] Create GitRepositoryAdded event in src/libraries/Domain/Hexalith.GitStorage.Events/GitRepository/GitRepositoryAdded.cs

#### Commands

- [ ] T014 [P] [US1] Create AddGitRepository command in src/libraries/Application/Hexalith.GitStorage.Commands/GitRepository/AddGitRepository.cs

#### Validators

- [ ] T015 [US1] Create AddGitRepositoryValidator in src/libraries/Application/Hexalith.GitStorage.Commands/GitRepository/Validators/AddGitRepositoryValidator.cs (depends on T014)

#### Aggregate Apply Method

- [ ] T016 [US1] Implement Apply method for GitRepositoryAdded in src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitRepository.cs (depends on T013)

#### Requests

- [ ] T017 [P] [US1] Create GetGitRepositorySummaries request in src/libraries/Application/Hexalith.GitStorage.Requests/GitRepository/GetGitRepositorySummaries.cs
- [ ] T018 [P] [US1] Create GetGitRepositoryDetails request in src/libraries/Application/Hexalith.GitStorage.Requests/GitRepository/GetGitRepositoryDetails.cs

#### Projections

- [ ] T019 [P] [US1] Create GitRepositoryAddedOnSummaryProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/GitRepository/ProjectionHandlers/GitRepositoryAddedOnSummaryProjectionHandler.cs
- [ ] T020 [P] [US1] Create GitRepositoryAddedOnDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/GitRepository/ProjectionHandlers/GitRepositoryAddedOnDetailsProjectionHandler.cs

#### API Controller

- [ ] T021 [US1] Create GitRepositoryIntegrationEventsController in src/libraries/Infrastructure/Hexalith.GitStorage.ApiServer/Controllers/GitRepositoryIntegrationEventsController.cs (depends on T013)

#### UI Components

- [ ] T022 [P] [US1] Create GitRepositoryEditViewModel in src/libraries/Presentation/Hexalith.GitStorage.UI.Components/GitRepository/GitRepositoryEditViewModel.cs
- [ ] T023 [P] [US1] Create GitRepositoryEditValidation in src/libraries/Presentation/Hexalith.GitStorage.UI.Components/GitRepository/GitRepositoryEditValidation.cs

#### UI Pages

- [ ] T024 [US1] Create GitRepositoryIndex.razor in src/libraries/Presentation/Hexalith.GitStorage.UI.Pages/GitRepository/GitRepositoryIndex.razor (depends on T017, T008)
- [ ] T025 [US1] Create GitRepositoryDetails.razor in src/libraries/Presentation/Hexalith.GitStorage.UI.Pages/GitRepository/GitRepositoryDetails.razor (depends on T018, T009, T022, T023)
- [ ] T025a [US1] Create GitRepositoryEdit.razor in src/libraries/Presentation/Hexalith.GitStorage.UI.Pages/GitRepository/GitRepositoryEdit.razor (depends on T022, T023, T014, T029, T037, T047, T048, T072)

**Checkpoint**: User Story 1 complete - administrators can create repositories and view them in list/detail views

---

## Phase 4: User Story 2 - Update Repository Description (Priority: P2)

**Goal**: Enable repository managers to update repository descriptions to provide accurate context about the repository's purpose.

**Independent Test**: Change a repository's description and verify the update persists and is retrievable via GetGitRepositoryDetails.

### Tests for User Story 2

- [ ] T026 [P] [US2] Create ChangeGitRepositoryDescriptionValidatorTests in test/Hexalith.GitStorage.Tests/GitRepository/ChangeGitRepositoryDescriptionValidatorTests.cs
- [ ] T027 [P] [US2] Add GitRepositoryDescriptionChanged event tests in test/Hexalith.GitStorage.Tests/GitRepository/GitRepositoryEventTests.cs

### Implementation for User Story 2

#### Events

- [ ] T028 [P] [US2] Create GitRepositoryDescriptionChanged event in src/libraries/Domain/Hexalith.GitStorage.Events/GitRepository/GitRepositoryDescriptionChanged.cs

#### Commands

- [ ] T029 [P] [US2] Create ChangeGitRepositoryDescription command in src/libraries/Application/Hexalith.GitStorage.Commands/GitRepository/ChangeGitRepositoryDescription.cs

#### Validators

- [ ] T030 [US2] Create ChangeGitRepositoryDescriptionValidator in src/libraries/Application/Hexalith.GitStorage.Commands/GitRepository/Validators/ChangeGitRepositoryDescriptionValidator.cs (depends on T029)

#### Aggregate Apply Method

- [ ] T031 [US2] Implement Apply method for GitRepositoryDescriptionChanged in src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitRepository.cs (depends on T028)

#### Projections

- [ ] T032 [P] [US2] Create GitRepositoryDescriptionChangedOnSummaryProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/GitRepository/ProjectionHandlers/GitRepositoryDescriptionChangedOnSummaryProjectionHandler.cs
- [ ] T033 [P] [US2] Create GitRepositoryDescriptionChangedOnDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/GitRepository/ProjectionHandlers/GitRepositoryDescriptionChangedOnDetailsProjectionHandler.cs

**Checkpoint**: User Story 2 complete - repository descriptions can be updated

---

## Phase 5: User Story 3 - Change Repository Visibility (Priority: P2)

**Goal**: Enable repository owners to change visibility (Public, Private, Internal) to control repository access.

**Independent Test**: Change a repository's visibility setting and verify the change is reflected in GetGitRepositoryDetails.

### Tests for User Story 3

- [ ] T034 [P] [US3] Create ChangeGitRepositoryVisibilityValidatorTests in test/Hexalith.GitStorage.Tests/GitRepository/ChangeGitRepositoryVisibilityValidatorTests.cs
- [ ] T035 [P] [US3] Add GitRepositoryVisibilityChanged event tests in test/Hexalith.GitStorage.Tests/GitRepository/GitRepositoryEventTests.cs

### Implementation for User Story 3

#### Events

- [ ] T036 [P] [US3] Create GitRepositoryVisibilityChanged event in src/libraries/Domain/Hexalith.GitStorage.Events/GitRepository/GitRepositoryVisibilityChanged.cs

#### Commands

- [ ] T037 [P] [US3] Create ChangeGitRepositoryVisibility command in src/libraries/Application/Hexalith.GitStorage.Commands/GitRepository/ChangeGitRepositoryVisibility.cs

#### Validators

- [ ] T038 [US3] Create ChangeGitRepositoryVisibilityValidator in src/libraries/Application/Hexalith.GitStorage.Commands/GitRepository/Validators/ChangeGitRepositoryVisibilityValidator.cs (depends on T037)

#### Aggregate Apply Method

- [ ] T039 [US3] Implement Apply method for GitRepositoryVisibilityChanged in src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitRepository.cs (depends on T036)

#### Projections

- [ ] T040 [P] [US3] Create GitRepositoryVisibilityChangedOnSummaryProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/GitRepository/ProjectionHandlers/GitRepositoryVisibilityChangedOnSummaryProjectionHandler.cs
- [ ] T041 [P] [US3] Create GitRepositoryVisibilityChangedOnDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/GitRepository/ProjectionHandlers/GitRepositoryVisibilityChangedOnDetailsProjectionHandler.cs

**Checkpoint**: User Story 3 complete - repository visibility can be changed

---

## Phase 6: User Story 4 - Enable or Disable a Repository (Priority: P2)

**Goal**: Enable administrators to enable or disable repositories to temporarily restrict access without deletion.

**Independent Test**: Disable a repository and verify it is marked as disabled, then re-enable it and verify the state change.

### Tests for User Story 4

- [ ] T042 [P] [US4] Create DisableGitRepositoryValidatorTests in test/Hexalith.GitStorage.Tests/GitRepository/DisableGitRepositoryValidatorTests.cs
- [ ] T043 [P] [US4] Create EnableGitRepositoryValidatorTests in test/Hexalith.GitStorage.Tests/GitRepository/EnableGitRepositoryValidatorTests.cs
- [ ] T044 [P] [US4] Add GitRepositoryDisabled and GitRepositoryEnabled event tests in test/Hexalith.GitStorage.Tests/GitRepository/GitRepositoryEventTests.cs

### Implementation for User Story 4

#### Events

- [ ] T045 [P] [US4] Create GitRepositoryDisabled event in src/libraries/Domain/Hexalith.GitStorage.Events/GitRepository/GitRepositoryDisabled.cs
- [ ] T046 [P] [US4] Create GitRepositoryEnabled event in src/libraries/Domain/Hexalith.GitStorage.Events/GitRepository/GitRepositoryEnabled.cs

#### Commands

- [ ] T047 [P] [US4] Create DisableGitRepository command in src/libraries/Application/Hexalith.GitStorage.Commands/GitRepository/DisableGitRepository.cs
- [ ] T048 [P] [US4] Create EnableGitRepository command in src/libraries/Application/Hexalith.GitStorage.Commands/GitRepository/EnableGitRepository.cs

#### Validators

- [ ] T049 [US4] Create DisableGitRepositoryValidator in src/libraries/Application/Hexalith.GitStorage.Commands/GitRepository/Validators/DisableGitRepositoryValidator.cs (depends on T047)
- [ ] T050 [US4] Create EnableGitRepositoryValidator in src/libraries/Application/Hexalith.GitStorage.Commands/GitRepository/Validators/EnableGitRepositoryValidator.cs (depends on T048)

#### Aggregate Apply Methods

- [ ] T051 [US4] Implement Apply method for GitRepositoryDisabled in src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitRepository.cs (depends on T045)
- [ ] T052 [US4] Implement Apply method for GitRepositoryEnabled in src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitRepository.cs (depends on T046)

#### Projections

- [ ] T053 [P] [US4] Create GitRepositoryDisabledOnSummaryProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/GitRepository/ProjectionHandlers/GitRepositoryDisabledOnSummaryProjectionHandler.cs
- [ ] T054 [P] [US4] Create GitRepositoryDisabledOnDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/GitRepository/ProjectionHandlers/GitRepositoryDisabledOnDetailsProjectionHandler.cs
- [ ] T055 [P] [US4] Create GitRepositoryEnabledOnSummaryProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/GitRepository/ProjectionHandlers/GitRepositoryEnabledOnSummaryProjectionHandler.cs
- [ ] T056 [P] [US4] Create GitRepositoryEnabledOnDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/GitRepository/ProjectionHandlers/GitRepositoryEnabledOnDetailsProjectionHandler.cs

**Checkpoint**: User Story 4 complete - repositories can be enabled/disabled

---

## Phase 7: User Story 5 - Synchronize Repository Metadata (Priority: P3)

**Goal**: Enable system operators to trigger synchronization of repository metadata from the source Git provider for data consistency.

**Independent Test**: Trigger a sync operation and verify metadata (e.g., last sync timestamp) is updated.

### Tests for User Story 5

- [ ] T057 [P] [US5] Create SyncGitRepositoryValidatorTests in test/Hexalith.GitStorage.Tests/GitRepository/SyncGitRepositoryValidatorTests.cs
- [ ] T058 [P] [US5] Add GitRepositorySynced and GitRepositoryMarkedNotFound event tests in test/Hexalith.GitStorage.Tests/GitRepository/GitRepositoryEventTests.cs

### Implementation for User Story 5

#### Events

- [ ] T059 [P] [US5] Create GitRepositorySynced event in src/libraries/Domain/Hexalith.GitStorage.Events/GitRepository/GitRepositorySynced.cs
- [ ] T060 [P] [US5] Create GitRepositoryMarkedNotFound event in src/libraries/Domain/Hexalith.GitStorage.Events/GitRepository/GitRepositoryMarkedNotFound.cs

#### Commands

- [ ] T061 [P] [US5] Create SyncGitRepository command in src/libraries/Application/Hexalith.GitStorage.Commands/GitRepository/SyncGitRepository.cs

#### Validators

- [ ] T062 [US5] Create SyncGitRepositoryValidator in src/libraries/Application/Hexalith.GitStorage.Commands/GitRepository/Validators/SyncGitRepositoryValidator.cs (depends on T061)

#### Aggregate Apply Methods

- [ ] T063 [US5] Implement Apply method for GitRepositorySynced in src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitRepository.cs (depends on T059)
- [ ] T064 [US5] Implement Apply method for GitRepositoryMarkedNotFound in src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitRepository.cs (depends on T060)

#### Projections

- [ ] T065 [P] [US5] Create GitRepositorySyncedOnSummaryProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/GitRepository/ProjectionHandlers/GitRepositorySyncedOnSummaryProjectionHandler.cs
- [ ] T066 [P] [US5] Create GitRepositorySyncedOnDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/GitRepository/ProjectionHandlers/GitRepositorySyncedOnDetailsProjectionHandler.cs
- [ ] T067 [P] [US5] Create GitRepositoryMarkedNotFoundOnSummaryProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/GitRepository/ProjectionHandlers/GitRepositoryMarkedNotFoundOnSummaryProjectionHandler.cs
- [ ] T068 [P] [US5] Create GitRepositoryMarkedNotFoundOnDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/GitRepository/ProjectionHandlers/GitRepositoryMarkedNotFoundOnDetailsProjectionHandler.cs

#### Organization-Level Sync Handler

- [ ] T068a [US5] Create SyncGitOrganizationRepositoriesHandler to discover and sync all repositories from a GitOrganization (handles FR-014) in src/libraries/Application/Hexalith.GitStorage.CommandHandlers/GitRepository/SyncGitOrganizationRepositoriesHandler.cs (depends on T059, T060)

**Checkpoint**: User Story 5 complete - repository sync operations functional

---

## Phase 8: User Story 6 - Change Default Branch (Priority: P3)

**Goal**: Enable repository managers to change the default branch so the system correctly references the primary branch for operations.

**Independent Test**: Change the default branch name and verify the update persists via GetGitRepositoryDetails.

### Tests for User Story 6

- [ ] T069 [P] [US6] Create ChangeGitRepositoryDefaultBranchValidatorTests in test/Hexalith.GitStorage.Tests/GitRepository/ChangeGitRepositoryDefaultBranchValidatorTests.cs
- [ ] T070 [P] [US6] Add GitRepositoryDefaultBranchChanged event tests in test/Hexalith.GitStorage.Tests/GitRepository/GitRepositoryEventTests.cs

### Implementation for User Story 6

#### Events

- [ ] T071 [P] [US6] Create GitRepositoryDefaultBranchChanged event in src/libraries/Domain/Hexalith.GitStorage.Events/GitRepository/GitRepositoryDefaultBranchChanged.cs

#### Commands

- [ ] T072 [P] [US6] Create ChangeGitRepositoryDefaultBranch command in src/libraries/Application/Hexalith.GitStorage.Commands/GitRepository/ChangeGitRepositoryDefaultBranch.cs

#### Validators

- [ ] T073 [US6] Create ChangeGitRepositoryDefaultBranchValidator in src/libraries/Application/Hexalith.GitStorage.Commands/GitRepository/Validators/ChangeGitRepositoryDefaultBranchValidator.cs (depends on T072)

#### Aggregate Apply Method

- [ ] T074 [US6] Implement Apply method for GitRepositoryDefaultBranchChanged in src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitRepository.cs (depends on T071)

#### Projections

- [ ] T075 [P] [US6] Create GitRepositoryDefaultBranchChangedOnSummaryProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/GitRepository/ProjectionHandlers/GitRepositoryDefaultBranchChangedOnSummaryProjectionHandler.cs
- [ ] T076 [P] [US6] Create GitRepositoryDefaultBranchChangedOnDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/GitRepository/ProjectionHandlers/GitRepositoryDefaultBranchChangedOnDetailsProjectionHandler.cs

**Checkpoint**: User Story 6 complete - default branch can be changed

---

## Phase 9: Polish & Cross-Cutting Concerns

**Purpose**: Integration, comprehensive tests, and final validation

- [ ] T077 Create GitRepositoryTests (aggregate behavior tests) in test/Hexalith.GitStorage.Tests/GitRepository/GitRepositoryTests.cs
- [ ] T078 Register all GitRepository events for polymorphic serialization in appropriate configuration
- [ ] T079 Register all GitRepository commands for polymorphic serialization in appropriate configuration
- [ ] T080 Register all GitRepository projection handlers in dependency injection configuration
- [ ] T081 Verify build succeeds with dotnet build
- [ ] T082 Run all tests and verify they pass with dotnet test
- [ ] T083 Run quickstart.md validation scenarios manually

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Stories (Phase 3-8)**: All depend on Foundational phase completion
  - User stories can proceed in parallel (if staffed)
  - Or sequentially in priority order (P1 → P2 → P3)
- **Polish (Phase 9)**: Depends on all user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational (Phase 2) - No dependencies on other stories
- **User Story 2 (P2)**: Can start after Foundational (Phase 2) - Independent of US1
- **User Story 3 (P2)**: Can start after Foundational (Phase 2) - Independent of US1, US2
- **User Story 4 (P2)**: Can start after Foundational (Phase 2) - Independent of US1-US3
- **User Story 5 (P3)**: Can start after Foundational (Phase 2) - Independent of other stories
- **User Story 6 (P3)**: Can start after Foundational (Phase 2) - Independent of other stories

### Within Each User Story

- Tests can be written first (TDD approach)
- Events before commands (commands emit events)
- Commands before validators
- Aggregate Apply methods after events
- Projections after events
- UI components after requests/view models

### Parallel Opportunities

- All Setup tasks (T001-T003) can run in parallel
- T006-T009 (base classes and view models) can run in parallel after T004
- Within each user story, tasks marked [P] can run in parallel
- Different user stories can be worked on in parallel by different team members

---

## Parallel Example: User Story 1

```bash
# Launch all tests for User Story 1 together:
Task: "T011 [P] [US1] Create AddGitRepositoryValidatorTests"
Task: "T012 [P] [US1] Create GitRepositoryAddedEventTests"

# Launch events and commands together:
Task: "T013 [P] [US1] Create GitRepositoryAdded event"
Task: "T014 [P] [US1] Create AddGitRepository command"
Task: "T017 [P] [US1] Create GetGitRepositorySummaries request"
Task: "T018 [P] [US1] Create GetGitRepositoryDetails request"

# Launch projections together:
Task: "T019 [P] [US1] Create GitRepositoryAddedOnSummaryProjectionHandler"
Task: "T020 [P] [US1] Create GitRepositoryAddedOnDetailsProjectionHandler"

# Launch UI components together:
Task: "T022 [P] [US1] Create GitRepositoryEditViewModel"
Task: "T023 [P] [US1] Create GitRepositoryEditValidation"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup (T001-T003)
2. Complete Phase 2: Foundational (T004-T010)
3. Complete Phase 3: User Story 1 (T011-T025)
4. **STOP and VALIDATE**: Test User Story 1 independently
5. Deploy/demo if ready - users can create and view repositories

### Incremental Delivery

1. Complete Setup + Foundational → Foundation ready
2. Add User Story 1 → Test independently → Deploy/Demo (MVP!)
3. Add User Stories 2-4 (P2) → Test independently → Deploy/Demo
4. Add User Stories 5-6 (P3) → Test independently → Deploy/Demo
5. Each story adds value without breaking previous stories

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup + Foundational together
2. Once Foundational is done:
   - Developer A: User Story 1 (Create Repository)
   - Developer B: User Stories 2-3 (Description + Visibility)
   - Developer C: User Story 4 (Enable/Disable)
3. After P1/P2 complete:
   - Developer A: User Story 5 (Sync)
   - Developer B: User Story 6 (Default Branch)
4. Stories complete and integrate independently

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- Verify tests fail before implementing (TDD)
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
- Follow existing GitOrganization patterns for consistency
- All events require `[PolymorphicSerialization]` and `[DataMember(Order = N)]` attributes
