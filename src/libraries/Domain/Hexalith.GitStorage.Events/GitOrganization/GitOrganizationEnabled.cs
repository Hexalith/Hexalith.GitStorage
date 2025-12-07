// <copyright file="GitOrganizationEnabled.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Events.GitOrganization;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Event raised when a disabled organization is re-enabled.
/// </summary>
/// <param name="Id">The identifier of the GitOrganization (composite key).</param>
[PolymorphicSerialization]
public partial record GitOrganizationEnabled(string Id)
    : GitOrganizationEvent(Id);
