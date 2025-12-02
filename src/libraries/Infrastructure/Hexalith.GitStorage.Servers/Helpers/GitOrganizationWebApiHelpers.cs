// <copyright file="GitOrganizationWebApiHelpers.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Servers.Helpers;

using Hexalith.GitStorage.Aggregates;
using Hexalith.GitStorage.Projections.Helpers;
using Hexalith.GitStorage.Requests.GitOrganization;
using Hexalith.Infrastructure.DaprRuntime.Helpers;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper class for GitOrganization Web API configuration.
/// </summary>
public static class GitOrganizationWebApiHelpers
{
    /// <summary>
    /// Adds the GitOrganization projection actor factories.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when services is null.</exception>
    public static IServiceCollection AddGitOrganizationProjectionActorFactories(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        _ = services.AddGitOrganizationProjections();
        _ = services.AddActorProjectionFactory<GitOrganizationSummaryViewModel>();
        _ = services.AddActorProjectionFactory<GitOrganizationDetailsViewModel>();
        _ = services.AddActorProjectionFactory<GitOrganization>();
        return services;
    }
}
