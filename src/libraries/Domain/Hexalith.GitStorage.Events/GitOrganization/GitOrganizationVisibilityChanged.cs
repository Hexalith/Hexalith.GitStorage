// <copyright file="GitOrganizationVisibilityChanged.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Events.GitOrganization;

using System.Runtime.Serialization;

using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Event raised when a GitOrganization visibility level is changed.
/// </summary>
/// <param name="Id">The identifier of the GitOrganization (composite key).</param>
/// <param name="Visibility">The new visibility level of the organization.</param>
[PolymorphicSerialization]
public partial record GitOrganizationVisibilityChanged(
    string Id,
    [property: DataMember(Order = 2)] GitOrganizationVisibility Visibility)
    : GitOrganizationEvent(Id);