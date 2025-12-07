# Tasks: Git API Credentials

**Input**: Design documents from `/specs/003-git-api-credentials/`
**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Tests**: Included per spec requirements (VI. Test-First Development principle from constitution)

**Organization**: Tasks are grouped by user story to enable independent implementation and testing of each story.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Path Conventions

Based on plan.md structure:
- **Domain**: `src/libraries/Domain/`
- **Application**: `src/libraries/Application/`
- **Presentation**: `src/libraries/Presentation/`
- **Tests**: `test/Hexalith.GitStorage.UnitTests/`

---

## Phase 1: Setup (Shared Infrastructure)

**Purpose**: Create foundational types required by all user stories

- [X] T001 [P] Create GitServerProviderType enum in src/libraries/Domain/Hexalith.GitStorage.Aggregates.Abstractions/Enums/GitServerProviderType.cs
- [X] T002 Verify solution builds after enum creation with `dotnet build Hexalith.GitStorage.sln`

---

## Phase 2: Foundational (Blocking Prerequisites)

**Purpose**: Core domain events that ALL user stories depend on

**⚠️ CRITICAL**: No user story work can begin until this phase is complete

- [X] T003 [P] Create GitStorageAccountApiCredentialsChanged event in src/libraries/Domain/Hexalith.GitStorage.Events/GitStorageAccount/GitStorageAccountApiCredentialsChanged.cs
- [X] T004 [P] Create GitStorageAccountApiCredentialsCleared event in src/libraries/Domain/Hexalith.GitStorage.Events/GitStorageAccount/GitStorageAccountApiCredentialsCleared.cs
- [X] T005 [P] Create GitStorageAccountApiCredentialsChangedValidator in src/libraries/Domain/Hexalith.GitStorage.Events/GitStorageAccount/GitStorageAccountApiCredentialsChangedValidator.cs
- [X] T006 [P] Create GitStorageAccountApiCredentialsClearedValidator in src/libraries/Domain/Hexalith.GitStorage.Events/GitStorageAccount/GitStorageAccountApiCredentialsClearedValidator.cs
- [X] T007 Extend GitStorageAccountAdded event with optional ServerUrl, AccessToken, ProviderType fields (DataMember Order 4, 5, 6) in src/libraries/Domain/Hexalith.GitStorage.Events/GitStorageAccount/GitStorageAccountAdded.cs
- [X] T008 Update GitStorageAccountAddedValidator for new optional fields in src/libraries/Domain/Hexalith.GitStorage.Events/GitStorageAccount/GitStorageAccountAddedValidator.cs
- [X] T009 Extend GitStorageAccount aggregate with ServerUrl, AccessToken, ProviderType fields (DataMember Order 8, 9, 10) in src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitStorageAccount.cs
- [X] T010 Add Apply overloads for GitStorageAccountApiCredentialsChanged and GitStorageAccountApiCredentialsCleared events in src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitStorageAccount.cs
- [X] T011 Update GitStorageAccount constructor from GitStorageAccountAdded to handle new credential fields in src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitStorageAccount.cs
- [X] T012 Verify solution builds after domain layer changes with `dotnet build Hexalith.GitStorage.sln`

**Checkpoint**: Foundation ready - user story implementation can now begin in parallel

---

## Phase 3: User Story 1 - Configure Git Server Connection (Priority: P1) 🎯 MVP

**Goal**: Administrators can configure API connection credentials for a Git storage account

**Independent Test**: Create a new Git storage account with API credentials via command and verify fields are stored correctly

### Tests for User Story 1 ⚠️

> **NOTE: Write these tests FIRST, ensure they FAIL before implementation**

- [X] T013 [P] [US1] Create aggregate tests for Apply GitStorageAccountApiCredentialsChanged in test/Hexalith.GitStorage.Tests/Domains/Aggregates/GitStorageAccountApiCredentialsTests.cs
- [X] T014 [P] [US1] Create aggregate tests for Apply GitStorageAccountApiCredentialsCleared in test/Hexalith.GitStorage.Tests/Domains/Aggregates/GitStorageAccountApiCredentialsTests.cs
- [X] T015 [P] [US1] Create aggregate tests for GitStorageAccountAdded with credentials in test/Hexalith.GitStorage.Tests/Domains/Aggregates/GitStorageAccountApiCredentialsTests.cs
- [X] T016 [P] [US1] Create ChangeGitStorageAccountApiCredentialsValidator tests (valid HTTPS URL, non-empty token, valid enum) in test/Hexalith.GitStorage.Tests/Domains/Commands/ChangeGitStorageAccountApiCredentialsValidatorTests.cs
- [X] T017 [P] [US1] Create ClearGitStorageAccountApiCredentialsValidator tests in test/Hexalith.GitStorage.Tests/Domains/Commands/ClearGitStorageAccountApiCredentialsValidatorTests.cs
- [X] T017b [P] [US1] Create JSON serialization round-trip tests for new events (GitStorageAccountApiCredentialsChanged, GitStorageAccountApiCredentialsCleared) in test/Hexalith.GitStorage.Tests/Domains/Events/GitStorageAccountApiCredentialsSerializationTests.cs

### Implementation for User Story 1

- [X] T018 [P] [US1] Create ChangeGitStorageAccountApiCredentials command in src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/ChangeGitStorageAccountApiCredentials.cs
- [X] T019 [P] [US1] Create ClearGitStorageAccountApiCredentials command in src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/ClearGitStorageAccountApiCredentials.cs
- [X] T020 [US1] Create ChangeGitStorageAccountApiCredentialsValidator with HTTPS URL validation in src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/ChangeGitStorageAccountApiCredentialsValidator.cs
- [X] T021 [US1] Create ClearGitStorageAccountApiCredentialsValidator in src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/ClearGitStorageAccountApiCredentialsValidator.cs
- [X] T022 [US1] Extend AddGitStorageAccount command with optional ServerUrl, AccessToken, ProviderType fields in src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/AddGitStorageAccount.cs
- [X] T023 [US1] Update AddGitStorageAccountValidator for conditional API credential validation in src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/AddGitStorageAccountValidator.cs
- [X] T024 [US1] Add PUT endpoint handler for /api/git-storage-accounts/{id}/api-credentials in src/libraries/Infrastructure/Hexalith.GitStorage.WebServer/Controllers/GitStorageAccountsController.cs
- [X] T025 [US1] Add DELETE endpoint handler for /api/git-storage-accounts/{id}/api-credentials in src/libraries/Infrastructure/Hexalith.GitStorage.WebServer/Controllers/GitStorageAccountsController.cs
- [X] T026 [US1] Run unit tests for US1 with `dotnet test` and fix any failures

**Checkpoint**: At this point, User Story 1 should be fully functional and testable independently

---

## Phase 4: User Story 2 - View Git Server Connection Details (Priority: P2)

**Goal**: Administrators can view configured Git server connection details with masked credentials

**Independent Test**: View a Git storage account details page and confirm all configured API fields are displayed (with token masked)

### Tests for User Story 2 ⚠️

- [ ] T027 [P] [US2] Create tests for MaskedAccessToken computed property (>4 chars, <=4 chars, null) in test/Hexalith.GitStorage.UnitTests/Requests/GitStorageAccountDetailsViewModelTests.cs
- [ ] T028 [P] [US2] Create tests for HasApiCredentials computed property in test/Hexalith.GitStorage.UnitTests/Requests/GitStorageAccountDetailsViewModelTests.cs

### Implementation for User Story 2

- [ ] T029 [US2] Extend GitStorageAccountDetailsViewModel with ServerUrl, AccessToken (internal), MaskedAccessToken (computed), ProviderType, HasApiCredentials in src/libraries/Application/Hexalith.GitStorage.Requests/GitStorageAccount/GitStorageAccountDetailsViewModel.cs
- [ ] T030 [P] [US2] Create GitStorageAccountApiCredentialsChangedOnDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Details/GitStorageAccountApiCredentialsChangedOnDetailsProjectionHandler.cs
- [ ] T031 [P] [US2] Create GitStorageAccountApiCredentialsClearedOnDetailsProjectionHandler in src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Details/GitStorageAccountApiCredentialsClearedOnDetailsProjectionHandler.cs
- [ ] T032 [US2] Update GitStorageAccountAddedOnDetailsProjectionHandler to map new credential fields in src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Details/GitStorageAccountAddedOnDetailsProjectionHandler.cs
- [ ] T033 [US2] Register new projection handlers in DI container in src/libraries/Application/Hexalith.GitStorage.Projections/GitStorageProjectionsHelper.cs
- [ ] T034 [US2] Run unit tests for US2 with `dotnet test` and fix any failures

**Checkpoint**: At this point, User Stories 1 AND 2 should both work independently

---

## Phase 5: User Story 3 - Select Git Server Provider Type (Priority: P3)

**Goal**: Administrators can specify the Git server provider type (GitHub, Forgejo, Gitea) when configuring a storage account

**Independent Test**: Create accounts with different provider types and verify the provider type is stored and displayed correctly

### Implementation for User Story 3

- [ ] T035 [US3] Extend GitStorageAccountEditViewModel with ServerUrl, AccessToken, ProviderType properties and change tracking in src/libraries/Presentation/Hexalith.GitStorage.UI.Pages/GitStorageAccount/GitStorageAccountEditViewModel.cs
- [ ] T036 [US3] Add ApiCredentialsChanged computed property for change detection in src/libraries/Presentation/Hexalith.GitStorage.UI.Pages/GitStorageAccount/GitStorageAccountEditViewModel.cs
- [ ] T037 [US3] Update SaveAsync method to dispatch ChangeGitStorageAccountApiCredentials when credentials changed in src/libraries/Presentation/Hexalith.GitStorage.UI.Pages/GitStorageAccount/GitStorageAccountEditViewModel.cs
- [ ] T038 [US3] Add UI form fields for Server URL (text input), Access Token (password input), Provider Type (dropdown) in src/libraries/Presentation/Hexalith.GitStorage.UI.Pages/GitStorageAccount/GitStorageAccountEditPage.razor
- [ ] T039 [US3] Add localization strings for new API credential fields in src/libraries/Domain/Hexalith.GitStorage.Localizations/Resources/ (if applicable)
- [ ] T040 [US3] Verify provider type dropdown defaults to GitHub when not specified
- [ ] T041 [US3] Run full build and tests with `dotnet build Hexalith.GitStorage.sln && dotnet test`

**Checkpoint**: All user stories should now be independently functional

---

## Phase 6: Polish & Cross-Cutting Concerns

**Purpose**: Improvements that affect multiple user stories

- [ ] T042 [P] Review and ensure all new files have correct copyright headers
- [ ] T043 [P] Verify all public members have XML documentation
- [ ] T044 [P] Ensure DataMember Order values don't conflict with existing fields
- [ ] T045 Run quickstart.md validation scenarios
- [ ] T046 Run final build and all tests with `dotnet build Hexalith.GitStorage.sln && dotnet test`

---

## Dependencies & Execution Order

### Phase Dependencies

- **Setup (Phase 1)**: No dependencies - can start immediately
- **Foundational (Phase 2)**: Depends on Setup completion - BLOCKS all user stories
- **User Stories (Phase 3-5)**: All depend on Foundational phase completion
  - User stories can then proceed in parallel (if staffed)
  - Or sequentially in priority order (P1 → P2 → P3)
- **Polish (Phase 6)**: Depends on all desired user stories being complete

### User Story Dependencies

- **User Story 1 (P1)**: Can start after Foundational (Phase 2) - No dependencies on other stories
- **User Story 2 (P2)**: Can start after Foundational (Phase 2) - Uses ViewModel extended in US1 implementation, but independently testable
- **User Story 3 (P3)**: Can start after Foundational (Phase 2) - Uses components from US1/US2, but independently testable via UI

### Within Each User Story

- Tests (if included) MUST be written and FAIL before implementation
- Domain changes before Application layer
- Commands before ViewModels
- ViewModels before Projection handlers
- Core implementation before UI integration
- Story complete before moving to next priority

### Parallel Opportunities

- T001 (enum) can run independently
- T003, T004, T005, T006 can all run in parallel (different event files)
- T013-T017 (US1 tests) can all run in parallel
- T018, T019 (US1 commands) can run in parallel
- T027, T028 (US2 tests) can run in parallel
- T030, T031 (US2 projection handlers) can run in parallel

---

## Parallel Example: User Story 1

```bash
# Launch all tests for User Story 1 together:
Task: "Create aggregate tests for Apply GitStorageAccountApiCredentialsChanged in test/Hexalith.GitStorage.UnitTests/Aggregates/GitStorageAccountApiCredentialsTests.cs"
Task: "Create ChangeGitStorageAccountApiCredentialsValidator tests in test/Hexalith.GitStorage.UnitTests/Commands/ChangeGitStorageAccountApiCredentialsValidatorTests.cs"

# Launch parallel commands for User Story 1:
Task: "Create ChangeGitStorageAccountApiCredentials command in src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/ChangeGitStorageAccountApiCredentials.cs"
Task: "Create ClearGitStorageAccountApiCredentials command in src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/ClearGitStorageAccountApiCredentials.cs"
```

---

## Implementation Strategy

### MVP First (User Story 1 Only)

1. Complete Phase 1: Setup
2. Complete Phase 2: Foundational (CRITICAL - blocks all stories)
3. Complete Phase 3: User Story 1
4. **STOP and VALIDATE**: Test User Story 1 independently
5. Deploy/demo if ready

### Incremental Delivery

1. Complete Setup + Foundational → Foundation ready
2. Add User Story 1 → Test independently → Deploy/Demo (MVP!)
3. Add User Story 2 → Test independently → Deploy/Demo
4. Add User Story 3 → Test independently → Deploy/Demo
5. Each story adds value without breaking previous stories

### Parallel Team Strategy

With multiple developers:

1. Team completes Setup + Foundational together
2. Once Foundational is done:
   - Developer A: User Story 1
   - Developer B: User Story 2 (after T029 ViewModel is ready from US1)
   - Developer C: User Story 3 (after US1/US2 core components)
3. Stories complete and integrate independently

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- Verify tests fail before implementing
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
- Avoid: vague tasks, same file conflicts, cross-story dependencies that break independence

---

## Summary

| Metric | Count |
|--------|-------|
| Total Tasks | 46 |
| Setup Tasks | 2 |
| Foundational Tasks | 10 |
| User Story 1 Tasks | 14 |
| User Story 2 Tasks | 8 |
| User Story 3 Tasks | 7 |
| Polish Tasks | 5 |
| Parallel Opportunities | 22 tasks marked [P] |
| MVP Scope | Phases 1-3 (26 tasks) |
