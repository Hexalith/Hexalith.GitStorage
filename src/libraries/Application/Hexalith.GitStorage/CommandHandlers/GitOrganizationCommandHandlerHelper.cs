// <copyright file="GitOrganizationCommandHandlerHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.CommandHandlers;

using Hexalith.Application.Commands;
using Hexalith.GitStorage.Aggregates;
using Hexalith.GitStorage.Commands.GitOrganization;
using Hexalith.GitStorage.Events.GitOrganization;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper class for adding GitOrganization command handlers to the service collection.
/// </summary>
public static class GitOrganizationCommandHandlerHelper
{
    /// <summary>
    /// Adds the GitOrganization command handlers to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddGitOrganizationCommandHandlers(this IServiceCollection services)
    {
        _ = services.TryAddSimpleInitializationCommandHandler<AddGitOrganization>(
            c => new GitOrganizationAdded(
                c.Id,
                c.Name,
                c.Description,
                c.GitStorageAccountId),
            ev => new GitOrganization((GitOrganizationAdded)ev));

        _ = services.TryAddSimpleCommandHandler<ChangeGitOrganizationDescription>(
            c => new GitOrganizationDescriptionChanged(
                c.Id,
                c.Name,
                c.Description));

        _ = services.TryAddSimpleCommandHandler<DisableGitOrganization>(
            c => new GitOrganizationDisabled(c.Id));

        _ = services.TryAddSimpleCommandHandler<EnableGitOrganization>(
            c => new GitOrganizationEnabled(c.Id));

        return services;
    }
}
