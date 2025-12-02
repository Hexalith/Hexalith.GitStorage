// <copyright file="GitOrganizationDisabled.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Events.GitOrganization;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Event raised when an organization is disabled locally.
/// </summary>
/// <param name="Id">The identifier of the GitOrganization (composite key).</param>
[PolymorphicSerialization]
public partial record GitOrganizationDisabled(string Id)
    : GitOrganizationEvent(Id);
