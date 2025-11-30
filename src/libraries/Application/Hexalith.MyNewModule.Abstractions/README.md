# Hexalith.MyNewModule.Abstractions

This project contains the interfaces, contracts, and shared definitions for the MyToDo application layer.

## Overview

The abstractions project defines the contracts that other projects depend on, enabling:
- Loose coupling between components
- Easy mocking for unit tests
- Clear API boundaries

## Contents

### Module Information

**HexalithMyNewModuleInformation.cs**

Static class providing module identification:

```csharp
public static class HexalithMyNewModuleInformation
{
    public static string Id => "Hexalith.MyNewModule";
    public static string Name => "Hexalith MyNewModule";
    public static string ShortName => "MyNewModule";
}
```

### Module Interface

**IMyToDoModule.cs**

Interface marking a module as a MyToDo module:

```csharp
public interface IMyToDoModule : IApplicationModule;
```

This interface is implemented by:
- `HexalithMyNewModuleApiServerModule`
- `HexalithMyNewModuleWebAppModule`
- `HexalithMyNewModuleWebServerModule`

### Service Interface

**IMyToDoService.cs**

Service contract for MyToDo operations:

```csharp
public interface IMyToDoService
{
    Task DoSomethingAsync(CancellationToken cancellationToken);
}
```

Extend this interface as needed for your module's specific services.

### Security

**MyToDoRoles.cs**

Defines security roles:

```csharp
public static class MyToDoRoles
{
    public const string Owner = nameof(MyNewModule) + nameof(Owner);
    public const string Contributor = nameof(MyNewModule) + nameof(Contributor);
    public const string Reader = nameof(MyNewModule) + nameof(Reader);
}
```

| Role | Constant Value | Description |
|------|---------------|-------------|
| Owner | `MyNewModuleOwner` | Full administrative access |
| Contributor | `MyNewModuleContributor` | Create, read, update access |
| Reader | `MyNewModuleReader` | Read-only access |

**MyToDoPolicies.cs**

Defines authorization policies:

```csharp
public static class MyToDoPolicies
{
    public const string Owner = MyToDoRoles.Owner;
    public const string Contributor = MyToDoRoles.Contributor;
    public const string Reader = MyToDoRoles.Reader;
}
```

## Dependencies

This project has minimal dependencies:
- `Hexalith.Application.Modules.Abstractions` - Module interfaces

## Usage

### Referencing in Other Projects

```xml
<ItemGroup>
  <ProjectReference Include="..\Hexalith.MyNewModule.Abstractions" />
</ItemGroup>
```

### Using in Code

```csharp
using Hexalith.MyNewModule;

// Get module information
var moduleId = HexalithMyNewModuleInformation.Id;
var moduleName = HexalithMyNewModuleInformation.Name;

// Check user roles
if (user.IsInRole(MyToDoRoles.Owner))
{
    // Allow administrative operations
}

// Apply authorization policy
[Authorize(Policy = MyToDoPolicies.Contributor)]
public class MyController : ControllerBase
{
    // Controller actions
}
```

### Implementing the Service Interface

```csharp
public class MyToDoService : IMyToDoService
{
    public async Task DoSomethingAsync(CancellationToken cancellationToken)
    {
        // Implementation
    }
}

// Register in DI
services.AddScoped<IMyToDoService, MyToDoService>();
```

## Design Guidelines

1. **Keep abstractions minimal** - Only include what's needed
2. **Avoid implementation details** - No concrete classes
3. **Use interfaces for services** - Enable dependency injection
4. **Constants for magic strings** - Centralize role/policy names
5. **No external dependencies** - Keep the project lightweight
