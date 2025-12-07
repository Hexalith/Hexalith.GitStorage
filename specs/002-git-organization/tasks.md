# Tasks: GitOrganization Entity with Visibility Property

**Input**: Design documents from `/specs/002-git-organization/`
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/

**Tests**: Tests are included as this is a DDD/CQRS/Event Sourcing implementation requiring validation of aggregate behavior and event serialization.

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

**Key Enhancement**: Add `Visibility` property (Public | Private | Internal) to the existing GitOrganization implementation.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Path Conventions

Following Hexalith module architecture:
- **Domain**: `src/libraries/Domain/`
- **Application**: `src/libraries/Application/`
- **Presentation**: `src/libraries/Presentation/`
- **Tests**: `test/Hexalith.GitStorage.Tests/`

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Domain enums, helpers, and base classes that all user stories depend on

- [X] T001 [P] Create GitOrganizationOrigin enum in `src/libraries/Domain/Hexalith.GitStorage.Aggregates.Abstractions/Enums/GitOrganizationOrigin.cs`
- [X] T002 [P] Create GitOrganizationSyncStatus enum in `src/libraries/Domain/Hexalith.GitStorage.Aggregates.Abstractions/Enums/GitOrganizationSyncStatus.cs`
- [ ] T002a [P] Create GitOrganizationVisibility enum (Public/Private/Internal) in `src/libraries/Domain/Hexalith.GitStorage.Aggregates.Abstractions/Enums/GitOrganizationVisibility.cs`
- [X] T003 [P] Create GitOrganizationDomainHelper in `src/libraries/Domain/Hexalith.GitStorage.Aggregates.Abstractions/GitOrganizationDomainHelper.cs`
- [X] T004 [P] Create base GitOrganizationEvent in `src/libraries/Domain/Hexalith.GitStorage.Events/GitOrganization/GitOrganizationEvent.cs`
- [X] T005 [P] Create base GitOrganizationCommand in `src/libraries/Application/Hexalith.GitStorage.Commands/GitOrganization/GitOrganizationCommand.cs`
- [X] T006 [P] Create base GitOrganizationRequest in `src/libraries/Application/Hexalith.GitStorage.Requests/GitOrganization/GitOrganizationRequest.cs`
- [ ] T006a [P] Add Visibility localization keys to `src/libraries/Domain/Hexalith.GitStorage.Localizations/GitOrganization.resx`
- [ ] T006b [P] Add French Visibility labels to `src/libraries/Domain/Hexalith.GitStorage.Localizations/GitOrganization.fr.resx`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core aggregate and shared events that MUST be complete before ANY user story can be implemented

**CRITICAL**: No user story work can begin until this phase is complete

- [ ] T007 Add Visibility parameter to GitOrganizationAdded event in `src/libraries/Domain/Hexalith.GitStorage.Events/GitOrganization/GitOrganizationAdded.cs`
- [ ] T008 [P] Add Visibility parameter to GitOrganizationSynced event in `src/libraries/Domain/Hexalith.GitStorage.Events/GitOrganization/GitOrganizationSynced.cs`
- [X] T009 [P] Create GitOrganizationDescriptionChanged event in `src/libraries/Domain/Hexalith.GitStorage.Events/GitOrganization/GitOrganizationDescriptionChanged.cs`
- [ ] T009a [P] Create GitOrganizationVisibilityChanged event in `src/libraries/Domain/Hexalith.GitStorage.Events/GitOrganization/GitOrganizationVisibilityChanged.cs`
- [ ] T009b [P] Create GitOrganizationVisibilityChangedValidator in `src/libraries/Application/Hexalith.GitStorage.Commands/GitOrganization/GitOrganizationVisibilityChangedValidator.cs`
- [X] T010 [P] Create GitOrganizationMarkedNotFound event in `src/libraries/Domain/Hexalith.GitStorage.Events/GitOrganization/GitOrganizationMarkedNotFound.cs`
- [X] T011 [P] Create GitOrganizationDisabled event in `src/libraries/Domain/Hexalith.GitStorage.Events/GitOrganization/GitOrganizationDisabled.cs`
- [X] T012 [P] Create GitOrganizationEnabled event in `src/libraries/Domain/Hexalith.GitStorage.Events/GitOrganization/GitOrganizationEnabled.cs`
- [ ] T013 Add Visibility property to GitOrganization aggregate in `src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitOrganization.cs`
- [ ] T013a Add ApplyEvent handler for GitOrganizationVisibilityChanged in `src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitOrganization.cs`
- [ ] T013b Update GitOrganization constructors to handle Visibility from events in `src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitOrganization.cs`
- [ ] T014 [P] Add Visibility property to GitOrganizationDetailsViewModel in `src/libraries/Application/Hexalith.GitStorage.Requests/GitOrganization/GitOrganizationDetailsViewModel.cs`
- [ ] T015 [P] Add Visibility property to GitOrganizationSummaryViewModel in `src/libraries/Application/Hexalith.GitStorage.Requests/GitOrganization/GitOrganizationSummaryViewModel.cs`
- [X] T016 Register GitOrganization aggregate in module helper (using Hexalith framework auto-discovery)
- [X] T017 Register GitOrganization events in module helper (using Hexalith framework auto-discovery)

### Tests for Foundational Phase

- [ ] T018 [P] Update aggregate tests for Visibility property in `test/Hexalith.GitStorage.Tests/Domains/Aggregates/GitOrganizationTests.cs`
- [ ] T019 [P] Add Visibility serialization tests in `test/Hexalith.GitStorage.Tests/Domains/Events/GitOrganizationEventTests.cs`
- [ ] T019a [P] Add GitOrganizationVisibilityChanged event tests in `test/Hexalith.GitStorage.Tests/Domains/Events/GitOrganizationEventTests.cs`

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel

---

## Phase 3: User Story 1 - Synchronize Organizations from Git Storage Account (Priority: P1) MVP

**Goal**: Allow administrators to synchronize the list of organizations from a connected Git Storage Account so they can see and manage all organizations available on the remote git server.

**Independent Test**: Trigger a sync operation for a configured Git Storage Account and verify that remote organizations appear in the local organization list.

### Implementation for User Story 1

- [X] T020 [US1] Create SyncGitOrganizations command in `src/libraries/Application/Hexalith.GitStorage.Commands/GitOrganization/SyncGitOrganizations.cs`
- [X] T021 [US1] Create SyncGitOrganizationsValidator in `src/libraries/Application/Hexalith.GitStorage.Commands/GitOrganization/SyncGitOrganizationsValidator.cs`
- [X] T022 [US1] Add ListOrganizationsAsync to IGitProviderAdapter in `src/libraries/Application/Hexalith.GitStorage.Abstractions/IGitProviderAdapter.cs`
- [ ] T023 [US1] Implement GitHub adapter ListOrganizationsAsync in `src/libraries/Infrastructure/Hexalith.GitStorage.GitHub/GitHubProviderAdapter.cs`
- [ ] T024 [US1] Implement Forgejo adapter ListOrganizationsAsync in `src/libraries/Infrastructure/Hexalith.GitStorage.Forgejo/ForgejoProviderAdapter.cs`
- [ ] T025 [US1] Create SyncGitOrganizationsHandler to orchestrate sync in `src/libraries/Application/Hexalith.GitStorage.Handlers/GitOrganization/SyncGitOrganizationsHandler.cs`
- [ ] T026 [US1] Register sync command handler in DI `src/libraries/Infrastructure/Hexalith.GitStorage.DaprRuntime.Abstractions/Helpers/GitStorageCommandsHelper.cs`

### Tests for User Story 1

- [ ] T027 [P] [US1] Create SyncGitOrganizations command validation tests in `test/Hexalith.GitStorage.Tests/GitOrganization/SyncGitOrganizationsValidatorTests.cs`
- [ ] T028 [P] [US1] Create SyncGitOrganizationsHandler tests in `test/Hexalith.GitStorage.Tests/GitOrganization/SyncGitOrganizationsHandlerTests.cs`

**Checkpoint**: At this point, User Story 1 should be fully functional - sync organizations from remote Git server

---

## Phase 4: User Story 2 - Create Organization via Application API (Priority: P2)

**Goal**: Allow administrators to create a new organization through this application so that the organization is automatically created on the remote Git Server and tracked locally.

**Independent Test**: Create a new organization via the API and verify both local persistence and creation on the remote git server.

### Implementation for User Story 2

- [ ] T029 [US2] Add Visibility parameter to AddGitOrganization command in `src/libraries/Application/Hexalith.GitStorage.Commands/GitOrganization/AddGitOrganization.cs`
- [ ] T030 [US2] Update AddGitOrganizationValidator to validate Visibility in `src/libraries/Application/Hexalith.GitStorage.Commands/GitOrganization/AddGitOrganizationValidator.cs`
- [X] T030a [US2] Create GitOrganizationNameValidator for FR-003 naming rules in `src/libraries/Domain/Hexalith.GitStorage.Aggregates/Validators/GitOrganizationValidator.cs`
- [X] T031 [US2] Add CreateOrganizationAsync to IGitProviderAdapter in `src/libraries/Application/Hexalith.GitStorage.Abstractions/IGitProviderAdapter.cs`
- [ ] T032 [US2] Implement GitHub adapter CreateOrganizationAsync in `src/libraries/Infrastructure/Hexalith.GitStorage.GitHub/GitHubProviderAdapter.cs`
- [ ] T033 [US2] Implement Forgejo adapter CreateOrganizationAsync in `src/libraries/Infrastructure/Hexalith.GitStorage.Forgejo/ForgejoProviderAdapter.cs`
- [ ] T034 [US2] Create GitOrganizationAddedHandler to trigger remote creation in `src/libraries/Application/Hexalith.GitStorage.Handlers/GitOrganization/GitOrganizationAddedHandler.cs`
- [ ] T035 [US2] Register AddGitOrganization command handler in DI `src/libraries/Infrastructure/Hexalith.GitStorage.DaprRuntime.Abstractions/Helpers/GitStorageCommandsHelper.cs`
- [X] T036 [US2] Add POST /git-organizations endpoint in `src/libraries/Infrastructure/Hexalith.GitStorage.WebServer/Controllers/GitOrganizationsController.cs`

### Tests for User Story 2

- [ ] T037 [P] [US2] Create AddGitOrganization aggregate apply tests in `test/Hexalith.GitStorage.Tests/GitOrganization/GitOrganizationAggregateTests.cs`
- [ ] T038 [P] [US2] Create AddGitOrganizationValidator tests in `test/Hexalith.GitStorage.Tests/GitOrganization/AddGitOrganizationValidatorTests.cs`
- [ ] T038a [P] [US2] Create GitOrganizationNameValidator tests (FR-003: alphanumeric/hyphens, max 39 chars, no leading hyphen, no consecutive hyphens) in `test/Hexalith.GitStorage.Tests/GitOrganization/GitOrganizationNameValidatorTests.cs`

**Checkpoint**: At this point, User Story 2 should be fully functional - create organizations via API with remote provisioning

---

## Phase 5: User Story 3 - View Organization Details (Priority: P3)

**Goal**: Allow administrators to view the details of a Git Organization so they can see its configuration, sync status, and relationship to the Git Storage Account.

**Independent Test**: Retrieve an existing organization and verify all stored details are displayed correctly.

### Implementation for User Story 3

- [X] T039 [US3] Create GetGitOrganizationDetails request in `src/libraries/Application/Hexalith.GitStorage.Requests/GitOrganization/GetGitOrganizationDetails.cs`
- [X] T040 [US3] Create GitOrganizationDetailsProjectionHandlers in `src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Details/`
- [X] T041 [US3] Create GetGitOrganizationDetailsHandler in `src/libraries/Application/Hexalith.GitStorage.Projections/RequestHandlers/GetGitOrganizationDetailsHandler.cs`
- [X] T042 [US3] Register GetGitOrganizationDetails request handler in `src/libraries/Application/Hexalith.GitStorage.Projections/Helpers/GitOrganizationProjectionHelper.cs`
- [X] T043 [US3] Add GET /git-organizations/{id} endpoint in `src/libraries/Infrastructure/Hexalith.GitStorage.WebServer/Controllers/GitOrganizationsController.cs`
- [ ] T044 [US3] Create GitOrganizationDetailsPage.razor with Visibility display in `src/libraries/Presentation/Hexalith.GitStorage.UI.Pages/GitOrganizations/GitOrganizationDetailsPage.razor`
- [ ] T045 [US3] Create GitOrganizationCard.razor component with Visibility badge in `src/libraries/Presentation/Hexalith.GitStorage.UI.Components/GitOrganizations/GitOrganizationCard.razor`

### Tests for User Story 3

- [ ] T046 [P] [US3] Create GetGitOrganizationDetails request tests in `test/Hexalith.GitStorage.Tests/GitOrganization/GetGitOrganizationDetailsTests.cs`
- [ ] T047 [P] [US3] Create projection handler tests in `test/Hexalith.GitStorage.Tests/GitOrganization/GitOrganizationDetailsProjectionTests.cs`

**Checkpoint**: At this point, User Story 3 should be fully functional - view organization details

---

## Phase 6: User Story 4 - Update Organization via Application (Priority: P4)

**Goal**: Allow administrators to update organization settings through this application so that changes are reflected both locally and on the remote Git Server.

**Independent Test**: Modify an existing organization's description and verify changes are persisted locally and updated on the remote server.

### Implementation for User Story 4

- [X] T048 [US4] Create ChangeGitOrganizationDescription command in `src/libraries/Application/Hexalith.GitStorage.Commands/GitOrganization/ChangeGitOrganizationDescription.cs`
- [ ] T048a [P] [US4] Create ChangeGitOrganizationVisibility command in `src/libraries/Application/Hexalith.GitStorage.Commands/GitOrganization/ChangeGitOrganizationVisibility.cs`
- [ ] T048b [P] [US4] Create ChangeGitOrganizationVisibilityValidator in `src/libraries/Application/Hexalith.GitStorage.Commands/GitOrganization/ChangeGitOrganizationVisibilityValidator.cs`
- [X] T049 [P] [US4] Create DisableGitOrganization command in `src/libraries/Application/Hexalith.GitStorage.Commands/GitOrganization/DisableGitOrganization.cs`
- [X] T050 [P] [US4] Create EnableGitOrganization command in `src/libraries/Application/Hexalith.GitStorage.Commands/GitOrganization/EnableGitOrganization.cs`
- [X] T051 [US4] Create ChangeGitOrganizationDescriptionValidator in `src/libraries/Application/Hexalith.GitStorage.Commands/GitOrganization/ChangeGitOrganizationDescriptionValidator.cs`
- [X] T052 [US4] Add UpdateOrganizationAsync to IGitProviderAdapter in `src/libraries/Application/Hexalith.GitStorage.Abstractions/IGitProviderAdapter.cs`
- [ ] T053 [US4] Implement GitHub adapter UpdateOrganizationAsync in `src/libraries/Infrastructure/Hexalith.GitStorage.GitHub/GitHubProviderAdapter.cs`
- [ ] T054 [US4] Implement Forgejo adapter UpdateOrganizationAsync in `src/libraries/Infrastructure/Hexalith.GitStorage.Forgejo/ForgejoProviderAdapter.cs`
- [ ] T055 [US4] Create GitOrganizationDescriptionChangedHandler for remote sync in `src/libraries/Application/Hexalith.GitStorage.Handlers/GitOrganization/GitOrganizationDescriptionChangedHandler.cs`
- [ ] T055a [P] [US4] Create GitOrganizationVisibilityChangedHandler for remote sync in `src/libraries/Application/Hexalith.GitStorage.Handlers/GitOrganization/GitOrganizationVisibilityChangedHandler.cs`
- [ ] T055b [P] [US4] Create GitOrganizationVisibilityChangedOnDetailsProjectionHandler in `src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Details/`
- [ ] T055c [P] [US4] Create GitOrganizationVisibilityChangedOnSummaryProjectionHandler in `src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Summaries/`
- [ ] T056 [US4] Register update/disable/enable/visibility command handlers in DI `src/libraries/Infrastructure/Hexalith.GitStorage.DaprRuntime.Abstractions/Helpers/GitStorageCommandsHelper.cs`
- [X] T057 [US4] Add PATCH /git-organizations/{id}/description endpoint in `src/libraries/Infrastructure/Hexalith.GitStorage.WebServer/Controllers/GitOrganizationsController.cs`
- [ ] T057a [US4] Add PATCH /git-organizations/{id}/visibility endpoint in `src/libraries/Infrastructure/Hexalith.GitStorage.WebServer/Controllers/GitOrganizationsController.cs`
- [X] T058 [US4] Add POST /git-organizations/{id}/disable endpoint in `src/libraries/Infrastructure/Hexalith.GitStorage.WebServer/Controllers/GitOrganizationsController.cs`
- [X] T059 [US4] Add POST /git-organizations/{id}/enable endpoint in `src/libraries/Infrastructure/Hexalith.GitStorage.WebServer/Controllers/GitOrganizationsController.cs`
- [ ] T060 [US4] Create GitOrganizationForm.razor component in `src/libraries/Presentation/Hexalith.GitStorage.UI.Components/GitOrganizations/GitOrganizationForm.razor`

### Tests for User Story 4

- [ ] T061 [P] [US4] Create ChangeGitOrganizationDescription aggregate apply tests in `test/Hexalith.GitStorage.Tests/GitOrganization/GitOrganizationAggregateTests.cs`
- [ ] T062 [P] [US4] Create Disable/Enable aggregate apply tests in `test/Hexalith.GitStorage.Tests/GitOrganization/GitOrganizationAggregateTests.cs`
- [ ] T062a [P] [US4] Create ChangeGitOrganizationVisibility command tests in `test/Hexalith.GitStorage.Tests/Domains/Commands/GitOrganizationCommandTests.cs`
- [ ] T062b [P] [US4] Create ChangeGitOrganizationVisibilityValidator tests in `test/Hexalith.GitStorage.Tests/Domains/Commands/GitOrganizationValidatorTests.cs`
- [X] T063 [P] [US4] Create command validator tests in `test/Hexalith.GitStorage.Tests/Domains/Commands/GitOrganizationValidatorTests.cs`

**Checkpoint**: At this point, User Story 4 should be fully functional - update, disable, and enable organizations

---

## Phase 7: User Story 5 - List Organizations (Priority: P5)

**Goal**: Allow administrators to browse a list of all Git Organizations so they can quickly find and manage organizations across all connected Git Storage Accounts.

**Independent Test**: List organizations and verify filtering by Git Storage Account works correctly.

### Implementation for User Story 5

- [X] T064 [US5] Create GetGitOrganizationSummaries request in `src/libraries/Application/Hexalith.GitStorage.Requests/GitOrganization/GetGitOrganizationSummaries.cs`
- [X] T065 [US5] Create GitOrganizationSummaryProjectionHandlers in `src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Summaries/`
- [X] T066 [US5] Create GetGitOrganizationSummariesHandler in `src/libraries/Application/Hexalith.GitStorage.Projections/Helpers/GitOrganizationProjectionHelper.cs`
- [X] T067 [US5] Register GetGitOrganizationSummaries request handler in `src/libraries/Application/Hexalith.GitStorage.Projections/Helpers/GitOrganizationProjectionHelper.cs`
- [X] T068 [US5] Add GET /git-organizations endpoint in `src/libraries/Infrastructure/Hexalith.GitStorage.WebServer/Controllers/GitOrganizationsController.cs`
- [ ] T069 [US5] Create GitOrganizationsPage.razor with Visibility column in `src/libraries/Presentation/Hexalith.GitStorage.UI.Pages/GitOrganizations/GitOrganizationsPage.razor`
- [ ] T070 [US5] Create GitOrganizationList.razor component with Visibility badges in `src/libraries/Presentation/Hexalith.GitStorage.UI.Components/GitOrganizations/GitOrganizationList.razor`
- [ ] T070a [P] [US5] Add Visibility filter dropdown to list view in `src/libraries/Presentation/Hexalith.GitStorage.UI.Pages/GitOrganizations/GitOrganizationsPage.razor`
- [ ] T071 [US5] Add GitOrganizations menu item to navigation `src/libraries/Presentation/Hexalith.GitStorage.UI.Components/Menu/GitStorageMenu.razor`

### Tests for User Story 5

- [ ] T072 [P] [US5] Create GetGitOrganizationSummaries request tests in `test/Hexalith.GitStorage.Tests/GitOrganization/GetGitOrganizationSummariesTests.cs`
- [ ] T073 [P] [US5] Create summary projection handler tests in `test/Hexalith.GitStorage.Tests/GitOrganization/GitOrganizationSummaryProjectionTests.cs`

**Checkpoint**: At this point, User Story 5 should be fully functional - list and filter organizations

---

## Phase 8: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

- [X] T074 [P] Add authorization policies to all endpoints in `src/libraries/Infrastructure/Hexalith.GitStorage.WebServer/Controllers/GitOrganizationsController.cs`
- [ ] T075 [P] Add logging for all GitOrganization operations in handlers
- [X] T076 [P] Register GitOrganization projections in `src/libraries/Application/Hexalith.GitStorage.Projections/Helpers/GitOrganizationProjectionHelper.cs`
- [ ] T077 Run quickstart.md validation - verify all code samples compile
- [X] T078 Run dotnet build to verify no compilation errors
- [X] T079 Run dotnet test to verify all tests pass

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Stories (Phase 3-7)**: All depend on Foundational phase completion
  - User stories can proceed in parallel (if staffed)
  - Or sequentially in priority order (P1 → P2 → P3 → P4 → P5)
- **Polish (Phase 8)**: Depends on all desired user stories being complete

### User Story Dependencies

| Story | Depends On | Can Parallel With |
|-------|------------|-------------------|
| US1 (Sync) | Foundational only | US2, US3, US4, US5 |
| US2 (Create) | Foundational only | US1, US3, US4, US5 |
| US3 (View Details) | Foundational only | US1, US2, US4, US5 |
| US4 (Update) | Foundational only | US1, US2, US3, US5 |
| US5 (List) | Foundational only | US1, US2, US3, US4 |

### Within Each User Story

- Commands before handlers
- Validators with commands
- Adapters before handlers that use them
- Handlers before API endpoints
- API endpoints before UI pages
- Tests can run in parallel with implementation

### Parallel Opportunities

**Phase 1 (all parallel)**:
- T001-T006 can all run in parallel

**Phase 2**:
- T007 first (base initialization event)
- T008-T012 in parallel (all other events)
- T013 after events (aggregate)
- T014-T015 in parallel (view models)
- T016-T019 after aggregate

**User Stories**: All 5 user stories can run in parallel after Foundational phase

---

## Parallel Example: Phase 1 Setup

```bash
# Launch all setup tasks together:
Task: "Create GitOrganizationOrigin enum"
Task: "Create GitOrganizationSyncStatus enum"
Task: "Create GitOrganizationDomainHelper"
Task: "Create base GitOrganizationEvent"
Task: "Create base GitOrganizationCommand"
Task: "Create base GitOrganizationRequest"
```

## Parallel Example: User Story 1 + User Story 2

```bash
# After Foundational complete, launch both stories:
# Team A works on US1:
Task: "Create SyncGitOrganizations command"
Task: "Create SyncGitOrganizationsHandler"

# Team B works on US2:
Task: "Create AddGitOrganization command"
Task: "Create AddGitOrganizationValidator"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup (T001-T006)
2. Complete Phase 2: Foundational (T007-T019)
3. Complete Phase 3: User Story 1 (T020-T028)
4. **STOP and VALIDATE**: Test sync from GitHub/Forgejo
5. Deploy/demo if ready

### Incremental Delivery

1. Setup + Foundational → Foundation ready
2. Add User Story 1 (Sync) → Test independently → Deploy (MVP!)
3. Add User Story 2 (Create) → Test independently → Deploy
4. Add User Story 3 (View) → Test independently → Deploy
5. Add User Story 4 (Update) → Test independently → Deploy
6. Add User Story 5 (List) → Test independently → Deploy

### Parallel Team Strategy

With 5 developers after Foundational:
- Developer A: User Story 1 (Sync)
- Developer B: User Story 2 (Create)
- Developer C: User Story 3 (View)
- Developer D: User Story 4 (Update)
- Developer E: User Story 5 (List)

---

## Summary

| Phase | Tasks | Description | Parallel |
|-------|-------|-------------|----------|
| Phase 1: Setup | 9 tasks | Enums + localizations including Visibility | All parallel |
| Phase 2: Foundational | 16 tasks | Events + aggregate including Visibility | T008-T012, T014-T015 parallel |
| Phase 3: US1 (Sync) | 9 tasks | Sync with Visibility from remote | T027-T028 parallel |
| Phase 4: US2 (Create) | 10 tasks | Create with Visibility | T037-T038 parallel |
| Phase 5: US3 (View) | 9 tasks | View details with Visibility badge | T046-T047 parallel |
| Phase 6: US4 (Update) | 22 tasks | Update including ChangeVisibility | T049-T050, T055a-c, T062a-b parallel |
| Phase 7: US5 (List) | 11 tasks | List with Visibility column/filter | T072-T073 parallel |
| Phase 8: Polish | 6 tasks | Final validation | T074-T076 parallel |
| **Total** | **92 tasks** | **Visibility adds ~13 new tasks** | |

### Visibility-Specific Tasks Summary

| Area | New Tasks |
|------|-----------|
| Enum | T002a (GitOrganizationVisibility) |
| Localizations | T006a, T006b (Visibility labels) |
| Events | T007, T008 (update), T009a, T009b (new VisibilityChanged) |
| Aggregate | T013, T013a, T013b (Visibility property + handlers) |
| ViewModels | T014, T015 (add Visibility property) |
| Commands | T029, T030 (update), T048a, T048b (ChangeVisibility) |
| Handlers | T055a (event), T055b, T055c (projections) |
| API | T057a (PATCH /visibility endpoint) |
| UI | T044, T045, T069, T070, T070a (Visibility display/filter) |
| Tests | T018, T019, T019a, T062a, T062b (Visibility tests) |

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- Verify tests fail before implementing
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
- Follow Hexalith DDD/CQRS/Event Sourcing patterns per CLAUDE.md
