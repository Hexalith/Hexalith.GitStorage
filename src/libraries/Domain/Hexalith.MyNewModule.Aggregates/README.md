# Hexalith.MyNewModule.Aggregates

This project contains the domain aggregate implementations for the MyToDo bounded context.

## Overview

Aggregates are the primary domain objects that encapsulate business rules and enforce invariants. They are the consistency boundary for transactions and the unit of persistence in event sourcing.

## Aggregate Pattern

### MyToDo Aggregate

The `MyToDo` aggregate is implemented as a C# record with immutable properties:

```csharp
[DataContract]
public sealed record MyToDo(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments,
    [property: DataMember(Order = 7)] bool Disabled) : IDomainAggregate
{
    // Implementation
}
```

### Key Features

| Feature | Description |
|---------|-------------|
| **Immutability** | Uses C# records for immutable state |
| **Event Sourcing** | State changes are captured as events |
| **Apply Method** | Handles domain events to produce new state |
| **Validation** | Business rules are enforced in event handlers |

## Event Handling

The aggregate handles the following events:

| Event | Description | Behavior |
|-------|-------------|----------|
| `MyToDoAdded` | Initial creation | Initializes aggregate from event data |
| `MyToDoDescriptionChanged` | Update name/comments | Returns new aggregate with updated fields |
| `MyToDoDisabled` | Disable module | Sets `Disabled` to `true` |
| `MyToDoEnabled` | Enable module | Sets `Disabled` to `false` |

### Apply Method

```csharp
public ApplyResult Apply([NotNull] object domainEvent)
{
    return domainEvent switch
    {
        MyToDoAdded e => ApplyEvent(e),
        MyToDoDescriptionChanged e => ApplyEvent(e),
        MyToDoDisabled e => ApplyEvent(e),
        MyToDoEnabled e => ApplyEvent(e),
        MyToDoEvent => ApplyResult.NotImplemented(this),
        _ => ApplyResult.InvalidEvent(this, domainEvent),
    };
}
```

### Business Rules

1. **Cannot apply events to disabled aggregate** (except Enable/Disable events)
2. **Cannot apply events to uninitialized aggregate** (except Added event)
3. **Cannot add an already existing aggregate**
4. **Cannot disable an already disabled aggregate**
5. **Cannot enable an already enabled aggregate**

## Validators

**Location**: `Validators/`

Validators ensure that events and commands meet business requirements before being applied.

```csharp
public class MyToDoAddedValidator : AbstractValidator<MyToDoAdded>
{
    public MyToDoAddedValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
```

## Dependencies

- `Hexalith.Domains` - Base domain abstractions
- `Hexalith.MyNewModule.Events` - Domain events
- `Hexalith.MyNewModule.Aggregates.Abstractions` - Domain helpers

## Usage Example

```csharp
// Create new aggregate from Added event
var addedEvent = new MyToDoAdded("mod-001", "My Module", "Description");
var aggregate = new MyToDo();
var result = aggregate.Apply(addedEvent);

if (result.Succeeded)
{
    var newAggregate = result.Aggregate as MyToDo;
    // newAggregate.Id == "mod-001"
    // newAggregate.Name == "My Module"
}

// Apply subsequent events
var changeEvent = new MyToDoDescriptionChanged("mod-001", "Updated Name", "New Description");
var updateResult = newAggregate.Apply(changeEvent);
```

## Testing

Tests for this project should verify:

1. Event application produces correct state
2. Business rules are enforced
3. Invalid operations return appropriate errors
4. Validators reject invalid data
