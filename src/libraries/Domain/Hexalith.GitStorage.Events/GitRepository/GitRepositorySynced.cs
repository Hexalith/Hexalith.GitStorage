// <copyright file="GitRepositorySynced.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Events.GitRepository;

using System.Runtime.Serialization;

using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Event raised when a GitRepository is discovered or re-synced from the remote Git Server.
/// </summary>
/// <param name="Id">The identifier of the GitRepository (composite key: {OrganizationId}-{RepositoryName}).</param>
/// <param name="Name">The repository name as it appears on the Git Server.</param>
/// <param name="Description">Optional description of the repository.</param>
/// <param name="OrganizationId">Reference to the parent GitOrganization entity.</param>
/// <param name="Visibility">The visibility level of the repository.</param>
/// <param name="DefaultBranch">The default branch name.</param>
/// <param name="RemoteId">The repository's unique identifier on the remote Git Server.</param>
/// <param name="Url">The HTTPS clone URL of the repository.</param>
/// <param name="SyncedAt">The timestamp when the sync occurred.</param>
[PolymorphicSerialization]
public partial record GitRepositorySynced(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Description,
    [property: DataMember(Order = 4)] string OrganizationId,
    [property: DataMember(Order = 5)] GitRepositoryVisibility Visibility,
    [property: DataMember(Order = 6)] string? DefaultBranch,
    [property: DataMember(Order = 7)] string? RemoteId,
    [property: DataMember(Order = 8)] string? Url,
    [property: DataMember(Order = 9)] DateTimeOffset SyncedAt)
    : GitRepositoryEvent(Id);
