# Hexalith.GitStorage.Commands

This project contains the command definitions for the GitStorageAccount bounded context.

## Overview

Commands represent the intent to change the system state. They are part of the CQRS (Command Query Responsibility Segregation) pattern implementation.

## Command Hierarchy

```
GitStorageAccountCommand (abstract base)
├── AddGitStorageAccount
├── ChangeGitStorageAccountDescription
├── DisableGitStorageAccount
└── EnableGitStorageAccount
```

## Commands

### GitStorageAccountCommand (Base Command)

All GitStorageAccount commands inherit from this abstract base record:

```csharp
[PolymorphicSerialization]
public abstract partial record GitStorageAccountCommand(string Id)
{
    public string AggregateId => Id;
    public static string AggregateName => GitStorageAccountDomainHelper.GitStorageAccountAggregateName;
}
```

### AddGitStorageAccount

Command to create a new module.

```csharp
[PolymorphicSerialization]
public partial record AddGitStorageAccount(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments)
    : GitStorageAccountCommand(Id);
```

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `Id` | `string` | Yes | Unique identifier for the module |
| `Name` | `string` | Yes | Display name of the module |
| `Comments` | `string?` | No | Optional description |

### ChangeGitStorageAccountDescription

Command to update the module's name and/or description.

```csharp
[PolymorphicSerialization]
public partial record ChangeGitStorageAccountDescription(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments)
    : GitStorageAccountCommand(Id);
```

### DisableGitStorageAccount

Command to disable a module.

```csharp
[PolymorphicSerialization]
public partial record DisableGitStorageAccount(string Id) : GitStorageAccountCommand(Id);
```

### EnableGitStorageAccount

Command to enable a previously disabled module.

```csharp
[PolymorphicSerialization]
public partial record EnableGitStorageAccount(string Id) : GitStorageAccountCommand(Id);
```

## Command Validators

**Location**: `GitStorageAccount/AddGitStorageAccountValidator.cs`

Commands are validated using FluentValidation:

```csharp
public class AddGitStorageAccountValidator : AbstractValidator<AddGitStorageAccount>
{
    public AddGitStorageAccountValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Module ID is required");
            
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100)
            .WithMessage("Name must be between 1 and 100 characters");
    }
}
```

## Command Handlers

Command handlers are responsible for:

1. Validating the command
2. Loading the aggregate from the event store
3. Executing business logic
4. Producing domain events
5. Persisting the events

Handlers are registered in the `Application` layer:

```csharp
services.AddGitStorageAccountCommandHandlers();
```

## Command Flow

```
┌─────────────┐    ┌────────────────┐    ┌───────────────┐
│   Command   │───▶│ Command Handler │───▶│   Aggregate   │
│  (Intent)   │    │  (Orchestrator) │    │ (Apply Logic) │
└─────────────┘    └────────────────┘    └───────────────┘
                           │                      │
                           ▼                      ▼
                    ┌────────────┐         ┌───────────┐
                    │ Validation │         │  Events   │
                    └────────────┘         └───────────┘
                                                 │
                                                 ▼
                                          ┌───────────┐
                                          │Event Store│
                                          └───────────┘
```

## Dependencies

- `Hexalith.PolymorphicSerializations` - Serialization support
- `Hexalith.GitStorage.Aggregates.Abstractions` - Domain helpers
- `FluentValidation` - Command validation

## Usage Example

```csharp
// Create a command
var command = new AddGitStorageAccount(
    Id: "mod-001",
    Name: "Sample Module",
    Comments: "This is a sample module"
);

// Submit via command service
await commandService.SubmitCommandAsync(user, command, cancellationToken);

// Or submit multiple commands
await commandService.SubmitCommandsAsync(user, [
    new AddGitStorageAccount("mod-001", "Module 1", null),
    new AddGitStorageAccount("mod-002", "Module 2", null)
], cancellationToken);
```

## Best Practices

1. **Commands are imperative** - Name them as actions (Add, Change, Disable)
2. **Include validation** - Fail fast with clear error messages
3. **Keep commands focused** - One intent per command
4. **Immutable data** - Use records for immutability
5. **Idempotency** - Design handlers to be idempotent when possible
