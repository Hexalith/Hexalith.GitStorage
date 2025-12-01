# Tasks: Git Organization Entity

**Input**: Design documents from `/specs/002-git-organization/`
**Prerequisites**: plan.md, spec.md, research.md, data-model.md, contracts/api.yaml

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Project initialization for GitOrganization entity

- [ ] T001 [P] Create GitOrganizationDomainHelper.cs with aggregate name constant in src/libraries/Domain/Hexalith.GitStorage.Aggregates.Abstractions/GitOrganizationDomainHelper.cs
- [ ] T002 [P] Create GitOrganizationOrigin enum in src/libraries/Domain/Hexalith.GitStorage.Aggregates.Abstractions/Enums/GitOrganizationOrigin.cs
- [ ] T003 [P] Create GitOrganizationSyncStatus enum in src/libraries/Domain/Hexalith.GitStorage.Aggregates.Abstractions/Enums/GitOrganizationSyncStatus.cs

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core domain events and aggregate that ALL user stories depend on

**CRITICAL**: No user story work can begin until this phase is complete

### Domain Events (Required by all stories)

- [ ] T004 [P] Create GitOrganizationEvent base record in src/libraries/Domain/Hexalith.GitStorage.Events/GitOrganization/GitOrganizationEvent.cs
- [ ] T005 [P] Create GitOrganizationAdded event in src/libraries/Domain/Hexalith.GitStorage.Events/GitOrganization/GitOrganizationAdded.cs
- [ ] T006 [P] Create GitOrganizationSynced event in src/libraries/Domain/Hexalith.GitStorage.Events/GitOrganization/GitOrganizationSynced.cs
- [ ] T007 [P] Create GitOrganizationDescriptionChanged event in src/libraries/Domain/Hexalith.GitStorage.Events/GitOrganization/GitOrganizationDescriptionChanged.cs
- [ ] T008 [P] Create GitOrganizationMarkedNotFound event in src/libraries/Domain/Hexalith.GitStorage.Events/GitOrganization/GitOrganizationMarkedNotFound.cs
- [ ] T009 [P] Create GitOrganizationDisabled event in src/libraries/Domain/Hexalith.GitStorage.Events/GitOrganization/GitOrganizationDisabled.cs
- [ ] T010 [P] Create GitOrganizationEnabled event in src/libraries/Domain/Hexalith.GitStorage.Events/GitOrganization/GitOrganizationEnabled.cs

### Aggregate (Required by all stories)

- [ ] T011 Create GitOrganization aggregate with Apply methods for all events in src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitOrganization.cs
- [ ] T012 Create GitOrganizationValidator for organization name validation in src/libraries/Domain/Hexalith.GitStorage.Aggregates/Validators/GitOrganizationValidator.cs

### Base Command and Request (Required by all stories)

- [ ] T013 [P] Create GitOrganizationCommand base record in src/libraries/Application/Hexalith.GitStorage.Commands/GitOrganization/GitOrganizationCommand.cs
- [ ] T014 [P] Create GitOrganizationRequest base record in src/libraries/Application/Hexalith.GitStorage.Requests/GitOrganization/GitOrganizationRequest.cs

### View Models (Required for queries)

- [ ] T015 [P] Create GitOrganizationDetailsViewModel in src/libraries/Application/Hexalith.GitStorage.Requests/GitOrganization/GitOrganizationDetailsViewModel.cs
- [ ] T016 [P] Create GitOrganizationSummaryViewModel in src/libraries/Application/Hexalith.GitStorage.Requests/GitOrganization/GitOrganizationSummaryViewModel.cs

### Provider Abstraction (Required for remote operations)

- [ ] T017 Create IGitProviderAdapter interface with methods for ListOrganizationsAsync, CreateOrganizationAsync, UpdateOrganizationAsync; include credential validation that throws GitProviderAuthenticationException on invalid/expired credentials in src/libraries/Infrastructure/Hexalith.GitStorage.Abstractions/IGitProviderAdapter.cs
- [ ] T018 Create GitOrganizationDto for adapter responses in src/libraries/Infrastructure/Hexalith.GitStorage.Abstractions/GitOrganizationDto.cs

**Checkpoint**: Foundation ready - user story implementation can now begin

---

## Phase 3: User Story 1 - Synchronize Organizations from Git Storage Account (Priority: P1) MVP

**Goal**: Administrators can sync organizations from a connected Git Storage Account to see and manage all remote organizations locally

**Independent Test**: Trigger a sync operation for a configured Git Storage Account and verify that remote organizations appear in the local organization list with correct SyncStatus

### Implementation for User Story 1

- [ ] T019 [US1] Create SyncGitOrganizations command in src/libraries/Application/Hexalith.GitStorage.Commands/GitOrganization/SyncGitOrganizations.cs
  > **Note**: This is a bulk operation command. AggregateId is the GitStorageAccountId being synced. Handler iterates remote orgs and emits individual GitOrganizationSynced/GitOrganizationMarkedNotFound events per organization.
- [ ] T020 [US1] Implement SyncGitOrganizationsHandler command handler with optimistic concurrency check (prevent concurrent syncs for same GitStorageAccountId) in src/libraries/Application/Hexalith.GitStorage/CommandHandlers/SyncGitOrganizationsHandler.cs
- [ ] T021 [US1] Create GetGitOrganizationSummaries request in src/libraries/Application/Hexalith.GitStorage.Requests/GitOrganization/GetGitOrganizationSummaries.cs
- [ ] T022 [US1] Implement GitOrganizationSyncedOnGitOrganizationDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Details/GitOrganizationSyncedOnGitOrganizationDetailsProjectionHandler.cs
- [ ] T023 [US1] Implement GitOrganizationSyncedOnGitOrganizationSummaryProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Summaries/GitOrganizationSyncedOnGitOrganizationSummaryProjectionHandler.cs
- [ ] T024 [US1] Implement GitOrganizationMarkedNotFoundOnGitOrganizationDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Details/GitOrganizationMarkedNotFoundOnGitOrganizationDetailsProjectionHandler.cs
- [ ] T025 [US1] Implement GitOrganizationMarkedNotFoundOnGitOrganizationSummaryProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Summaries/GitOrganizationMarkedNotFoundOnGitOrganizationSummaryProjectionHandler.cs
- [ ] T026 [US1] Create POST /git-storage-accounts/{id}/sync-organizations endpoint in GitOrganizationWebApiHelpers in src/libraries/Infrastructure/Hexalith.GitStorage.Servers/Helpers/GitOrganizationWebApiHelpers.cs
- [ ] T027 [US1] Implement GitOrganizationIntegrationEventsController for sync command in src/libraries/Infrastructure/Hexalith.GitStorage.ApiServer/Controllers/GitOrganizationIntegrationEventsController.cs

**Checkpoint**: User Story 1 complete - administrators can sync organizations from Git Storage Account

---

## Phase 4: User Story 2 - Create Organization via Application API (Priority: P2)

**Goal**: Administrators can create new organizations through the API, which creates them both locally and on the remote Git Server

**Independent Test**: Create a new organization via the API and verify both local persistence and creation on the remote git server

### Implementation for User Story 2

- [ ] T028 [US2] Create AddGitOrganization command in src/libraries/Application/Hexalith.GitStorage.Commands/GitOrganization/AddGitOrganization.cs
- [ ] T029 [US2] Implement AddGitOrganizationHandler command handler in src/libraries/Application/Hexalith.GitStorage/CommandHandlers/AddGitOrganizationHandler.cs
- [ ] T030 [US2] Implement GitOrganizationAddedOnGitOrganizationDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Details/GitOrganizationAddedOnGitOrganizationDetailsProjectionHandler.cs
- [ ] T031 [US2] Implement GitOrganizationAddedOnGitOrganizationSummaryProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Summaries/GitOrganizationAddedOnGitOrganizationSummaryProjectionHandler.cs
- [ ] T032 [US2] Create GitOrganizationAddedEventHandler to create organization on remote via IGitProviderAdapter in src/libraries/Application/Hexalith.GitStorage/EventHandlers/GitOrganizationAddedEventHandler.cs
- [ ] T033 [US2] Add POST /git-organizations endpoint to GitOrganizationWebApiHelpers in src/libraries/Infrastructure/Hexalith.GitStorage.Servers/Helpers/GitOrganizationWebApiHelpers.cs
- [ ] T034 [US2] Add create organization endpoint to GitOrganizationIntegrationEventsController in src/libraries/Infrastructure/Hexalith.GitStorage.ApiServer/Controllers/GitOrganizationIntegrationEventsController.cs

**Checkpoint**: User Story 2 complete - administrators can create organizations via API

---

## Phase 5: User Story 3 - View Organization Details (Priority: P3)

**Goal**: Administrators can view full details of a Git Organization including configuration, sync status, and Git Storage Account relationship

**Independent Test**: Retrieve an existing organization by ID and verify all stored details are displayed correctly

### Implementation for User Story 3

- [ ] T035 [US3] Create GetGitOrganizationDetails request in src/libraries/Application/Hexalith.GitStorage.Requests/GitOrganization/GetGitOrganizationDetails.cs
- [ ] T036 [US3] Add GET /git-organizations/{id} endpoint to GitOrganizationWebApiHelpers in src/libraries/Infrastructure/Hexalith.GitStorage.Servers/Helpers/GitOrganizationWebApiHelpers.cs
- [ ] T037 [US3] Add get organization details endpoint to GitOrganizationIntegrationEventsController in src/libraries/Infrastructure/Hexalith.GitStorage.ApiServer/Controllers/GitOrganizationIntegrationEventsController.cs

**Checkpoint**: User Story 3 complete - administrators can view organization details

---

## Phase 6: User Story 4 - Update Organization via Application (Priority: P4)

**Goal**: Administrators can update organization settings (description) and changes are synced to the remote Git Server

**Independent Test**: Modify an existing organization's description and verify changes are persisted locally and updated on the remote server

### Implementation for User Story 4

- [ ] T038 [US4] Create ChangeGitOrganizationDescription command in src/libraries/Application/Hexalith.GitStorage.Commands/GitOrganization/ChangeGitOrganizationDescription.cs
- [ ] T039 [US4] Implement ChangeGitOrganizationDescriptionHandler command handler in src/libraries/Application/Hexalith.GitStorage/CommandHandlers/ChangeGitOrganizationDescriptionHandler.cs
- [ ] T040 [US4] Implement GitOrganizationDescriptionChangedOnGitOrganizationDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Details/GitOrganizationDescriptionChangedOnGitOrganizationDetailsProjectionHandler.cs
- [ ] T041 [US4] Create GitOrganizationDescriptionChangedEventHandler to sync changes to remote via IGitProviderAdapter in src/libraries/Application/Hexalith.GitStorage/EventHandlers/GitOrganizationDescriptionChangedEventHandler.cs
- [ ] T042 [US4] Add PATCH /git-organizations/{id} endpoint to GitOrganizationWebApiHelpers in src/libraries/Infrastructure/Hexalith.GitStorage.Servers/Helpers/GitOrganizationWebApiHelpers.cs
- [ ] T043 [US4] Add update organization endpoint to GitOrganizationIntegrationEventsController in src/libraries/Infrastructure/Hexalith.GitStorage.ApiServer/Controllers/GitOrganizationIntegrationEventsController.cs

**Checkpoint**: User Story 4 complete - administrators can update organizations

---

## Phase 7: User Story 5 - List Organizations (Priority: P5)

**Goal**: Administrators can browse a paginated list of all Git Organizations with filtering by Git Storage Account

**Independent Test**: List organizations and verify filtering by Git Storage Account works correctly

### Implementation for User Story 5

- [ ] T044 [US5] Add GET /git-organizations endpoint with pagination and filtering to GitOrganizationWebApiHelpers in src/libraries/Infrastructure/Hexalith.GitStorage.Servers/Helpers/GitOrganizationWebApiHelpers.cs
- [ ] T045 [US5] Add list organizations endpoint to GitOrganizationIntegrationEventsController in src/libraries/Infrastructure/Hexalith.GitStorage.ApiServer/Controllers/GitOrganizationIntegrationEventsController.cs

**Checkpoint**: User Story 5 complete - administrators can list and filter organizations

---

## Phase 8: Additional Operations (Disable/Enable)

**Purpose**: Enable/Disable functionality as specified in data model

- [ ] T046 [P] Create DisableGitOrganization command in src/libraries/Application/Hexalith.GitStorage.Commands/GitOrganization/DisableGitOrganization.cs
- [ ] T047 [P] Create EnableGitOrganization command in src/libraries/Application/Hexalith.GitStorage.Commands/GitOrganization/EnableGitOrganization.cs
- [ ] T048 Implement DisableGitOrganizationHandler command handler in src/libraries/Application/Hexalith.GitStorage/CommandHandlers/DisableGitOrganizationHandler.cs
- [ ] T049 Implement EnableGitOrganizationHandler command handler in src/libraries/Application/Hexalith.GitStorage/CommandHandlers/EnableGitOrganizationHandler.cs
- [ ] T050 [P] Implement GitOrganizationDisabledOnGitOrganizationDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Details/GitOrganizationDisabledOnGitOrganizationDetailsProjectionHandler.cs
- [ ] T051 [P] Implement GitOrganizationDisabledOnGitOrganizationSummaryProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Summaries/GitOrganizationDisabledOnGitOrganizationSummaryProjectionHandler.cs
- [ ] T052 [P] Implement GitOrganizationEnabledOnGitOrganizationDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Details/GitOrganizationEnabledOnGitOrganizationDetailsProjectionHandler.cs
- [ ] T053 [P] Implement GitOrganizationEnabledOnGitOrganizationSummaryProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Summaries/GitOrganizationEnabledOnGitOrganizationSummaryProjectionHandler.cs
- [ ] T054 Add POST /git-organizations/{id}/disable endpoint to GitOrganizationWebApiHelpers in src/libraries/Infrastructure/Hexalith.GitStorage.Servers/Helpers/GitOrganizationWebApiHelpers.cs
- [ ] T055 Add POST /git-organizations/{id}/enable endpoint to GitOrganizationWebApiHelpers in src/libraries/Infrastructure/Hexalith.GitStorage.Servers/Helpers/GitOrganizationWebApiHelpers.cs
- [ ] T056 Add disable/enable endpoints to GitOrganizationIntegrationEventsController in src/libraries/Infrastructure/Hexalith.GitStorage.ApiServer/Controllers/GitOrganizationIntegrationEventsController.cs

**Checkpoint**: All CRUD and lifecycle operations available

---

## Phase 9: Polish & Cross-Cutting Concerns

**Purpose**: Final integration, registration, and validation

- [ ] T057 Register GitOrganization aggregate in domain module registration in src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitStorageAggregatesModule.cs
- [ ] T058 Register GitOrganization events in event module registration in src/libraries/Domain/Hexalith.GitStorage.Events/GitStorageEventsModule.cs
- [ ] T059 Register GitOrganization commands in command module registration in src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageCommandsModule.cs
- [ ] T060 Register GitOrganization requests in request module registration in src/libraries/Application/Hexalith.GitStorage.Requests/GitStorageRequestsModule.cs
- [ ] T061 Register GitOrganization projection handlers in projections module in src/libraries/Application/Hexalith.GitStorage.Projections/GitStorageProjectionsModule.cs
- [ ] T062 Register GitOrganization command handlers in Hexalith.GitStorage module in src/libraries/Application/Hexalith.GitStorage/GitStorageModule.cs
- [ ] T063 Register GitOrganization event handlers in Hexalith.GitStorage module in src/libraries/Application/Hexalith.GitStorage/GitStorageModule.cs
- [ ] T064 Build solution and verify no compilation errors with dotnet build
- [ ] T065 Run quickstart.md validation scenarios manually

---

## Phase 10: Unit Tests (Constitution Compliance)

**Purpose**: Satisfy Constitution Principle VI - Test-First Development

- [ ] T066 [P] Create GitOrganizationAggregateTests with Apply method tests for all events in tests/Hexalith.GitStorage.UnitTests/GitOrganization/GitOrganizationAggregateTests.cs
- [ ] T067 [P] Create GitOrganizationEventsSerializationTests for JSON round-trip tests in tests/Hexalith.GitStorage.UnitTests/GitOrganization/GitOrganizationEventsSerializationTests.cs
- [ ] T068 [P] Create GitOrganizationCommandsValidationTests for command validation in tests/Hexalith.GitStorage.UnitTests/GitOrganization/GitOrganizationCommandsValidationTests.cs

**Checkpoint**: All constitution-mandated tests in place

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Stories (Phase 3-7)**: All depend on Foundational phase completion
  - User stories can proceed sequentially in priority order (P1 → P2 → P3 → P4 → P5)
  - US1 (Sync) and US2 (Create) can potentially run in parallel if staffed
- **Additional Operations (Phase 8)**: Depends on Foundational phase completion
- **Polish (Phase 9)**: Depends on all user stories being complete
- **Unit Tests (Phase 10)**: Depends on Foundational phase completion (can run in parallel with user stories)

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational (Phase 2) - No dependencies on other stories
- **User Story 2 (P2)**: Can start after Foundational (Phase 2) - No dependencies on other stories
- **User Story 3 (P3)**: Can start after Foundational (Phase 2) - Benefits from US1/US2 data but independently testable
- **User Story 4 (P4)**: Can start after Foundational (Phase 2) - Benefits from US1/US2 data but independently testable
- **User Story 5 (P5)**: Can start after Foundational (Phase 2) - Benefits from US1/US2 data but independently testable

### Within Each Phase

- Events before aggregate (aggregate depends on event types)
- Base command/request before specific commands/requests
- Commands before handlers
- Handlers before projection handlers
- Domain before API endpoints
- Core implementation before integration

### Parallel Opportunities

**Phase 1 (Setup)**:
```
T001 || T002 || T003  (all different files)
```

**Phase 2 (Foundational)**:
```
T004 || T005 || T006 || T007 || T008 || T009 || T010  (all event files)
Then: T011 (aggregate needs events)
Then: T012 (validator)
T013 || T014  (base command and request)
T015 || T016  (view models)
T017 → T018  (interface then DTO)
```

**Phase 3-7 (User Stories)**:
Each user story can be worked on independently once Foundational is complete.

---

## Parallel Example: Phase 2 Foundational Events

```bash
# Launch all domain events in parallel:
Task: "Create GitOrganizationEvent base record"
Task: "Create GitOrganizationAdded event"
Task: "Create GitOrganizationSynced event"
Task: "Create GitOrganizationDescriptionChanged event"
Task: "Create GitOrganizationMarkedNotFound event"
Task: "Create GitOrganizationDisabled event"
Task: "Create GitOrganizationEnabled event"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup (T001-T003)
2. Complete Phase 2: Foundational (T004-T018) - CRITICAL, blocks all stories
3. Complete Phase 3: User Story 1 - Sync Organizations (T019-T027)
4. **STOP and VALIDATE**: Test sync operation independently
5. Deploy/demo if ready

### Incremental Delivery

1. Complete Setup + Foundational → Foundation ready
2. Add User Story 1 (Sync) → Test independently → Deploy/Demo (MVP!)
3. Add User Story 2 (Create) → Test independently → Deploy/Demo
4. Add User Story 3 (View Details) → Test independently → Deploy/Demo
5. Add User Story 4 (Update) → Test independently → Deploy/Demo
6. Add User Story 5 (List) → Test independently → Deploy/Demo
7. Add Phase 8 (Disable/Enable) → Complete feature set
8. Each story adds value without breaking previous stories

### Full Implementation (All Stories)

If implementing all stories sequentially:

1. Phase 1: ~3 tasks
2. Phase 2: ~15 tasks (foundation)
3. Phase 3-7: ~27 tasks (5 user stories)
4. Phase 8: ~11 tasks (additional operations)
5. Phase 9: ~9 tasks (polish)
6. Phase 10: ~3 tasks (unit tests)
7. **Total: 68 tasks**

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
- All files require copyright header per CLAUDE.md
- Follow Hexalith naming conventions: Events use past tense (Added, Synced), Commands use imperative (Add, Sync)
- Use [PolymorphicSerialization] attribute on events, commands, requests
- Use [DataContract] and [DataMember(Order = N)] for serialization
