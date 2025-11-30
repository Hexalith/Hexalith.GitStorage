// <copyright file="MyToDoWebApiHelpers.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Servers.Helpers;

using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.MyNewModule.Aggregates;
using Hexalith.MyNewModule.Projections.Helpers;
using Hexalith.MyNewModule.Requests.MyToDo;

using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Class MyToDoWebApiHelpers.
/// </summary>
public static class MyToDoWebApiHelpers
{
    /// <summary>
    /// Adds the MyToDo projection actor factories.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>IServiceCollection.</returns>
    /// <exception cref="ArgumentNullException">Thrown when services is null.</exception>
    public static IServiceCollection AddMyToDoProjectionActorFactories(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);
        _ = services.AddMyToDoProjections();
        _ = services.AddActorProjectionFactory<MyToDoSummaryViewModel>();
        _ = services.AddActorProjectionFactory<MyToDoDetailsViewModel>();
        _ = services.AddActorProjectionFactory<MyToDo>();
        return services;
    }
}