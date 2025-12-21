// <copyright file="GitRepositoryOrigin.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Aggregates.Enums;

/// <summary>
/// Specifies how a Git Repository was added to the system.
/// </summary>
public enum GitRepositoryOrigin
{
    /// <summary>
    /// Repository was discovered from the remote Git Server during synchronization.
    /// </summary>
    Synced = 0,

    /// <summary>
    /// Repository was created via this application's API.
    /// </summary>
    CreatedViaApplication = 1,
}
