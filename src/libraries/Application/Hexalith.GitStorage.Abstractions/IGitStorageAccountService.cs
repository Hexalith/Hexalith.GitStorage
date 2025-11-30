// <copyright file="IGitStorageAccountService.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage;

using System.Threading.Tasks;

/// <summary>
/// Interface for GitStorageAccount upload service.
/// </summary>
public interface IGitStorageAccountService
{
    /// <summary>
    /// Performs an asynchronous operation related to GitStorageAccount.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DoSomethingAsync(CancellationToken cancellationToken);
}