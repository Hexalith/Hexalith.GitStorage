// <copyright file="GitStorageAccountCommandHandlerHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.GitStorage.CommandHandlers;

using Hexalith.Application.Commands;
using Hexalith.GitStorage.Aggregates;
using Hexalith.GitStorage.Commands.GitStorageAccount;
using Hexalith.GitStorage.Events.GitStorageAccount;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper class for adding GitStorageAccount command handlers to the service collection.
/// </summary>
public static class GitStorageAccountCommandHandlerHelper
{
    /// <summary>
    /// Adds the GitStorageAccount command handlers to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddGitStorageAccountCommandHandlers(this IServiceCollection services)
    {
        _ = services.TryAddSimpleInitializationCommandHandler<AddGitStorageAccount>(
            c => new GitStorageAccountAdded(
                c.Id,
                c.Name,
                c.Comments),
            ev => new GitStorageAccount((GitStorageAccountAdded)ev));

        _ = services.TryAddSimpleCommandHandler<ChangeGitStorageAccountDescription>(
            c => new GitStorageAccountDescriptionChanged(
                c.Id,
                c.Name,
                c.Comments));

        _ = services.TryAddSimpleCommandHandler<DisableGitStorageAccount>(
            c => new GitStorageAccountDisabled(c.Id));

        _ = services.TryAddSimpleCommandHandler<EnableGitStorageAccount>(
            c => new GitStorageAccountEnabled(c.Id));

        return services;
    }
}