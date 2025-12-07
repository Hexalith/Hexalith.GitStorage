// <copyright file="ChangeGitOrganizationVisibility.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Commands.GitOrganization;

using System.Runtime.Serialization;

using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Command to change the visibility of a GitOrganization.
/// </summary>
/// <param name="Id">The GitOrganization identifier (composite key).</param>
/// <param name="Visibility">The new visibility level for the organization.</param>
[PolymorphicSerialization]
public partial record ChangeGitOrganizationVisibility(
    string Id,
    [property: DataMember(Order = 2)] GitOrganizationVisibility Visibility)
    : GitOrganizationCommand(Id);