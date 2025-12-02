// <copyright file="DisableGitOrganization.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Commands.GitOrganization;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Command to disable a GitOrganization locally.
/// </summary>
/// <param name="Id">The composite key of the GitOrganization to disable.</param>
[PolymorphicSerialization]
public partial record DisableGitOrganization(string Id)
    : GitOrganizationCommand(Id);
