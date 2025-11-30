// <copyright file="MyToDoStatus.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Aggregates.Enums;

/// <summary>
/// Represents the status of MyToDo.
/// </summary>
public enum MyToDoStatus
{
    /// <summary>
    /// The MyToDo is in draft status.
    /// </summary>
    Draft = 0,

    /// <summary>
    /// The MyToDo is submitted for approval.
    /// </summary>
    Submitted = 1,

    /// <summary>
    /// The MyToDo is approved.
    /// </summary>
    Approved = 2,

    /// <summary>
    /// The MyToDo is rejected.
    /// </summary>
    Rejected = 3,
}