# Hexalith.MyNewModule.Commands

This project contains the command definitions for the MyToDo bounded context.

## Overview

Commands represent the intent to change the system state. They are part of the CQRS (Command Query Responsibility Segregation) pattern implementation.

## Command Hierarchy

```
MyToDoCommand (abstract base)
├── AddMyToDo
├── ChangeMyToDoDescription
├── DisableMyToDo
└── EnableMyToDo
```

## Commands

### MyToDoCommand (Base Command)

All MyToDo commands inherit from this abstract base record:

```csharp
[PolymorphicSerialization]
public abstract partial record MyToDoCommand(string Id)
{
    public string AggregateId => Id;
    public static string AggregateName => MyToDoDomainHelper.MyToDoAggregateName;
}
```

### AddMyToDo

Command to create a new module.

```csharp
[PolymorphicSerialization]
public partial record AddMyToDo(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments)
    : MyToDoCommand(Id);
```

| Property | Type | Required | Description |
|----------|------|----------|-------------|
| `Id` | `string` | Yes | Unique identifier for the module |
| `Name` | `string` | Yes | Display name of the module |
| `Comments` | `string?` | No | Optional description |

### ChangeMyToDoDescription

Command to update the module's name and/or description.

```csharp
[PolymorphicSerialization]
public partial record ChangeMyToDoDescription(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments)
    : MyToDoCommand(Id);
```

### DisableMyToDo

Command to disable a module.

```csharp
[PolymorphicSerialization]
public partial record DisableMyToDo(string Id) : MyToDoCommand(Id);
```

### EnableMyToDo

Command to enable a previously disabled module.

```csharp
[PolymorphicSerialization]
public partial record EnableMyToDo(string Id) : MyToDoCommand(Id);
```

## Command Validators

**Location**: `MyToDo/AddMyToDoValidator.cs`

Commands are validated using FluentValidation:

```csharp
public class AddMyToDoValidator : AbstractValidator<AddMyToDo>
{
    public AddMyToDoValidator()
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
services.AddMyToDoCommandHandlers();
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
- `Hexalith.MyNewModule.Aggregates.Abstractions` - Domain helpers
- `FluentValidation` - Command validation

## Usage Example

```csharp
// Create a command
var command = new AddMyToDo(
    Id: "mod-001",
    Name: "Sample Module",
    Comments: "This is a sample module"
);

// Submit via command service
await commandService.SubmitCommandAsync(user, command, cancellationToken);

// Or submit multiple commands
await commandService.SubmitCommandsAsync(user, [
    new AddMyToDo("mod-001", "Module 1", null),
    new AddMyToDo("mod-002", "Module 2", null)
], cancellationToken);
```

## Best Practices

1. **Commands are imperative** - Name them as actions (Add, Change, Disable)
2. **Include validation** - Fail fast with clear error messages
3. **Keep commands focused** - One intent per command
4. **Immutable data** - Use records for immutability
5. **Idempotency** - Design handlers to be idempotent when possible
