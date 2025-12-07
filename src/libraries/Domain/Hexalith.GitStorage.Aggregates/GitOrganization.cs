// <copyright file="GitOrganization.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Aggregates;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

using Hexalith.Domains;
using Hexalith.Domains.Results;
using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.GitStorage.Events.GitOrganization;

/// <summary>
/// Represents a GitOrganization aggregate.
/// </summary>
/// <param name="Id">The GitOrganization identifier (composite key: {GitStorageAccountId}-{OrganizationName}).</param>
/// <param name="Name">The organization name as it appears on the Git Server.</param>
/// <param name="Description">Optional description of the organization.</param>
/// <param name="GitStorageAccountId">Reference to the parent GitStorageAccount entity.</param>
/// <param name="Visibility">The visibility level of the organization.</param>
/// <param name="Origin">How the organization was added to the system.</param>
/// <param name="RemoteId">The organization's unique identifier on the remote Git Server.</param>
/// <param name="SyncStatus">Current synchronization state with the remote Git Server.</param>
/// <param name="LastSyncedAt">Timestamp of the last successful sync.</param>
/// <param name="Disabled">Whether the organization is suspended locally.</param>
[DataContract]
public sealed record GitOrganization(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Description,
    [property: DataMember(Order = 4)] string GitStorageAccountId,
    [property: DataMember(Order = 5)] GitOrganizationVisibility Visibility,
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
        : this(string.Empty, string.Empty, null, string.Empty, GitOrganizationVisibility.Public, GitOrganizationOrigin.Synced, null, GitOrganizationSyncStatus.Synced, null, false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GitOrganization"/> class with the specified added event.
    /// </summary>
    /// <param name="added">The event that adds a GitOrganization.</param>
    public GitOrganization(GitOrganizationAdded added)
        : this(
            (added ?? throw new ArgumentNullException(nameof(added))).Id,
            added.Name,
            added.Description,
            added.GitStorageAccountId,
            added.Visibility,
            GitOrganizationOrigin.CreatedViaApplication,
            null,
            GitOrganizationSyncStatus.Synced,
            null,
            false)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="GitOrganization"/> class with the specified synced event.
    /// </summary>
    /// <param name="synced">The event that syncs a GitOrganization.</param>
    public GitOrganization(GitOrganizationSynced synced)
        : this(
            (synced ?? throw new ArgumentNullException(nameof(synced))).Id,
            synced.Name,
            synced.Description,
            synced.GitStorageAccountId,
            synced.Visibility,
            GitOrganizationOrigin.Synced,
            synced.RemoteId,
            GitOrganizationSyncStatus.Synced,
            synced.SyncedAt,
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
        if (domainEvent is GitOrganizationEvent && domainEvent is not GitOrganizationEnabled or GitOrganizationDisabled && Disabled)
        {
            return ApplyResult.NotEnabled(this);
        }
        else if (!(this as IDomainAggregate).IsInitialized() && domainEvent is not GitOrganizationAdded and not GitOrganizationSynced)
        {
            return ApplyResult.NotInitialized(this);
        }
        else
        {
            return domainEvent switch
            {
                GitOrganizationAdded e => ApplyEvent(e),
                GitOrganizationSynced e => ApplyEvent(e),
                GitOrganizationDescriptionChanged e => ApplyEvent(e),
                GitOrganizationVisibilityChanged e => ApplyEvent(e),
                GitOrganizationMarkedNotFound e => ApplyEvent(e),
                GitOrganizationDisabled e => ApplyEvent(e),
                GitOrganizationEnabled e => ApplyEvent(e),
                GitOrganizationEvent => ApplyResult.NotImplemented(this),
                _ => ApplyResult.InvalidEvent(this, domainEvent),
            };
        }
    }

    private ApplyResult ApplyEvent(GitOrganizationAdded e) => !(this as IDomainAggregate).IsInitialized()
        ? ApplyResult.Success(new GitOrganization(e), [e])
        : ApplyResult.Error(this, "The GitOrganization already exists.");

    private ApplyResult ApplyEvent(GitOrganizationSynced e) => !(this as IDomainAggregate).IsInitialized()
        ? ApplyResult.Success(new GitOrganization(e), [e])
        : ApplyResult.Success(
            this with
            {
                Name = e.Name,
                Description = e.Description,
                Visibility = e.Visibility,
                RemoteId = e.RemoteId,
                SyncStatus = GitOrganizationSyncStatus.Synced,
                LastSyncedAt = e.SyncedAt,
            },
            [e]);

    private ApplyResult ApplyEvent(GitOrganizationDescriptionChanged e) => Description == e.Description && Name == e.Name
        ? ApplyResult.Error(this, "The GitOrganization name and description are already set to the specified values.")
        : ApplyResult.Success(this with { Description = e.Description, Name = e.Name }, [e]);

    private ApplyResult ApplyEvent(GitOrganizationVisibilityChanged e) => Visibility == e.Visibility
        ? ApplyResult.Error(this, "The GitOrganization visibility is already set to the specified value.")
        : ApplyResult.Success(this with { Visibility = e.Visibility }, [e]);

    private ApplyResult ApplyEvent(GitOrganizationMarkedNotFound e) => SyncStatus == GitOrganizationSyncStatus.NotFoundOnRemote
        ? ApplyResult.Error(this, "The GitOrganization is already marked as not found on remote.")
        : ApplyResult.Success(this with { SyncStatus = GitOrganizationSyncStatus.NotFoundOnRemote }, [e]);

    private ApplyResult ApplyEvent(GitOrganizationDisabled e) => Disabled
        ? ApplyResult.Error(this, "The GitOrganization is already disabled.")
        : ApplyResult.Success(this with { Disabled = true }, [e]);

    private ApplyResult ApplyEvent(GitOrganizationEnabled e) => Disabled
        ? ApplyResult.Success(this with { Disabled = false }, [e])
        : ApplyResult.Error(this, "The GitOrganization is already enabled.");
}