// <copyright file="GitStorageAccountStatus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Aggregates.Enums;

/// <summary>
/// Represents the status of GitStorageAccount.
/// </summary>
public enum GitStorageAccountStatus
{
    /// <summary>
    /// The GitStorageAccount is in draft status.
    /// </summary>
    Draft = 0,

    /// <summary>
    /// The GitStorageAccount is submitted for approval.
    /// </summary>
    Submitted = 1,

    /// <summary>
    /// The GitStorageAccount is approved.
    /// </summary>
    Approved = 2,

    /// <summary>
    /// The GitStorageAccount is rejected.
    /// </summary>
    Rejected = 3,
}