// <copyright file="GitOrganizationProjectionHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Projections.Helpers;

using Hexalith.Application.Aggregates;
using Hexalith.Application.Projections;
using Hexalith.Application.Requests;
using Hexalith.GitStorage.Aggregates;
using Hexalith.GitStorage.Events.GitOrganization;
using Hexalith.GitStorage.Projections.ProjectionHandlers.Details;
using Hexalith.GitStorage.Projections.ProjectionHandlers.Summaries;
using Hexalith.GitStorage.Projections.RequestHandlers;
using Hexalith.GitStorage.Requests.GitOrganization;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Helper class for adding GitOrganization projections to the service collection.
/// </summary>
public static class GitOrganizationProjectionHelper
{
    /// <summary>
    /// Adds the GitOrganization aggregate providers to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddGitOrganizationAggregateProviders(this IServiceCollection services)
    {
        _ = services
            .AddSingleton<IDomainAggregateProvider, DomainAggregateProvider<GitOrganization>>();
        return services;
    }

    /// <summary>
    /// Adds the GitOrganization projections to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddGitOrganizationProjectionHandlers(this IServiceCollection services)
        => services

            // Collection projections
            .AddScoped<IProjectionUpdateHandler<GitOrganizationAdded>, IdsCollectionProjectionHandler<GitOrganizationAdded>>()
            .AddScoped<IProjectionUpdateHandler<GitOrganizationSynced>, IdsCollectionProjectionHandler<GitOrganizationSynced>>()

            // Summary projections
            .AddScoped<IProjectionUpdateHandler<GitOrganizationAdded>, GitOrganizationAddedOnGitOrganizationSummaryProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<GitOrganizationSynced>, GitOrganizationSyncedOnGitOrganizationSummaryProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<GitOrganizationDescriptionChanged>, GitOrganizationDescriptionChangedOnGitOrganizationSummaryProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<GitOrganizationMarkedNotFound>, GitOrganizationMarkedNotFoundOnGitOrganizationSummaryProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<GitOrganizationDisabled>, GitOrganizationDisabledOnGitOrganizationSummaryProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<GitOrganizationEnabled>, GitOrganizationEnabledOnGitOrganizationSummaryProjectionHandler>()

            // Details projections
            .AddScoped<IProjectionUpdateHandler<GitOrganizationAdded>, GitOrganizationAddedOnGitOrganizationDetailsProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<GitOrganizationSynced>, GitOrganizationSyncedOnGitOrganizationDetailsProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<GitOrganizationDescriptionChanged>, GitOrganizationDescriptionChangedOnGitOrganizationDetailsProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<GitOrganizationMarkedNotFound>, GitOrganizationMarkedNotFoundOnGitOrganizationDetailsProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<GitOrganizationDisabled>, GitOrganizationDisabledOnGitOrganizationDetailsProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<GitOrganizationEnabled>, GitOrganizationEnabledOnGitOrganizationDetailsProjectionHandler>();

    /// <summary>
    /// Adds the GitOrganization projections and request handlers to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddGitOrganizationProjections(this IServiceCollection services)
        => services
        .AddGitOrganizationProjectionHandlers()
        .AddGitOrganizationRequestHandlers();

    /// <summary>
    /// Adds the GitOrganization request handlers to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddGitOrganizationRequestHandlers(this IServiceCollection services)
    {
        services.TryAddScoped<IRequestHandler<GetGitOrganizationDetails>, GetGitOrganizationDetailsHandler>();
        services.TryAddScoped<IRequestHandler<GetGitOrganizationSummaries>, GetFilteredCollectionHandler<GetGitOrganizationSummaries, GitOrganizationSummaryViewModel>>();
        return services;
    }
}
