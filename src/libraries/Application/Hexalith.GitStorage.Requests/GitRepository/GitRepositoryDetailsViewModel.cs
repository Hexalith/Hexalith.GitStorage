// <copyright file="GitRepositoryDetailsViewModel.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Requests.GitRepository;

using System.Runtime.Serialization;

using Hexalith.Domains.ValueObjects;
using Hexalith.GitStorage.Aggregates.Enums;

/// <summary>
/// Represents the details of a GitRepository.
/// </summary>
/// <param name="Id">The GitRepository identifier (composite key).</param>
/// <param name="Name">The repository name.</param>
/// <param name="Description">Optional description of the repository.</param>
/// <param name="Url">The HTTPS clone URL of the repository.</param>
/// <param name="DefaultBranch">The default branch name.</param>
/// <param name="OrganizationId">Reference to the parent GitOrganization entity.</param>
/// <param name="OrganizationName">Parent Git Organization name (denormalized for display).</param>
/// <param name="Visibility">The visibility level of the repository.</param>
/// <param name="Origin">How the repository was added to the system.</param>
/// <param name="RemoteId">The remote server's unique identifier for the repository.</param>
/// <param name="SyncStatus">Current synchronization state with the remote Git Server.</param>
/// <param name="LastSyncedAt">Timestamp of the last successful sync.</param>
/// <param name="Disabled">Whether the repository is disabled locally.</param>
[DataContract]
public sealed record GitRepositoryDetailsViewModel(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Description,
    [property: DataMember(Order = 4)] string? Url,
    [property: DataMember(Order = 5)] string? DefaultBranch,
    [property: DataMember(Order = 6)] string OrganizationId,
    [property: DataMember(Order = 7)] string OrganizationName,
    [property: DataMember(Order = 8)] GitRepositoryVisibility Visibility,
    [property: DataMember(Order = 9)] GitRepositoryOrigin Origin,
    [property: DataMember(Order = 10)] string? RemoteId,
    [property: DataMember(Order = 11)] GitRepositorySyncStatus SyncStatus,
    [property: DataMember(Order = 12)] DateTimeOffset? LastSyncedAt,
    [property: DataMember(Order = 13)] bool Disabled) : IIdDescription
{
    /// <inheritdoc/>
    string IIdDescription.Description => Name;

    /// <summary>
    /// Gets an empty GitRepository details view model.
    /// </summary>
    /// <returns>An empty GitRepository details view model.</returns>
    public static GitRepositoryDetailsViewModel Empty => new(
        string.Empty,
        string.Empty,
        null,
        null,
        null,
        string.Empty,
        string.Empty,
        GitRepositoryVisibility.Public,
        GitRepositoryOrigin.Synced,
        null,
        GitRepositorySyncStatus.Synced,
        null,
        false);

    /// <summary>
    /// Creates a new GitRepository details view model with the specified ID.
    /// </summary>
    /// <param name="id">The GitRepository identifier.</param>
    /// <param name="organizationId">The Git Organization identifier.</param>
    /// <returns>A new GitRepository details view model.</returns>
    public static GitRepositoryDetailsViewModel Create(string id, string organizationId)
        => new(
            id,
            string.Empty,
            null,
            null,
            null,
            organizationId,
            string.Empty,
            GitRepositoryVisibility.Public,
            GitRepositoryOrigin.CreatedViaApplication,
            null,
            GitRepositorySyncStatus.Synced,
            null,
            false);
}
