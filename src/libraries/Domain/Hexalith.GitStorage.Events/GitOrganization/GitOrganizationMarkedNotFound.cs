// <copyright file="GitOrganizationMarkedNotFound.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Events.GitOrganization;

using System.Runtime.Serialization;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Event raised when an organization is not found on the remote during sync.
/// </summary>
/// <param name="Id">The identifier of the GitOrganization (composite key).</param>
/// <param name="MarkedAt">Timestamp when the organization was flagged as not found.</param>
[PolymorphicSerialization]
public partial record GitOrganizationMarkedNotFound(
    string Id,
    [property: DataMember(Order = 2)] DateTimeOffset MarkedAt)
    : GitOrganizationEvent(Id);
