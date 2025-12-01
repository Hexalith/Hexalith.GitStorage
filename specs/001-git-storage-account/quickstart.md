# Quickstart: Git Storage Account Entity

**Feature Branch**: `001-git-storage-account`
**Date**: 2025-12-01

## Prerequisites

- .NET 10 SDK installed
- Docker (for Dapr/Aspire local development)
- Visual Studio 2022 or VS Code with C# extension

## Building the Solution

```bash
# Navigate to repository root
cd d:\Hexalith.GitStorage

# Restore and build
dotnet build

# Run tests
dotnet test
```

## Running with Aspire

```bash
# Start the application with Aspire orchestration
cd AspireHost
dotnet run
```

The Aspire dashboard will be available at the displayed URL (typically `https://localhost:15000`).

## API Usage Examples

### Authentication

All endpoints require Admin role authentication. Include a JWT bearer token with Admin claims:

```http
Authorization: Bearer <your-jwt-token>
```

### Create a Git Storage Account

```bash
curl -X POST https://localhost:5001/api/GitStorageAccount \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "$type": "AddGitStorageAccount",
    "id": "github-main",
    "name": "GitHub Main Repository",
    "comments": "Primary repository for production deployments"
  }'
```

**Expected Response**: `202 Accepted`

### Get Account Details

```bash
curl -X GET https://localhost:5001/api/GitStorageAccount/github-main \
  -H "Authorization: Bearer <token>"
```

**Expected Response**:
```json
{
  "id": "github-main",
  "name": "GitHub Main Repository",
  "comments": "Primary repository for production deployments",
  "disabled": false
}
```

### List All Accounts (Paginated)

```bash
curl -X GET "https://localhost:5001/api/GitStorageAccount?skip=0&take=20" \
  -H "Authorization: Bearer <token>"
```

**Expected Response**:
```json
[
  {
    "id": "github-main",
    "name": "GitHub Main Repository",
    "disabled": false
  }
]
```

### Update Account Description

```bash
curl -X POST https://localhost:5001/api/GitStorageAccount \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "$type": "ChangeGitStorageAccountDescription",
    "id": "github-main",
    "name": "GitHub Primary Repository",
    "comments": "Updated: Now includes staging branches"
  }'
```

### Disable an Account

```bash
curl -X POST https://localhost:5001/api/GitStorageAccount \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "$type": "DisableGitStorageAccount",
    "id": "github-main"
  }'
```

### Re-enable an Account

```bash
curl -X POST https://localhost:5001/api/GitStorageAccount \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token>" \
  -d '{
    "$type": "EnableGitStorageAccount",
    "id": "github-main"
  }'
```

## Code Examples

### Creating an Account Programmatically

```csharp
// Using the command pattern
var command = new AddGitStorageAccount(
    Id: "github-main",
    Name: "GitHub Main Repository",
    Comments: "Primary repository for production");

// Send via command bus (Dapr pub/sub)
await commandBus.SendAsync(command);
```

### Querying Account Details

```csharp
// Using the request pattern
var request = new GetGitStorageAccountDetails("github-main");
var result = await requestHandler.HandleAsync(request);

if (result.Result is GitStorageAccountDetailsViewModel details)
{
    Console.WriteLine($"Account: {details.Name}, Disabled: {details.Disabled}");
}
```

### Working with the Aggregate Directly (Tests)

```csharp
// Create new aggregate from event
var aggregate = new GitStorageAccount();
var addedEvent = new GitStorageAccountAdded("test-id", "Test Account", "Test comments");

var result = aggregate.Apply(addedEvent);

// Verify state
result.Failed.ShouldBeFalse();
var newAggregate = result.Aggregate.ShouldBeOfType<GitStorageAccount>();
newAggregate.Id.ShouldBe("test-id");
newAggregate.Name.ShouldBe("Test Account");
newAggregate.Disabled.ShouldBeFalse();
```

## Testing

### Unit Tests

```bash
# Run all tests
dotnet test

# Run specific test class
dotnet test --filter "FullyQualifiedName~GitStorageAccountTests"

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### Test Structure

```
test/Hexalith.GitStorage.Tests/
├── Domains/
│   ├── Aggregates/
│   │   └── GitStorageAccountTests.cs      # Aggregate Apply tests
│   ├── Commands/
│   │   └── GitStorageAccountCommandTests.cs # Command serialization tests
│   └── Events/
│       └── GitStorageAccountEventTests.cs   # Event serialization tests
```

## Key Files to Review

| Purpose | File Location |
|---------|---------------|
| Aggregate | `src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitStorageAccount.cs` |
| Events | `src/libraries/Domain/Hexalith.GitStorage.Events/GitStorageAccount/` |
| Commands | `src/libraries/Application/Hexalith.GitStorage.Commands/GitStorageAccount/` |
| Requests | `src/libraries/Application/Hexalith.GitStorage.Requests/GitStorageAccount/` |
| Projections | `src/libraries/Application/Hexalith.GitStorage.Projections/ProjectionHandlers/Details/` |
| API Controller | `src/libraries/Infrastructure/Hexalith.GitStorage.ApiServer/Controllers/` |

## Common Issues

### Validation Errors

If you receive a `400 Bad Request`, check:
- `id` and `name` fields are provided and non-empty
- The `$type` discriminator is spelled correctly
- JSON property names match the schema (camelCase)

### Authorization Errors

`401 Unauthorized`: No valid JWT token provided
`403 Forbidden`: User lacks Admin role claim

### Conflict Errors

`409 Conflict`:
- Attempting to create an account with an existing ID
- Version mismatch due to concurrent updates

## Next Steps

1. Verify the account entity is created successfully
2. Test all CRUD operations via the API
3. Verify event sourcing by checking the event store
4. Test the projection handlers by querying read models
5. Integrate with UI components (Blazor pages)
