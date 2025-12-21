# Feature Specification: Git API Credentials

**Feature Branch**: `003-git-api-credentials`
**Created**: 2025-12-07
**Status**: Draft
**Input**: User description: "Add fields needed to connect to the GitHub/Forgejo servers API to the git account aggregate and UI"

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Configure Git Server Connection (Priority: P1)

As an administrator, I want to configure API connection credentials for a Git storage account so that the system can authenticate and communicate with GitHub or Forgejo servers to sync repositories and organizations.

**Why this priority**: This is the core functionality - without API credentials, the system cannot communicate with external Git servers. All other Git-related features depend on this connection being established.

**Independent Test**: Can be fully tested by creating a new Git storage account with API credentials and verifying the connection parameters are stored correctly. The system should be able to validate that connection settings are complete.

**Acceptance Scenarios**:

1. **Given** I am on the Git Storage Account creation page, **When** I provide server URL, access token, and optional settings, **Then** the account is created with all API connection fields stored securely.
2. **Given** I am viewing an existing Git Storage Account, **When** I update the API credentials (e.g., new access token), **Then** the credentials are updated without affecting other account properties.
3. **Given** I am configuring a Git Storage Account, **When** I provide an invalid server URL format, **Then** the system displays a validation error and prevents saving.

---

### User Story 2 - View Git Server Connection Details (Priority: P2)

As an administrator, I want to view the configured Git server connection details for a storage account so that I can verify the configuration is correct and troubleshoot connectivity issues.

**Why this priority**: Administrators need visibility into connection settings for troubleshooting and auditing purposes, but this is secondary to the ability to configure credentials.

**Independent Test**: Can be tested by viewing a Git storage account details page and confirming all configured API fields (except sensitive credentials) are displayed.

**Acceptance Scenarios**:

1. **Given** a Git Storage Account with configured API credentials, **When** I view the account details, **Then** I see the server URL, provider type, and whether credentials are configured (but not the full access token).
2. **Given** a Git Storage Account without configured API credentials, **When** I view the account details, **Then** I see empty or placeholder values indicating credentials need to be configured.

---

### User Story 3 - Select Git Server Provider Type (Priority: P3)

As an administrator, I want to specify the Git server provider type (GitHub, Forgejo, Gitea) when configuring a storage account so that the system can use the appropriate API dialect for that provider.

**Why this priority**: Different Git providers may have API variations; however, GitHub and Forgejo/Gitea share similar APIs, so this is lower priority than core credential configuration.

**Independent Test**: Can be tested by creating accounts with different provider types and verifying the provider type is stored and displayed correctly.

**Acceptance Scenarios**:

1. **Given** I am configuring a Git Storage Account, **When** I select a provider type from the dropdown (GitHub, Forgejo, Gitea), **Then** the provider type is saved with the account.
2. **Given** I do not specify a provider type, **When** I save the account, **Then** the system defaults to a sensible provider type (GitHub).

---

### Edge Cases

- What happens when the access token is expired or revoked? **[DEFERRED]** Authentication failure handling will be addressed in a future feature when API connectivity testing is implemented. This feature only stores credentials.
- How does the system handle server URL changes? The system should allow updating the server URL independently of other credentials.
- What happens if the user provides a server URL without the protocol (e.g., `github.com` instead of `https://github.com`)? The system should reject invalid URLs with a clear validation message. URL normalization (auto-adding https://) is explicitly out of scope per research.md Decision 3.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: System MUST store a server URL (base API endpoint) for each Git Storage Account; initial values can be set via `AddGitStorageAccount` command.
- **FR-002**: System MUST store an access token for authenticating with the Git server API.
- **FR-003**: System MUST store a provider type indicating the Git server platform (GitHub, Forgejo, Gitea, or other compatible platforms).
- **FR-004**: System MUST validate that the server URL is a valid HTTPS URL format before saving.
- **FR-005**: System MUST display access tokens partially masked, showing only the last 4 characters (e.g., `••••••••abc1`) to allow verification while preventing full exposure.
- **FR-006**: System MUST allow updating API credentials via a single unified command (`ChangeGitStorageAccountApiCredentials`) that updates server URL, access token, and provider type together.
- **FR-007**: System MUST allow clearing/removing API credentials from an existing account.
- **FR-008**: System MUST provide a default provider type when none is explicitly selected.
- **FR-009**: System MUST emit domain events when API credentials are added or changed.
- **FR-010**: System MUST validate that access token is not empty when provided.

### Key Entities

- **GitStorageAccount**: Extended with API connection fields:
  - Server URL (the base URL of the Git server API, e.g., `https://api.github.com` or `https://forgejo.example.com/api/v1`)
  - Access Token (personal access token or OAuth token for API authentication)
  - Provider Type (enum: GitHub, Forgejo, Gitea, Generic)

- **GitServerProviderType**: An enumeration representing supported Git server platforms to enable provider-specific API handling.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Administrators can configure Git server API credentials within the existing account management workflow in under 2 minutes.
- **SC-002**: Access tokens are never displayed in full in the user interface (masked/hidden).
- **SC-003**: 100% of invalid server URLs are rejected with clear validation messages before save.
- **SC-004**: All API credential changes are persisted immediately and reflected on subsequent page loads.
- **SC-005**: Account details page loads and displays connection configuration within 2 seconds.

## Clarifications

### Session 2025-12-07

- Q: Should credential updates use dedicated commands or unified/existing flows? → A: Single unified command for credential updates (ChangeGitStorageAccountApiCredentials) plus initial credentials settable via AddGitStorageAccount.
- Q: How should the access token be displayed in the UI? → A: Partially masked showing last 4 characters (e.g., `••••••••abc1`).
- Q: Should credential changes have additional audit logging beyond domain events? → A: Standard domain events only; existing event sourcing provides sufficient audit trail.

## Assumptions

- Access tokens are stored as-is; encryption at rest is handled by the underlying infrastructure/storage layer.
- Audit logging relies on existing event sourcing; no additional explicit audit log entries are required for credential changes.
- The server URL validation focuses on format (valid HTTPS URL) rather than connectivity testing (which would be a separate feature).
- Provider type defaults to "GitHub" if not specified, as it is the most common use case.
- The existing GitStorageAccount aggregate structure and CQRS patterns will be extended rather than replaced.
- API credentials are optional fields - a Git Storage Account can exist without configured API credentials.
