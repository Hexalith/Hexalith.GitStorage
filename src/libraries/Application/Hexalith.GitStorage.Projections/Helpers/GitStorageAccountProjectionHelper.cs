// <copyright file="GitStorageAccountProjectionHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.GitStorage.Projections.Helpers;

using Hexalith.Application.Aggregates;
using Hexalith.Application.Projections;
using Hexalith.Application.Requests;
using Hexalith.Domain.Events;
using Hexalith.GitStorage.Aggregates;
using Hexalith.GitStorage.Events.GitStorageAccount;
using Hexalith.GitStorage.Projections.ProjectionHandlers.Details;
using Hexalith.GitStorage.Projections.ProjectionHandlers.Summaries;
using Hexalith.GitStorage.Projections.RequestHandlers;
using Hexalith.GitStorage.Requests.GitStorageAccount;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Helper class for adding GitStorageAccount projections to the service collection.
/// </summary>
public static class GitStorageAccountProjectionHelper
{
    /// <summary>
    /// Adds the GitStorageAccount aggregate providers to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddGitStorageAccountAggregateProviders(this IServiceCollection services)
    {
        _ = services
            .AddSingleton<IDomainAggregateProvider, DomainAggregateProvider<GitStorageAccount>>();
        return services;
    }

    /// <summary>
    /// Adds the GitStorageAccount projections to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddGitStorageAccountProjectionHandlers(this IServiceCollection services)
        => services

            // Collection projections
            .AddScoped<IProjectionUpdateHandler<GitStorageAccountAdded>, IdsCollectionProjectionHandler<GitStorageAccountAdded>>()

            // Summary projections
            .AddScoped<IProjectionUpdateHandler<GitStorageAccountAdded>, GitStorageAccountAddedOnSummaryProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<GitStorageAccountDescriptionChanged>, GitStorageAccountDescriptionChangedOnSummaryProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<GitStorageAccountDisabled>, GitStorageAccountDisabledOnSummaryProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<GitStorageAccountEnabled>, GitStorageAccountEnabledOnSummaryProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<SnapshotEvent>, GitStorageAccountSnapshotOnSummaryProjectionHandler>()

            // Details
            .AddScoped<IProjectionUpdateHandler<GitStorageAccountAdded>, GitStorageAccountAddedOnDetailsProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<GitStorageAccountDescriptionChanged>, GitStorageAccountDescriptionChangedOnDetailsProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<GitStorageAccountDisabled>, GitStorageAccountDisabledOnDetailsProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<GitStorageAccountEnabled>, GitStorageAccountEnabledOnDetailsProjectionHandler>();

    /// <summary>
    /// Adds the GitStorageAccount projections and request handlers to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddGitStorageAccountProjections(this IServiceCollection services)
        => services
        .AddGitStorageAccountProjectionHandlers()
        .AddGitStorageAccountRequestHandlers();

    /// <summary>
    /// Adds the GitStorageAccount query services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddGitStorageAccountQueryServices(this IServiceCollection services)
    => services;

    /// <summary>
    /// Adds the GitStorageAccount request handlers to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddGitStorageAccountRequestHandlers(this IServiceCollection services)
    {
        services.TryAddScoped<IRequestHandler<GetGitStorageAccountDetails>, GetGitStorageAccountDetailsHandler>();
        services.TryAddScoped<IRequestHandler<GetGitStorageAccountSummaries>, GetFilteredCollectionHandler<GetGitStorageAccountSummaries, GitStorageAccountSummaryViewModel>>();
        services.TryAddScoped<IRequestHandler<GetGitStorageAccountIds>, GetAggregateIdsRequestHandler<GetGitStorageAccountIds>>();
        services.TryAddScoped<IRequestHandler<GetGitStorageAccountExports>, GetExportsRequestHandler<GetGitStorageAccountExports, GitStorageAccount>>();
        return services;
    }
}