// <copyright file="GitRepositorySummaryViewModel.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Requests.GitRepository;

using System.Runtime.Serialization;

using Hexalith.Domains.ValueObjects;
using Hexalith.GitStorage.Aggregates.Enums;

/// <summary>
/// Represents a summary view of a GitRepository with essential information.
/// </summary>
/// <param name="Id">The unique identifier of the GitRepository (composite key).</param>
/// <param name="Name">The repository name.</param>
/// <param name="OrganizationId">Reference to the parent GitOrganization entity.</param>
/// <param name="Visibility">The visibility level of the repository.</param>
/// <param name="SyncStatus">Current synchronization state with the remote Git Server.</param>
/// <param name="Disabled">Indicates whether the repository is disabled.</param>
[DataContract]
public sealed record GitRepositorySummaryViewModel(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string OrganizationId,
    [property: DataMember(Order = 4)] GitRepositoryVisibility Visibility,
    [property: DataMember(Order = 5)] GitRepositorySyncStatus SyncStatus,
    [property: DataMember(Order = 6)] bool Disabled) : IIdDescription
{
    /// <inheritdoc/>
    string IIdDescription.Description => Name;

    /// <inheritdoc/>
    string IIdDescription.Search => $"{Id} {Name} {OrganizationId} {Visibility}";
}
