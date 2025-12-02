// <copyright file="ChangeGitOrganizationDescription.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Commands.GitOrganization;

using System.Runtime.Serialization;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Command to change the description of a GitOrganization.
/// </summary>
/// <param name="Id">The composite key of the GitOrganization.</param>
/// <param name="Name">The organization name (may be updated).</param>
/// <param name="Description">The new description for the organization.</param>
[PolymorphicSerialization]
public partial record ChangeGitOrganizationDescription(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Description)
    : GitOrganizationCommand(Id);
