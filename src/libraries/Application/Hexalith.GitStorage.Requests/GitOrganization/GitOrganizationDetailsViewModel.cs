// <copyright file="GitOrganizationDetailsViewModel.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Requests.GitOrganization;

using System.Runtime.Serialization;

using Hexalith.Domains.ValueObjects;
using Hexalith.GitStorage.Aggregates.Enums;

/// <summary>
/// Represents the details of a GitOrganization.
/// </summary>
/// <param name="Id">The GitOrganization identifier (composite key).</param>
/// <param name="Name">The organization name.</param>
/// <param name="Description">Optional description of the organization.</param>
/// <param name="GitStorageAccountId">Reference to the parent GitStorageAccount entity.</param>
/// <param name="GitStorageAccountName">Parent Git Storage Account name (denormalized).</param>
/// <param name="Visibility">The visibility level of the organization.</param>
/// <param name="Origin">How the organization was added to the system.</param>
/// <param name="RemoteId">The remote server's unique identifier for the organization.</param>
/// <param name="SyncStatus">Current synchronization state with the remote Git Server.</param>
/// <param name="LastSyncedAt">Timestamp of the last successful sync.</param>
/// <param name="Disabled">Whether the organization is disabled locally.</param>
[DataContract]
public sealed record GitOrganizationDetailsViewModel(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Description,
    [property: DataMember(Order = 4)] string GitStorageAccountId,
    [property: DataMember(Order = 5)] string GitStorageAccountName,
    [property: DataMember(Order = 6)] GitOrganizationVisibility Visibility,
    [property: DataMember(Order = 7)] GitOrganizationOrigin Origin,
    [property: DataMember(Order = 8)] string? RemoteId,
    [property: DataMember(Order = 9)] GitOrganizationSyncStatus SyncStatus,
    [property: DataMember(Order = 10)] DateTimeOffset? LastSyncedAt,
    [property: DataMember(Order = 11)] bool Disabled) : IIdDescription
{
    /// <inheritdoc/>
    string IIdDescription.Description => Name;

    /// <summary>
    /// Gets an empty GitOrganization details view model.
    /// </summary>
    /// <returns>An empty GitOrganization details view model.</returns>
    public static GitOrganizationDetailsViewModel Empty => new(
        string.Empty,
        string.Empty,
        null,
        string.Empty,
        string.Empty,
        GitOrganizationVisibility.Public,
        GitOrganizationOrigin.Synced,
        null,
        GitOrganizationSyncStatus.Synced,
        null,
        false);

    /// <summary>
    /// Creates a new GitOrganization details view model with the specified ID.
    /// </summary>
    /// <param name="id">The GitOrganization identifier.</param>
    /// <param name="gitStorageAccountId">The Git Storage Account identifier.</param>
    /// <returns>A new GitOrganization details view model.</returns>
    public static GitOrganizationDetailsViewModel Create(string id, string gitStorageAccountId)
        => new(
            id,
            string.Empty,
            null,
            gitStorageAccountId,
            string.Empty,
            GitOrganizationVisibility.Public,
            GitOrganizationOrigin.CreatedViaApplication,
            null,
            GitOrganizationSyncStatus.Synced,
            null,
            false);
}