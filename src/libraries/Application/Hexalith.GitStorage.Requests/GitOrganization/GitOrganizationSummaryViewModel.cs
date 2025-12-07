// <copyright file="GitOrganizationSummaryViewModel.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Requests.GitOrganization;

using System.Runtime.Serialization;

using Hexalith.Domains.ValueObjects;
using Hexalith.GitStorage.Aggregates.Enums;

/// <summary>
/// Represents a summary view of a GitOrganization with essential information.
/// </summary>
/// <param name="Id">The unique identifier of the GitOrganization (composite key).</param>
/// <param name="Name">The organization name.</param>
/// <param name="GitStorageAccountId">Reference to the parent GitStorageAccount entity.</param>
/// <param name="Visibility">The visibility level of the organization.</param>
/// <param name="SyncStatus">Current synchronization state with the remote Git Server.</param>
/// <param name="Disabled">Indicates whether the organization is disabled.</param>
[DataContract]
public sealed record GitOrganizationSummaryViewModel(
    [property: DataMember(Order = 1)] string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string GitStorageAccountId,
    [property: DataMember(Order = 4)] GitOrganizationVisibility Visibility,
    [property: DataMember(Order = 5)] GitOrganizationSyncStatus SyncStatus,
    [property: DataMember(Order = 6)] bool Disabled) : IIdDescription
{
    /// <inheritdoc/>
    string IIdDescription.Description => Name;

    /// <inheritdoc/>
    string IIdDescription.Search => $"{Id} {Name} {GitStorageAccountId} {Visibility}";
}