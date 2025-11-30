# CLAUDE.md

AI assistant guidance for **Hexalith.GitStorage** - a DDD/CQRS/Event Sourcing module template.

## Critical Rules (Always Follow)

### File Header (Required on ALL .cs files)

```csharp
// <copyright file="{FileName}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
```

### C# Style Requirements

- **Primary constructors** - Use for classes and records; NEVER duplicate properties in body
- **File-scoped namespaces** - Always use `namespace X;` not `namespace X { }`
- **XML documentation** - Required on all public/protected/internal members
- **Record params** - Document with `<param>` tags, not `<property>`

### Record Documentation Pattern

```csharp
/// <summary>
/// Represents a domain entity.
/// </summary>
/// <param name="Id">The unique identifier.</param>
/// <param name="Name">The display name.</param>
[DataContract]
public sealed record MyEntity(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name);
```

## Tech Stack

- **.NET 10** / **C# 13** (use latest features)
- **Blazor InteractiveAuto** (SSR + WebAssembly)
- **Dapr** / **.NET Aspire** / **Azure Cosmos DB**
- **xUnit + Shouldly + Moq** (testing)

## Architecture Layers

| Layer | Projects | Purpose |
|-------|----------|---------|
| **Domain** | `*.Aggregates`, `*.Events`, `*.Localizations` | Core business logic |
| **Application** | `*.Commands`, `*.Requests`, `*.Projections` | CQRS handlers |
| **Infrastructure** | `*.ApiServer`, `*.WebServer`, `*.WebApp` | Technical implementation |
| **Presentation** | `*.UI.Components`, `*.UI.Pages` | Blazor UI |

## Code Patterns

### Domain Event

```csharp
[PolymorphicSerialization]
public partial record GitStorageAccountAdded(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments)
    : GitStorageAccountEvent(Id);
```

### Command (same pattern as Event)

```csharp
[PolymorphicSerialization]
public abstract partial record GitStorageAccountCommand(string Id)
{
    public string AggregateId => Id;
    public static string AggregateName => GitStorageAccountDomainHelper.GitStorageAccountAggregateName;
}
```

### Aggregate Apply Pattern

```csharp
public ApplyResult Apply([NotNull] object domainEvent)
{
    ArgumentNullException.ThrowIfNull(domainEvent);
    return domainEvent switch
    {
        GitStorageAccountAdded e => ApplyEvent(e),
        GitStorageAccountDescriptionChanged e => ApplyEvent(e),
        GitStorageAccountEvent => ApplyResult.NotImplemented(this),
        _ => ApplyResult.InvalidEvent(this, domainEvent),
    };
}

private ApplyResult ApplyEvent(GitStorageAccountAdded e) => !(this as IDomainAggregate).IsInitialized()
    ? ApplyResult.Success(new GitStorageAccount(e), [e])
    : ApplyResult.Error(this, "The GitStorageAccount already exists.");
```

### Request with Result

```csharp
[PolymorphicSerialization]
public partial record GetGitStorageAccountDetails(
    string Id,
    [property: DataMember(Order = 2)] GitStorageAccountDetailsViewModel? Result = null)
    : GitStorageAccountRequest(Id), IRequest
{
    object? IRequest.Result => Result;
}
```

## Naming Conventions

| Type | Pattern | Example |
|------|---------|---------|
| Events | `{Entity}{PastTenseVerb}` | `GitStorageAccountAdded`, `GitStorageAccountDisabled` |
| Commands | `{Verb}{Entity}` | `AddGitStorageAccount`, `DisableGitStorageAccount` |
| Requests | `Get{Entity}{Details\|Summaries}` | `GetGitStorageAccountDetails` |
| Handlers | `{Command\|Request}Handler` | `AddGitStorageAccountHandler` |
| Private fields | `_camelCase` | `_repository` |

## Key Attributes

- `[PolymorphicSerialization]` - Required on events, commands, requests (enables JSON polymorphism)
- `[DataContract]` - On aggregates and DTOs
- `[DataMember(Order = N)]` - Property serialization order (start at 1 for Id, increment)

## Entity Creation Order

1. **Events** → `src/libraries/Domain/Hexalith.GitStorage.Events/{Entity}/`
2. **Aggregate** → `src/libraries/Domain/Hexalith.GitStorage.Aggregates/`
3. **Commands** → `src/libraries/Application/Hexalith.GitStorage.Commands/{Entity}/`
4. **Requests** → `src/libraries/Application/Hexalith.GitStorage.Requests/{Entity}/`
5. **Projections** → `src/libraries/Application/Hexalith.GitStorage.Projections/`
6. **UI Components** → `src/libraries/Presentation/Hexalith.GitStorage.UI.Components/`
7. **UI Pages** → `src/libraries/Presentation/Hexalith.GitStorage.UI.Pages/`

## Testing

```csharp
[Fact]
public void Apply_GitStorageAccountAdded_ShouldInitializeAggregate()
{
    // Arrange
    GitStorageAccount aggregate = new();
    GitStorageAccountAdded added = new("test-id", "Test Name", null);

    // Act
    ApplyResult result = aggregate.Apply(added);

    // Assert
    result.Failed.ShouldBeFalse();
    result.Aggregate.ShouldBeOfType<GitStorageAccount>()
        .Id.ShouldBe("test-id");
}
```

Test naming: `{Method}_{Scenario}_{ExpectedResult}`

## Commands

```bash
dotnet build                    # Build solution
dotnet test                     # Run tests
cd AspireHost && dotnet run     # Start with Aspire orchestration
./initialize.ps1 -PackageName "YourModule"  # Initialize from template
```

## Do Not Modify

- `DOCUMENTATION.ai.md` - AI prompts
- `Hexalith.Builds/` - Build configs (submodule)
- `HexalithApp/` - Base framework (submodule)

## Hexalith Package Version

Current: **1.71.1** (defined in `Directory.Packages.props`)
