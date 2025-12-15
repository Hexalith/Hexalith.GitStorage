# Quickstart: GitRepository Implementation

**Feature**: 004-git-repository
**Date**: 2025-12-15

This guide provides quick-reference patterns for implementing the GitRepository aggregate. All examples follow established patterns from GitOrganization and GitStorageAccount.

## 1. Create Domain Events

### Base Event Class

```csharp
// src/libraries/Domain/Hexalith.GitStorage.Events/GitRepository/GitRepositoryEvent.cs
namespace Hexalith.GitStorage.Events.GitRepository;

using System.Runtime.Serialization;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Base class for all GitRepository domain events.
/// </summary>
/// <param name="Id">The repository identifier.</param>
[PolymorphicSerialization]
public abstract partial record GitRepositoryEvent([property: DataMember(Order = 1)] string Id)
    : IPolymorphicSerializable
{
    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    public string AggregateId => Id;

    /// <summary>
    /// Gets the aggregate name.
    /// </summary>
    public static string AggregateName => "GitRepository";
}
```

### Event Implementation

```csharp
// src/libraries/Domain/Hexalith.GitStorage.Events/GitRepository/GitRepositoryAdded.cs
namespace Hexalith.GitStorage.Events.GitRepository;

using System.Runtime.Serialization;

using Hexalith.GitStorage.Aggregates.Abstractions.Enums;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Event raised when a new Git repository is created.
/// </summary>
[PolymorphicSerialization]
public sealed record GitRepositoryAdded(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Description,
    [property: DataMember(Order = 4)] string OrganizationId,
    [property: DataMember(Order = 5)] GitRepositoryVisibility Visibility,
    [property: DataMember(Order = 6)] string? DefaultBranch,
    [property: DataMember(Order = 7)] string? RemoteId,
    [property: DataMember(Order = 8)] string? Url)
    : GitRepositoryEvent(Id);
```

## 2. Create Aggregate

```csharp
// src/libraries/Domain/Hexalith.GitStorage.Aggregates/GitRepository.cs
namespace Hexalith.GitStorage.Aggregates;

using Hexalith.Domain.Aggregates;
using Hexalith.GitStorage.Aggregates.Abstractions.Enums;
using Hexalith.GitStorage.Events.GitRepository;

/// <summary>
/// Git repository aggregate root.
/// </summary>
public sealed record GitRepository : IDomainAggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GitRepository"/> class.
    /// </summary>
    public GitRepository() => Id = string.Empty;

    /// <summary>
    /// Initializes a new instance from GitRepositoryAdded event.
    /// </summary>
    public GitRepository(GitRepositoryAdded added)
    {
        ArgumentNullException.ThrowIfNull(added);
        Id = added.Id;
        Name = added.Name;
        Description = added.Description;
        OrganizationId = added.OrganizationId;
        Visibility = added.Visibility;
        DefaultBranch = added.DefaultBranch ?? "main";
        RemoteId = added.RemoteId;
        Url = added.Url;
        Origin = GitRepositoryOrigin.CreatedViaApplication;
        SyncStatus = GitRepositorySyncStatus.Synced;
    }

    /// <inheritdoc/>
    public string AggregateId => Id;

    /// <inheritdoc/>
    public static string AggregateName => "GitRepository";

    public string Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public string OrganizationId { get; init; } = string.Empty;
    public GitRepositoryVisibility Visibility { get; init; }
    public string DefaultBranch { get; init; } = "main";
    public string? RemoteId { get; init; }
    public string? Url { get; init; }
    public GitRepositoryOrigin Origin { get; init; }
    public GitRepositorySyncStatus SyncStatus { get; init; }
    public DateTimeOffset? LastSyncedAt { get; init; }
    public bool Disabled { get; init; }

    /// <inheritdoc/>
    public bool IsInitialized() => !string.IsNullOrWhiteSpace(Id);

    /// <inheritdoc/>
    public ApplyResult Apply(object @event)
    {
        return @event switch
        {
            GitRepositoryAdded e => ApplyEvent(e),
            GitRepositorySynced e => ApplyEvent(e),
            GitRepositoryDescriptionChanged e => ApplyEvent(e),
            GitRepositoryVisibilityChanged e => ApplyEvent(e),
            GitRepositoryDefaultBranchChanged e => ApplyEvent(e),
            GitRepositoryDisabled e => ApplyEvent(e),
            GitRepositoryEnabled e => ApplyEvent(e),
            GitRepositoryMarkedNotFound e => ApplyEvent(e),
            _ => ApplyResult.Error(this, $"Unknown event type: {@event.GetType().Name}")
        };
    }

    private ApplyResult ApplyEvent(GitRepositoryAdded e)
    {
        if (IsInitialized())
            return ApplyResult.Error(this, "Repository already initialized");
        return ApplyResult.Success(new GitRepository(e));
    }

    private ApplyResult ApplyEvent(GitRepositoryDisabled e)
    {
        if (!IsInitialized())
            return ApplyResult.NotInitialized(this);
        if (Disabled)
            return ApplyResult.Success(this); // Idempotent
        return ApplyResult.Success(this with { Disabled = true });
    }

    // ... similar pattern for other events
}
```

## 3. Create Commands

```csharp
// src/libraries/Application/Hexalith.GitStorage.Commands/GitRepository/AddGitRepository.cs
namespace Hexalith.GitStorage.Commands.GitRepository;

using System.Runtime.Serialization;

using Hexalith.GitStorage.Aggregates.Abstractions.Enums;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Command to create a new Git repository.
/// </summary>
[PolymorphicSerialization]
public sealed record AddGitRepository(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Description,
    [property: DataMember(Order = 4)] string OrganizationId,
    [property: DataMember(Order = 5)] GitRepositoryVisibility Visibility,
    [property: DataMember(Order = 6)] string? DefaultBranch)
    : GitRepositoryCommand(Id);
```

## 4. Create Validator

```csharp
// src/libraries/Application/Hexalith.GitStorage.Commands/GitRepository/Validators/AddGitRepositoryValidator.cs
namespace Hexalith.GitStorage.Commands.GitRepository.Validators;

using FluentValidation;

using Hexalith.GitStorage.Commands.GitRepository;
using Hexalith.GitStorage.Localizations;

using Microsoft.Extensions.Localization;

/// <summary>
/// Validator for AddGitRepository command.
/// </summary>
public sealed class AddGitRepositoryValidator : AbstractValidator<AddGitRepository>
{
    private const string NamePattern = @"^(?!\.)(?!.*\.\.)[a-zA-Z0-9._-]{1,100}(?<!\.git)$";

    public AddGitRepositoryValidator(IStringLocalizer<Labels> localizer)
    {
        _ = RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage(localizer[Labels.FieldIsRequired, nameof(AddGitRepository.Id)]);

        _ = RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(localizer[Labels.FieldIsRequired, nameof(AddGitRepository.Name)])
            .MaximumLength(100)
            .Matches(NamePattern)
            .WithMessage(localizer[Labels.InvalidRepositoryNameFormat]);

        _ = RuleFor(x => x.OrganizationId)
            .NotEmpty()
            .WithMessage(localizer[Labels.FieldIsRequired, nameof(AddGitRepository.OrganizationId)]);

        _ = RuleFor(x => x.Visibility)
            .IsInEnum()
            .WithMessage(localizer[Labels.InvalidVisibilityValue]);
    }
}
```

## 5. Create Projection Handler

```csharp
// src/libraries/Application/Hexalith.GitStorage.Projections/GitRepository/ProjectionHandlers/GitRepositoryAddedOnSummaryProjectionHandler.cs
namespace Hexalith.GitStorage.Projections.GitRepository.ProjectionHandlers;

using Hexalith.Application.Projections;
using Hexalith.GitStorage.Events.GitRepository;
using Hexalith.GitStorage.Requests.GitRepository;

/// <summary>
/// Handles GitRepositoryAdded events for summary projections.
/// </summary>
public sealed class GitRepositoryAddedOnSummaryProjectionHandler
    : KeyValueProjectionUpdateEventHandlerBase<GitRepositoryAdded, GitRepositorySummaryViewModel>
{
    protected override ValueTask<GitRepositorySummaryViewModel?> ApplyEventAsync(
        GitRepositoryAdded ev,
        GitRepositorySummaryViewModel? currentValue,
        CancellationToken ct)
    {
        return ValueTask.FromResult<GitRepositorySummaryViewModel?>(
            new GitRepositorySummaryViewModel(
                ev.Id,
                ev.Name,
                ev.OrganizationId,
                ev.Visibility,
                GitRepositorySyncStatus.Synced,
                Disabled: false));
    }
}
```

## 6. Create UI Page

```razor
@* src/libraries/Presentation/Hexalith.GitStorage.UI.Pages/GitRepository/GitRepositoryIndex.razor *@
@page "/GitRepository/GitRepository"
@using Hexalith.GitStorage.Requests.GitRepository
@using Microsoft.FluentUI.AspNetCore.Components

<HexEntityIndexPage
    TModel="GitRepositorySummaryViewModel"
    TRequest="GetGitRepositorySummaries"
    Title="Git Repositories"
    AddUrl="/GitRepository/Add/GitRepository">
    <GridColumns>
        <PropertyColumn Property="@(p => p.Name)" Title="Name" Sortable="true" />
        <PropertyColumn Property="@(p => p.OrganizationId)" Title="Organization" />
        <PropertyColumn Property="@(p => p.Visibility)" Title="Visibility" />
        <PropertyColumn Property="@(p => p.SyncStatus)" Title="Sync Status" />
        <TemplateColumn Title="Actions">
            <FluentButton Appearance="Appearance.Outline"
                          OnClick="@(() => Navigation.NavigateTo($"/GitRepository/{context.Id}"))">
                View
            </FluentButton>
        </TemplateColumn>
    </GridColumns>
</HexEntityIndexPage>
```

## ID Generation Pattern

Repository IDs follow the composite key pattern:

```csharp
public static string GenerateId(string organizationId, string repositoryName)
    => $"{organizationId}-{repositoryName}";
```

## File Checklist

Domain Layer:
- [ ] `GitRepositoryEvent.cs` - Base event
- [ ] `GitRepositoryAdded.cs`
- [ ] `GitRepositorySynced.cs`
- [ ] `GitRepositoryDescriptionChanged.cs`
- [ ] `GitRepositoryVisibilityChanged.cs`
- [ ] `GitRepositoryDefaultBranchChanged.cs`
- [ ] `GitRepositoryDisabled.cs`
- [ ] `GitRepositoryEnabled.cs`
- [ ] `GitRepositoryMarkedNotFound.cs`
- [ ] `GitRepository.cs` - Aggregate
- [ ] `GitRepositoryVisibility.cs` - Enum (if not reusing)
- [ ] `GitRepositoryOrigin.cs` - Enum
- [ ] `GitRepositorySyncStatus.cs` - Enum

Application Layer:
- [ ] `GitRepositoryCommand.cs` - Base command
- [ ] `AddGitRepository.cs`
- [ ] `SyncGitRepository.cs`
- [ ] `ChangeGitRepositoryDescription.cs`
- [ ] `ChangeGitRepositoryVisibility.cs`
- [ ] `ChangeGitRepositoryDefaultBranch.cs`
- [ ] `DisableGitRepository.cs`
- [ ] `EnableGitRepository.cs`
- [ ] Validators for all commands
- [ ] `GitRepositoryRequest.cs` - Base request
- [ ] `GetGitRepositorySummaries.cs`
- [ ] `GetGitRepositoryDetails.cs`
- [ ] `GitRepositorySummaryViewModel.cs`
- [ ] `GitRepositoryDetailsViewModel.cs`
- [ ] Projection handlers for each event

Infrastructure Layer:
- [ ] `GitRepositoryIntegrationEventsController.cs`

Presentation Layer:
- [ ] `GitRepositoryIndex.razor`
- [ ] `GitRepositoryDetails.razor`
- [ ] `GitRepositoryEditViewModel.cs`
- [ ] `GitRepositoryEditValidation.cs`

Tests:
- [ ] `AddGitRepositoryValidatorTests.cs`
- [ ] `GitRepositoryTests.cs`
- [ ] `GitRepositoryEventTests.cs`
