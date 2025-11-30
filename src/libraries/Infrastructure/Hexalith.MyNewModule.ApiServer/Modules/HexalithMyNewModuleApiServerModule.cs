// <copyright file="HexalithMyNewModuleApiServerModule.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.ApiServer.Modules;

using System.Collections.Generic;

using Dapr.Actors.Runtime;

using Hexalith.Application.Modules.Modules;
using Hexalith.Application.Services;
using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.CosmosDb.Configurations;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Helpers;
using Hexalith.MyNewModule;
using Hexalith.MyNewModule.Aggregates;
using Hexalith.MyNewModule.ApiServer.Controllers;
using Hexalith.MyNewModule.Commands.Extensions;
using Hexalith.MyNewModule.Events.Extensions;
using Hexalith.MyNewModule.Helpers;
using Hexalith.MyNewModule.Requests.Extensions;
using Hexalith.MyNewModule.Requests.MyToDo;
using Hexalith.MyNewModule.Servers.Helpers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// The MyToDo construction site client module.
/// </summary>
public sealed class HexalithMyNewModuleApiServerModule : IApiServerApplicationModule, IMyToDoModule
{
    /// <inheritdoc/>
    public IDictionary<string, AuthorizationPolicy> AuthorizationPolicies => MyToDoModulePolicies.AuthorizationPolicies;

    /// <inheritdoc/>
    public IEnumerable<string> Dependencies => [];

    /// <inheritdoc/>
    public string Description => "Hexalith MyToDo API Server module";

    /// <inheritdoc/>
    public string Id => "Hexalith.MyNewModule.ApiServer";

    /// <inheritdoc/>
    public string Name => "Hexalith MyToDo API Server";

    /// <inheritdoc/>
    public int OrderWeight => 0;

    /// <inheritdoc/>
    public string Version => "1.0";

    /// <inheritdoc/>
    string IApplicationModule.Path => Path;

    private static string Path => nameof(MyToDo);

    /// <summary>
    /// Adds services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(services);
        _ = services
            .ConfigureSettings<CosmosDbSettings>(configuration);

        HexalithMyNewModuleEventsSerialization.RegisterPolymorphicMappers();
        HexalithMyNewModuleCommandsSerialization.RegisterPolymorphicMappers();
        HexalithMyNewModuleRequestsSerialization.RegisterPolymorphicMappers();

        // Add application module
        services.TryAddSingleton<IMyToDoModule, HexalithMyNewModuleApiServerModule>();

        // Add command handlers
        _ = services.AddMyToDo();

        // Add projection handlers and actor factories for event processing
        _ = services.AddMyToDoProjectionActorFactories();

        _ = services
         .AddControllers()
         .AddApplicationPart(typeof(MyToDoIntegrationEventsController).Assembly);
    }

    /// <summary>
    /// Registers the actors associated with the module.
    /// </summary>
    /// <param name="actorCollection">The actor collection.</param>
    /// <exception cref="ArgumentNullException">Thrown when actorCollection is null.</exception>
    /// <exception cref="ArgumentException">Thrown when actorCollection is not an ActorRegistrationCollection.</exception>
    public static void RegisterActors(object actorCollection)
    {
        ArgumentNullException.ThrowIfNull(actorCollection);
        if (actorCollection is not ActorRegistrationCollection actorRegistrations)
        {
            throw new ArgumentException($"{nameof(RegisterActors)} parameter must be an {nameof(ActorRegistrationCollection)}. Actual type : {actorCollection.GetType().Name}.", nameof(actorCollection));
        }

        actorRegistrations.RegisterActor<DomainAggregateActor>(MyToDoDomainHelper.MyToDoAggregateName.ToAggregateActorName());
        actorRegistrations.RegisterProjectionActor<MyToDo>();
        actorRegistrations.RegisterProjectionActor<MyToDoSummaryViewModel>();
        actorRegistrations.RegisterProjectionActor<MyToDoDetailsViewModel>();
        actorRegistrations.RegisterActor<SequentialStringListActor>(IIdCollectionFactory.GetAggregateCollectionName(MyToDoDomainHelper.MyToDoAggregateName));
    }

    /// <inheritdoc/>
    public void UseModule(object application)
    {
    }
}