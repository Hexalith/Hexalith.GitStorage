# Quickstart: GitOrganization Implementation

**Feature**: 002-git-organization
**Date**: 2025-12-07

## Overview

This guide provides step-by-step implementation instructions for the GitOrganization entity following Hexalith DDD/CQRS/Event Sourcing patterns.

## Prerequisites

- Feature 001 (GitStorageAccount) implemented
- Understanding of Hexalith framework patterns
- .NET 10 SDK installed

## Implementation Order

Follow this sequence to maintain proper dependency flow:

1. **Enums** (no dependencies)
2. **Domain Helper** (no dependencies)
3. **Events** (depends on enums, helper)
4. **Aggregate** (depends on events)
5. **Commands** (depends on helper)
6. **Requests & ViewModels** (depends on helper, enums)
7. **Projections** (depends on events, viewmodels)
8. **Command Handlers** (depends on commands, events)
9. **Event Handlers** (depends on events, adapters)
10. **UI Pages** (depends on all above)
11. **Tests** (parallel with implementation)

## Step 1: Enums

### File: `Hexalith.GitStorage.Aggregates.Abstractions/Enums/GitOrganizationOrigin.cs`

```csharp
// <copyright file="GitOrganizationOrigin.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Domain;

/// <summary>
/// Indicates how a Git Organization was added to the system.
/// </summary>
public enum GitOrganizationOrigin
{
    /// <summary>
    /// The organization was discovered from the remote Git server during synchronization.
    /// </summary>
    Synced = 0,

    /// <summary>
    /// The organization was created through the application API.
    /// </summary>
    CreatedViaApplication = 1,
}
```

### File: `Hexalith.GitStorage.Aggregates.Abstractions/Enums/GitOrganizationVisibility.cs`

```csharp
// <copyright file="GitOrganizationVisibility.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Aggregates.Enums;

/// <summary>
/// Specifies the visibility level of a Git Organization.
/// </summary>
public enum GitOrganizationVisibility
{
    /// <summary>
    /// Organization is visible to everyone.
    /// </summary>
    Public = 0,

    /// <summary>
    /// Organization is only visible to members.
    /// </summary>
    Private = 1,

    /// <summary>
    /// Organization is visible to all authenticated users within the enterprise.
    /// Maps to GitHub 'internal' and Forgejo 'limited'.
    /// </summary>
    Internal = 2,
}
```

### File: `Hexalith.GitStorage.Aggregates.Abstractions/Enums/GitOrganizationSyncStatus.cs`

```csharp
// <copyright file="GitOrganizationSyncStatus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Domain;

/// <summary>
/// Represents the synchronization status of a Git Organization with the remote server.
/// </summary>
public enum GitOrganizationSyncStatus
{
    /// <summary>
    /// The organization is successfully synchronized with the remote server.
    /// </summary>
    Synced = 0,

    /// <summary>
    /// The organization was previously synced but is no longer found on the remote server.
    /// </summary>
    NotFoundOnRemote = 1,

    /// <summary>
    /// The last synchronization operation failed with an error.
    /// </summary>
    SyncError = 2,
}
```

## Step 2: Domain Helper

### File: `Hexalith.GitStorage.Aggregates.Abstractions/GitOrganizationDomainHelper.cs`

```csharp
// <copyright file="GitOrganizationDomainHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Domain;

/// <summary>
/// Helper class for Git Organization domain operations.
/// </summary>
public static class GitOrganizationDomainHelper
{
    /// <summary>
    /// The aggregate name for Git Organization entities.
    /// </summary>
    public const string GitOrganizationAggregateName = "GitOrganization";

    /// <summary>
    /// Generates a deterministic ID for a Git Organization.
    /// </summary>
    /// <param name="gitStorageAccountId">The parent Git Storage Account ID.</param>
    /// <param name="organizationName">The organization name.</param>
    /// <returns>A composite key in format {gitStorageAccountId}-{organizationName}.</returns>
    public static string GenerateId(string gitStorageAccountId, string organizationName)
        => $"{gitStorageAccountId}-{organizationName.ToLowerInvariant()}";
}
```

## Step 3: Base Event

### File: `Hexalith.GitStorage.Events/GitOrganization/GitOrganizationEvent.cs`

```csharp
// <copyright file="GitOrganizationEvent.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Events.GitOrganization;

using Hexalith.GitStorage.Domain;
using Hexalith.PolymorphicSerialization;

/// <summary>
/// Base class for all Git Organization domain events.
/// </summary>
/// <param name="Id">The unique identifier of the Git Organization.</param>
[PolymorphicSerialization]
public abstract partial record GitOrganizationEvent(string Id)
{
    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    public string AggregateId => Id;

    /// <summary>
    /// Gets the aggregate name.
    /// </summary>
    public static string AggregateName => GitOrganizationDomainHelper.GitOrganizationAggregateName;
}
```

## Step 4: Concrete Events

### File: `Hexalith.GitStorage.Events/GitOrganization/GitOrganizationAdded.cs`

```csharp
// <copyright file="GitOrganizationAdded.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Events.GitOrganization;

using System.Runtime.Serialization;

using Hexalith.PolymorphicSerialization;

/// <summary>
/// Event raised when a Git Organization is created via the application API.
/// </summary>
/// <param name="Id">The unique identifier of the Git Organization.</param>
/// <param name="Name">The name of the organization as it appears on the Git server.</param>
/// <param name="Description">Optional description of the organization.</param>
/// <param name="Visibility">The visibility level of the organization.</param>
/// <param name="GitStorageAccountId">The ID of the parent Git Storage Account.</param>
[PolymorphicSerialization]
public partial record GitOrganizationAdded(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Description,
    [property: DataMember(Order = 4)] GitOrganizationVisibility Visibility,
    [property: DataMember(Order = 5)] string GitStorageAccountId)
    : GitOrganizationEvent(Id);
```

## Step 5: Aggregate

### File: `Hexalith.GitStorage.Aggregates/GitOrganization.cs`

```csharp
// <copyright file="GitOrganization.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Aggregates;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using Hexalith.Domain.Aggregates;
using Hexalith.GitStorage.Domain;
using Hexalith.GitStorage.Events.GitOrganization;

/// <summary>
/// Represents a Git Organization aggregate.
/// </summary>
[DataContract]
public sealed record GitOrganization(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Description,
    [property: DataMember(Order = 4)] GitOrganizationVisibility Visibility,
    [property: DataMember(Order = 5)] string GitStorageAccountId,
    [property: DataMember(Order = 6)] GitOrganizationOrigin Origin,
    [property: DataMember(Order = 7)] string? RemoteId,
    [property: DataMember(Order = 8)] GitOrganizationSyncStatus SyncStatus,
    [property: DataMember(Order = 9)] DateTimeOffset? LastSyncedAt,
    [property: DataMember(Order = 10)] bool Disabled) : IDomainAggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GitOrganization"/> class.
    /// </summary>
    public GitOrganization()
        : this(
            string.Empty,
            string.Empty,
            null,
            string.Empty,
            GitOrganizationOrigin.Synced,
            null,
            GitOrganizationSyncStatus.Synced,
            null,
            false)
    {
    }

    /// <inheritdoc/>
    public string AggregateId => Id;

    /// <inheritdoc/>
    public string AggregateName => GitOrganizationDomainHelper.GitOrganizationAggregateName;

    /// <inheritdoc/>
    public ApplyResult Apply([NotNull] object domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);

        // Disabled check: only allow enable/disable events when disabled
        if (domainEvent is GitOrganizationEvent &&
            domainEvent is not GitOrganizationEnabled and not GitOrganizationDisabled &&
            Disabled)
        {
            return ApplyResult.NotEnabled(this);
        }

        // Initialization check
        if (!(this as IDomainAggregate).IsInitialized() &&
            domainEvent is not GitOrganizationAdded and not GitOrganizationSynced)
        {
            return ApplyResult.NotInitialized(this);
        }

        return domainEvent switch
        {
            GitOrganizationAdded e => ApplyEvent(e),
            GitOrganizationSynced e => ApplyEvent(e),
            GitOrganizationDescriptionChanged e => ApplyEvent(e),
            GitOrganizationMarkedNotFound e => ApplyEvent(e),
            GitOrganizationDisabled e => ApplyEvent(e),
            GitOrganizationEnabled e => ApplyEvent(e),
            GitOrganizationEvent => ApplyResult.NotImplemented(this),
            _ => ApplyResult.InvalidEvent(this, domainEvent),
        };
    }

    private ApplyResult ApplyEvent(GitOrganizationAdded e) =>
        !(this as IDomainAggregate).IsInitialized()
            ? ApplyResult.Success(
                new GitOrganization(
                    e.Id,
                    e.Name,
                    e.Description,
                    e.GitStorageAccountId,
                    GitOrganizationOrigin.CreatedViaApplication,
                    null,
                    GitOrganizationSyncStatus.Synced,
                    null,
                    false),
                [e])
            : ApplyResult.Error(this, "The GitOrganization already exists.");

    private ApplyResult ApplyEvent(GitOrganizationDisabled e) =>
        Disabled
            ? ApplyResult.Error(this, "The GitOrganization is already disabled.")
            : ApplyResult.Success(this with { Disabled = true }, [e]);

    private ApplyResult ApplyEvent(GitOrganizationEnabled e) =>
        !Disabled
            ? ApplyResult.Error(this, "The GitOrganization is already enabled.")
            : ApplyResult.Success(this with { Disabled = false }, [e]);

    // ... additional ApplyEvent methods
}
```

## Step 6: Commands

### File: `Hexalith.GitStorage.Commands/GitOrganization/AddGitOrganization.cs`

```csharp
// <copyright file="AddGitOrganization.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Commands.GitOrganization;

using System.Runtime.Serialization;

using Hexalith.PolymorphicSerialization;

/// <summary>
/// Command to create a new Git Organization via the application API.
/// </summary>
/// <param name="Id">The unique identifier for the new Git Organization.</param>
/// <param name="Name">The name of the organization.</param>
/// <param name="Description">Optional description.</param>
/// <param name="Visibility">The visibility level of the organization.</param>
/// <param name="GitStorageAccountId">The parent Git Storage Account ID.</param>
[PolymorphicSerialization]
public partial record AddGitOrganization(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Description,
    [property: DataMember(Order = 4)] GitOrganizationVisibility Visibility,
    [property: DataMember(Order = 5)] string GitStorageAccountId)
    : GitOrganizationCommand(Id);
```

## Step 7: Request & ViewModel

### File: `Hexalith.GitStorage.Requests/GitOrganization/GetGitOrganizationDetails.cs`

```csharp
// <copyright file="GetGitOrganizationDetails.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Requests.GitOrganization;

using System.Runtime.Serialization;

using Hexalith.Application.Requests;
using Hexalith.PolymorphicSerialization;

/// <summary>
/// Request to get Git Organization details by ID.
/// </summary>
/// <param name="Id">The unique identifier of the Git Organization.</param>
/// <param name="Result">The result containing organization details.</param>
[PolymorphicSerialization]
public partial record GetGitOrganizationDetails(
    string Id,
    [property: DataMember(Order = 2)] GitOrganizationDetailsViewModel? Result = null)
    : GitOrganizationRequest(Id), IRequest
{
    /// <inheritdoc/>
    object? IRequest.Result => Result;
}
```

## Key Patterns Reference

### Event Application Pattern

```csharp
private ApplyResult ApplyEvent(SomeEvent e) =>
    condition
        ? ApplyResult.Success(this with { Property = e.Value }, [e])
        : ApplyResult.Error(this, "Error message");
```

### Command Handler Registration

```csharp
services.TryAddSimpleInitializationCommandHandler<AddGitOrganization>(
    c => new GitOrganizationAdded(c.Id, c.Name, c.Description, c.Visibility, c.GitStorageAccountId),
    ev => new GitOrganization((GitOrganizationAdded)ev));
```

### Projection Handler Pattern

```csharp
protected override Task<GitOrganizationDetailsViewModel?> ApplyEventAsync(
    GitOrganizationAdded baseEvent,
    GitOrganizationDetailsViewModel? model,
    CancellationToken cancellationToken)
{
    return Task.FromResult<GitOrganizationDetailsViewModel?>(
        new GitOrganizationDetailsViewModel(
            baseEvent.Id,
            baseEvent.Name,
            // ... map all fields
        ));
}
```

## Testing Checklist

- [ ] Aggregate initialization tests
- [ ] Event serialization round-trip tests
- [ ] Command validation tests
- [ ] Projection handler tests
- [ ] Integration tests for sync workflow

## Common Pitfalls

1. **Forgetting `[PolymorphicSerialization]`** - Events won't deserialize correctly
2. **Wrong `DataMember(Order = N)`** - Serialization order matters
3. **Missing parameterless constructor** - Required for deserialization
4. **Forgetting disabled state check** - Events should be blocked when disabled
5. **Not using `with` for immutable updates** - Records must remain immutable
