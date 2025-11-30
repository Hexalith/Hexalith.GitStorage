// <copyright file="GitStorageAccountWebApiHelpers.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Servers.Helpers;

using Hexalith.GitStorage.Aggregates;
using Hexalith.GitStorage.Projections.Helpers;
using Hexalith.GitStorage.Requests.GitStorageAccount;
using Hexalith.Infrastructure.DaprRuntime.Helpers;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Class GitStorageAccountWebApiHelpers.
/// </summary>
public static class GitStorageAccountWebApiHelpers
{
    /// <summary>
    /// Adds the GitStorageAccount projection actor factories.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when services is null.</exception>
    public static IServiceCollection AddGitStorageAccountProjectionActorFactories(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        _ = services.AddGitStorageAccountProjections();
        _ = services.AddActorProjectionFactory<GitStorageAccountSummaryViewModel>();
        _ = services.AddActorProjectionFactory<GitStorageAccountDetailsViewModel>();
        _ = services.AddActorProjectionFactory<GitStorageAccount>();
        return services;
    }
}