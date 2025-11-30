// <copyright file="MyToDoProjectionHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Projections.Helpers;

using Hexalith.Application.Aggregates;
using Hexalith.Application.Projections;
using Hexalith.Application.Requests;
using Hexalith.Domain.Events;
using Hexalith.MyNewModule.Aggregates;
using Hexalith.MyNewModule.Events.MyToDo;
using Hexalith.MyNewModule.Projections.ProjectionHandlers.Details;
using Hexalith.MyNewModule.Projections.ProjectionHandlers.Summaries;
using Hexalith.MyNewModule.Projections.RequestHandlers;
using Hexalith.MyNewModule.Requests.MyToDo;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// Helper class for adding mytodo projections to the service collection.
/// </summary>
public static class MyToDoProjectionHelper
{
    /// <summary>
    /// Adds the mytodo aggregate providers to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMyToDoAggregateProviders(this IServiceCollection services)
    {
        _ = services
            .AddSingleton<IDomainAggregateProvider, DomainAggregateProvider<MyToDo>>();
        return services;
    }

    /// <summary>
    /// Adds the mytodo projections to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMyToDoProjectionHandlers(this IServiceCollection services)
        => services

            // Collection projections
            .AddScoped<IProjectionUpdateHandler<MyToDoAdded>, IdsCollectionProjectionHandler<MyToDoAdded>>()

            // Summary projections
            .AddScoped<IProjectionUpdateHandler<MyToDoAdded>, MyToDoAddedOnSummaryProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<MyToDoDescriptionChanged>, MyToDoDescriptionChangedOnSummaryProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<MyToDoDisabled>, MyToDoDisabledOnSummaryProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<MyToDoEnabled>, MyToDoEnabledOnSummaryProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<SnapshotEvent>, MyToDoSnapshotOnSummaryProjectionHandler>()

            // Details
            .AddScoped<IProjectionUpdateHandler<MyToDoAdded>, MyToDoAddedOnDetailsProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<MyToDoDescriptionChanged>, MyToDoDescriptionChangedOnDetailsProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<MyToDoDisabled>, MyToDoDisabledOnDetailsProjectionHandler>()
            .AddScoped<IProjectionUpdateHandler<MyToDoEnabled>, MyToDoEnabledOnDetailsProjectionHandler>();

    /// <summary>
    /// Adds the mytodo projections and request handlers to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMyToDoProjections(this IServiceCollection services)
        => services
        .AddMyToDoProjectionHandlers()
        .AddMyToDoRequestHandlers();

    /// <summary>
    /// Adds the mytodo query services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMyToDoQueryServices(this IServiceCollection services)
    => services;

    /// <summary>
    /// Adds the mytodo request handlers to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMyToDoRequestHandlers(this IServiceCollection services)
    {
        services.TryAddScoped<IRequestHandler<GetMyToDoDetails>, GetMyToDoDetailsHandler>();
        services.TryAddScoped<IRequestHandler<GetMyToDoSummaries>, GetFilteredCollectionHandler<GetMyToDoSummaries, MyToDoSummaryViewModel>>();
        services.TryAddScoped<IRequestHandler<GetMyToDoIds>, GetAggregateIdsRequestHandler<GetMyToDoIds>>();
        services.TryAddScoped<IRequestHandler<GetMyToDoExports>, GetExportsRequestHandler<GetMyToDoExports, MyToDo>>();
        return services;
    }
}