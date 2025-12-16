// <copyright file="GitRepositorySyncStatus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Aggregates.Enums;

/// <summary>
/// Specifies the synchronization status of a Git Repository with the remote server.
/// </summary>
public enum GitRepositorySyncStatus
{
    /// <summary>
    /// Repository is successfully synchronized with the remote server.
    /// </summary>
    Synced = 0,

    /// <summary>
    /// Repository exists locally but was not found on the remote server.
    /// </summary>
    NotFoundOnRemote = 1,

    /// <summary>
    /// An error occurred during the last synchronization attempt.
    /// </summary>
    SyncError = 2,
}
