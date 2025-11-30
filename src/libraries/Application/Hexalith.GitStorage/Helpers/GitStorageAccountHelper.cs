// <copyright file="GitStorageAccountHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.GitStorage.Helpers;

using Hexalith.GitStorage.CommandHandlers;
using Hexalith.GitStorage.EventHandlers;
using Hexalith.GitStorage.Projections.Helpers;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper class for adding GitStorageAccount projections to the service collection.
/// </summary>
public static class GitStorageAccountHelper
{
    /// <summary>
    /// Adds the GitStorageAccount module to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddGitStorageAccount(this IServiceCollection services)
    {
        _ = services.AddGitStorageAccountCommandHandlers();
        _ = services.AddGitStorageAccountAggregateProviders();
        _ = services.AddGitStorageAccountEventValidators();
        return services;
    }
}