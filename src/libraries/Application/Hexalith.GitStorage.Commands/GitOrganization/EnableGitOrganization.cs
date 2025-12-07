// <copyright file="EnableGitOrganization.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Commands.GitOrganization;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Command to enable a previously disabled GitOrganization.
/// </summary>
/// <param name="Id">The composite key of the GitOrganization to enable.</param>
[PolymorphicSerialization]
public partial record EnableGitOrganization(string Id)
    : GitOrganizationCommand(Id);
