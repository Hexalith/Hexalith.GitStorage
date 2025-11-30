// <copyright file="IMyToDoService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule;

using System.Threading.Tasks;

/// <summary>
/// Interface for MyToDo upload service.
/// </summary>
public interface IMyToDoService
{
    /// <summary>
    /// Performs an asynchronous operation related to MyToDo.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DoSomethingAsync(CancellationToken cancellationToken);
}