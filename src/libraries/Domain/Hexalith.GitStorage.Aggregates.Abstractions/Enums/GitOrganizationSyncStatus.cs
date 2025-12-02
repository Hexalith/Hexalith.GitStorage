// <copyright file="GitOrganizationSyncStatus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Aggregates.Enums;

/// <summary>
/// Indicates the current synchronization state with the remote Git Server.
/// </summary>
public enum GitOrganizationSyncStatus
{
    /// <summary>
    /// Successfully synchronized with remote; organization exists on both local and remote.
    /// </summary>
    Synced = 0,

    /// <summary>
    /// Organization exists locally but was not found during last sync (may have been deleted remotely).
    /// </summary>
    NotFoundOnRemote = 1,

    /// <summary>
    /// Remote operation failed; awaiting retry (set by event handler failure).
    /// </summary>
    SyncError = 2,
}
