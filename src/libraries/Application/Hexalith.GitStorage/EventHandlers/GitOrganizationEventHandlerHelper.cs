// <copyright file="GitOrganizationEventHandlerHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.GitStorage.EventHandlers;

using FluentValidation;

using Hexalith.GitStorage.Events.GitOrganization;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper class for adding GitOrganization event handlers to the service collection.
/// </summary>
public static class GitOrganizationEventHandlerHelper
{
    /// <summary>
    /// Adds the GitOrganization event validators to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddGitOrganizationEventValidators(this IServiceCollection services)
            => services.AddTransient<IValidator<GitOrganizationAdded>, GitOrganizationAddedValidator>();
}