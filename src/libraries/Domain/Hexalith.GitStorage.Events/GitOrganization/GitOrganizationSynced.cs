// <copyright file="GitOrganizationSynced.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Events.GitOrganization;

using System.Runtime.Serialization;

using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Event raised when an organization is discovered or re-synced from the remote server.
/// </summary>
/// <param name="Id">The identifier of the GitOrganization (composite key).</param>
/// <param name="Name">The organization name from remote.</param>
/// <param name="Description">Description from remote.</param>
/// <param name="GitStorageAccountId">Reference to the parent GitStorageAccount entity.</param>
/// <param name="Visibility">The visibility level of the organization.</param>
/// <param name="RemoteId">The remote server's unique identifier for the organization.</param>
/// <param name="SyncedAt">Timestamp of the sync operation.</param>
[PolymorphicSerialization]
public partial record GitOrganizationSynced(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Description,
    [property: DataMember(Order = 4)] string GitStorageAccountId,
    [property: DataMember(Order = 5)] GitOrganizationVisibility Visibility,
    [property: DataMember(Order = 6)] string? RemoteId,
    [property: DataMember(Order = 7)] DateTimeOffset SyncedAt)
    : GitOrganizationEvent(Id);