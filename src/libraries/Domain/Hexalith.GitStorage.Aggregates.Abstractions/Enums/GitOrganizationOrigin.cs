// <copyright file="GitOrganizationOrigin.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Aggregates.Enums;

/// <summary>
/// Indicates how the organization was added to the system.
/// </summary>
public enum GitOrganizationOrigin
{
    /// <summary>
    /// Discovered from remote Git Server during sync operation.
    /// </summary>
    Synced = 0,

    /// <summary>
    /// Created through the application API and provisioned on remote.
    /// </summary>
    CreatedViaApplication = 1,
}
