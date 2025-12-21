// <copyright file="GitRepository.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Aggregates;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using Hexalith.Domains;
using Hexalith.Domains.Results;
using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.GitStorage.Events.GitRepository;

/// <summary>
/// Represents a GitRepository aggregate.
/// </summary>
/// <param name="Id">The GitRepository identifier (composite key: {OrganizationId}-{RepositoryName}).</param>
/// <param name="Name">The repository name as it appears on the Git Server.</param>
/// <param name="Description">Optional description of the repository.</param>
/// <param name="Url">The HTTPS clone URL of the repository.</param>
/// <param name="DefaultBranch">The default branch name (e.g., "main").</param>
/// <param name="OrganizationId">Reference to the parent GitOrganization entity.</param>
/// <param name="Visibility">The visibility level of the repository.</param>
/// <param name="Origin">How the repository was added to the system.</param>
/// <param name="RemoteId">The repository's unique identifier on the remote Git Server.</param>
/// <param name="SyncStatus">Current synchronization state with the remote Git Server.</param>
/// <param name="LastSyncedAt">Timestamp of the last successful sync.</param>
/// <param name="Disabled">Whether the repository is suspended locally.</param>
[DataContract]
public sealed record GitRepository(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Description,
    [property: DataMember(Order = 4)] string? Url,
    [property: DataMember(Order = 5)] string? DefaultBranch,
    [property: DataMember(Order = 6)] string OrganizationId,
    [property: DataMember(Order = 7)] GitRepositoryVisibility Visibility,
    [property: DataMember(Order = 8)] GitRepositoryOrigin Origin,
    [property: DataMember(Order = 9)] string? RemoteId,
    [property: DataMember(Order = 10)] GitRepositorySyncStatus SyncStatus,
    [property: DataMember(Order = 11)] DateTimeOffset? LastSyncedAt,
    [property: DataMember(Order = 12)] bool Disabled) : IDomainAggregate
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GitRepository"/> class.
    /// </summary>
    public GitRepository()
        : this(string.Empty, string.Empty, null, null, null, string.Empty, GitRepositoryVisibility.Public, GitRepositoryOrigin.Synced, null, GitRepositorySyncStatus.Synced, null, false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GitRepository"/> class with the specified added event.
    /// </summary>
    /// <param name="added">The event that adds a GitRepository.</param>
    public GitRepository(GitRepositoryAdded added)
        : this(
            (added ?? throw new ArgumentNullException(nameof(added))).Id,
            added.Name,
            added.Description,
            added.Url,
            added.DefaultBranch,
            added.OrganizationId,
            added.Visibility,
            GitRepositoryOrigin.CreatedViaApplication,
            added.RemoteId,
            GitRepositorySyncStatus.Synced,
            null,
            false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GitRepository"/> class with the specified synced event.
    /// </summary>
    /// <param name="synced">The event that syncs a GitRepository.</param>
    public GitRepository(GitRepositorySynced synced)
        : this(
            (synced ?? throw new ArgumentNullException(nameof(synced))).Id,
            synced.Name,
            synced.Description,
            synced.Url,
            synced.DefaultBranch,
            synced.OrganizationId,
            synced.Visibility,
            GitRepositoryOrigin.Synced,
            synced.RemoteId,
            GitRepositorySyncStatus.Synced,
            synced.SyncedAt,
            false)
    {
    }

    /// <inheritdoc/>
    public string AggregateId => Id;

    /// <inheritdoc/>
    public string AggregateName => GitRepositoryDomainHelper.GitRepositoryAggregateName;

    /// <inheritdoc/>
    public ApplyResult Apply([NotNull] object domainEvent)
    {
        ArgumentNullException.ThrowIfNull(domainEvent);
        if (domainEvent is GitRepositoryEvent && domainEvent is not GitRepositoryEnabled or GitRepositoryDisabled && Disabled)
        {
            return ApplyResult.NotEnabled(this);
        }
        else if (!(this as IDomainAggregate).IsInitialized() && domainEvent is not GitRepositoryAdded and not GitRepositorySynced)
        {
            return ApplyResult.NotInitialized(this);
        }
        else
        {
            return domainEvent switch
            {
                GitRepositoryAdded e => ApplyEvent(e),
                GitRepositorySynced e => ApplyEvent(e),
                GitRepositoryDescriptionChanged e => ApplyEvent(e),
                GitRepositoryVisibilityChanged e => ApplyEvent(e),
                GitRepositoryDefaultBranchChanged e => ApplyEvent(e),
                GitRepositoryMarkedNotFound e => ApplyEvent(e),
                GitRepositoryDisabled e => ApplyEvent(e),
                GitRepositoryEnabled e => ApplyEvent(e),
                GitRepositoryEvent => ApplyResult.NotImplemented(this),
                _ => ApplyResult.InvalidEvent(this, domainEvent),
            };
        }
    }

    private ApplyResult ApplyEvent(GitRepositoryAdded e) => !(this as IDomainAggregate).IsInitialized()
        ? ApplyResult.Success(new GitRepository(e), [e])
        : ApplyResult.Error(this, "The GitRepository already exists.");

    private ApplyResult ApplyEvent(GitRepositorySynced e) => !(this as IDomainAggregate).IsInitialized()
        ? ApplyResult.Success(new GitRepository(e), [e])
        : ApplyResult.Success(
            this with
            {
                Name = e.Name,
                Description = e.Description,
                Url = e.Url,
                DefaultBranch = e.DefaultBranch,
                Visibility = e.Visibility,
                RemoteId = e.RemoteId,
                SyncStatus = GitRepositorySyncStatus.Synced,
                LastSyncedAt = e.SyncedAt,
            },
            [e]);

    private ApplyResult ApplyEvent(GitRepositoryDescriptionChanged e) => Description == e.Description
        ? ApplyResult.Error(this, "The GitRepository description is already set to the specified value.")
        : ApplyResult.Success(this with { Description = e.Description }, [e]);

    private ApplyResult ApplyEvent(GitRepositoryVisibilityChanged e) => Visibility == e.Visibility
        ? ApplyResult.Error(this, "The GitRepository visibility is already set to the specified value.")
        : ApplyResult.Success(this with { Visibility = e.Visibility }, [e]);

    private ApplyResult ApplyEvent(GitRepositoryDefaultBranchChanged e) => DefaultBranch == e.DefaultBranch
        ? ApplyResult.Error(this, "The GitRepository default branch is already set to the specified value.")
        : ApplyResult.Success(this with { DefaultBranch = e.DefaultBranch }, [e]);

    private ApplyResult ApplyEvent(GitRepositoryMarkedNotFound e) => SyncStatus == GitRepositorySyncStatus.NotFoundOnRemote
        ? ApplyResult.Error(this, "The GitRepository is already marked as not found on remote.")
        : ApplyResult.Success(this with { SyncStatus = GitRepositorySyncStatus.NotFoundOnRemote }, [e]);

    private ApplyResult ApplyEvent(GitRepositoryDisabled e) => Disabled
        ? ApplyResult.Error(this, "The GitRepository is already disabled.")
        : ApplyResult.Success(this with { Disabled = true }, [e]);

    private ApplyResult ApplyEvent(GitRepositoryEnabled e) => Disabled
        ? ApplyResult.Success(this with { Disabled = false }, [e])
        : ApplyResult.Error(this, "The GitRepository is already enabled.");
}
