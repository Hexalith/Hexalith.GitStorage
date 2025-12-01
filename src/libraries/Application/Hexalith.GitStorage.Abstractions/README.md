# Hexalith.GitStorage.Abstractions

This project contains the interfaces, contracts, and shared definitions for the GitStorage application layer. It provides the foundation for managing Git storage providers (GitHub and Forgejo), organizations, and repositories.

## Overview

The abstractions project defines the contracts that other projects depend on, enabling:
- Loose coupling between components
- Easy mocking for unit tests
- Clear API boundaries

## Contents

### Module Information

**HexalithGitStorageInformation.cs**

Static class providing module identification:

```csharp
public static class HexalithGitStorageInformation
{
    public static string Id => "Hexalith.GitStorage";
    public static string Name => "Hexalith GitStorage";
    public static string ShortName => "GitStorage";
}
```

### Module Interface

**IGitStorageAccountModule.cs**

Interface marking a module as a GitStorageAccount module:

```csharp
public interface IGitStorageAccountModule : IApplicationModule;
```

This interface is implemented by:
- `HexalithGitStorageApiServerModule`
- `HexalithGitStorageWebAppModule`
- `HexalithGitStorageWebServerModule`

### Service Interfaces

**IGitStorageAccountService.cs**

Service contract for Git storage account operations:

```csharp
public interface IGitStorageAccountService
{
    Task<IEnumerable<Organization>> GetOrganizationsAsync(string accountId, CancellationToken cancellationToken);
    Task<IEnumerable<Repository>> GetRepositoriesAsync(string accountId, string organizationName, CancellationToken cancellationToken);
    Task CreateRepositoryAsync(string accountId, string organizationName, CreateRepositoryRequest request, CancellationToken cancellationToken);
}
```

**IGitProviderAdapter.cs**

Provider-specific adapter interface for GitHub and Forgejo:

```csharp
public interface IGitProviderAdapter
{
    GitProviderType ProviderType { get; }
    Task<bool> TestConnectionAsync(string baseUrl, string token, CancellationToken cancellationToken);
    Task<IEnumerable<Organization>> GetOrganizationsAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Repository>> GetRepositoriesAsync(string organizationName, CancellationToken cancellationToken);
}
```

### Security

**GitStorageAccountRoles.cs**

Defines security roles:

```csharp
public static class GitStorageAccountRoles
{
    public const string Owner = nameof(GitStorage) + nameof(Owner);
    public const string Contributor = nameof(GitStorage) + nameof(Contributor);
    public const string Reader = nameof(GitStorage) + nameof(Reader);
}
```

| Role | Constant Value | Description |
|------|---------------|-------------|
| Owner | `GitStorageOwner` | Full administrative access |
| Contributor | `GitStorageContributor` | Create, read, update access |
| Reader | `GitStorageReader` | Read-only access |

**GitStorageAccountPolicies.cs**

Defines authorization policies:

```csharp
public static class GitStorageAccountPolicies
{
    public const string Owner = GitStorageAccountRoles.Owner;
    public const string Contributor = GitStorageAccountRoles.Contributor;
    public const string Reader = GitStorageAccountRoles.Reader;
}
```

## Dependencies

This project has minimal dependencies:
- `Hexalith.Application.Modules.Abstractions` - Module interfaces

## Usage

### Referencing in Other Projects

```xml
<ItemGroup>
  <ProjectReference Include="..\Hexalith.GitStorage.Abstractions" />
</ItemGroup>
```

### Using in Code

```csharp
using Hexalith.GitStorage;

// Get module information
var moduleId = HexalithGitStorageInformation.Id;
var moduleName = HexalithGitStorageInformation.Name;

// Check user roles
if (user.IsInRole(GitStorageAccountRoles.Owner))
{
    // Allow administrative operations
}

// Apply authorization policy
[Authorize(Policy = GitStorageAccountPolicies.Contributor)]
public class MyController : ControllerBase
{
    // Controller actions
}
```

### Implementing the Service Interface

```csharp
public class GitStorageAccountService : IGitStorageAccountService
{
    public async Task DoSomethingAsync(CancellationToken cancellationToken)
    {
        // Implementation
    }
}

// Register in DI
services.AddScoped<IGitStorageAccountService, GitStorageAccountService>();
```

## Design Guidelines

1. **Keep abstractions minimal** - Only include what's needed
2. **Avoid implementation details** - No concrete classes
3. **Use interfaces for services** - Enable dependency injection
4. **Constants for magic strings** - Centralize role/policy names
5. **No external dependencies** - Keep the project lightweight
