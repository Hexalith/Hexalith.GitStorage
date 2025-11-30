# Hexalith.GitStorage.Aggregates

This project contains the domain aggregate implementations for the GitStorageAccount bounded context.

## Overview

Aggregates are the primary domain objects that encapsulate business rules and enforce invariants. They are the consistency boundary for transactions and the unit of persistence in event sourcing.

## Aggregate Pattern

### GitStorageAccount Aggregate

The `GitStorageAccount` aggregate is implemented as a C# record with immutable properties:

```csharp
[DataContract]
public sealed record GitStorageAccount(
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
| `GitStorageAccountAdded` | Initial creation | Initializes aggregate from event data |
| `GitStorageAccountDescriptionChanged` | Update name/comments | Returns new aggregate with updated fields |
| `GitStorageAccountDisabled` | Disable module | Sets `Disabled` to `true` |
| `GitStorageAccountEnabled` | Enable module | Sets `Disabled` to `false` |

### Apply Method

```csharp
public ApplyResult Apply([NotNull] object domainEvent)
{
    return domainEvent switch
    {
        GitStorageAccountAdded e => ApplyEvent(e),
        GitStorageAccountDescriptionChanged e => ApplyEvent(e),
        GitStorageAccountDisabled e => ApplyEvent(e),
        GitStorageAccountEnabled e => ApplyEvent(e),
        GitStorageAccountEvent => ApplyResult.NotImplemented(this),
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
public class GitStorageAccountAddedValidator : AbstractValidator<GitStorageAccountAdded>
{
    public GitStorageAccountAddedValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}
```

## Dependencies

- `Hexalith.Domains` - Base domain abstractions
- `Hexalith.GitStorage.Events` - Domain events
- `Hexalith.GitStorage.Aggregates.Abstractions` - Domain helpers

## Usage Example

```csharp
// Create new aggregate from Added event
var addedEvent = new GitStorageAccountAdded("mod-001", "My Module", "Description");
var aggregate = new GitStorageAccount();
var result = aggregate.Apply(addedEvent);

if (result.Succeeded)
{
    var newAggregate = result.Aggregate as GitStorageAccount;
    // newAggregate.Id == "mod-001"
    // newAggregate.Name == "My Module"
}

// Apply subsequent events
var changeEvent = new GitStorageAccountDescriptionChanged("mod-001", "Updated Name", "New Description");
var updateResult = newAggregate.Apply(changeEvent);
```

## Testing

Tests for this project should verify:

1. Event application produces correct state
2. Business rules are enforced
3. Invalid operations return appropriate errors
4. Validators reject invalid data
