// <copyright file="HexalithGitStorageApiServerModule.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.ApiServer.Modules;

using System.Collections.Generic;

using Dapr.Actors.Runtime;

using Hexalith.Application.Modules.Modules;
using Hexalith.Application.Services;
using Hexalith.Extensions.Configuration;
using Hexalith.GitStorage;
using Hexalith.GitStorage.Aggregates;
using Hexalith.GitStorage.ApiServer.Controllers;
using Hexalith.GitStorage.CommandHandlers;
using Hexalith.GitStorage.Commands.Extensions;
using Hexalith.GitStorage.Events.Extensions;
using Hexalith.GitStorage.Helpers;
using Hexalith.GitStorage.Projections.Helpers;
using Hexalith.GitStorage.Requests.Extensions;
using Hexalith.GitStorage.Requests.GitOrganization;
using Hexalith.GitStorage.Requests.GitStorageAccount;
using Hexalith.GitStorage.Servers.Helpers;
using Hexalith.Infrastructure.CosmosDb.Configurations;
using Hexalith.Infrastructure.DaprRuntime.Actors;
using Hexalith.Infrastructure.DaprRuntime.Helpers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// The GitStorageAccount construction site client module.
/// </summary>
public sealed class HexalithGitStorageApiServerModule : IApiServerApplicationModule, IGitStorageModule
{
    /// <inheritdoc/>
    public IDictionary<string, AuthorizationPolicy> AuthorizationPolicies => GitStorageModulePolicies.AuthorizationPolicies;

    /// <inheritdoc/>
    public IEnumerable<string> Dependencies => [];

    /// <inheritdoc/>
    public string Description => "Hexalith GitStorageAccount API Server module";

    /// <inheritdoc/>
    public string Id => "Hexalith.GitStorage.ApiServer";

    /// <inheritdoc/>
    public string Name => "Hexalith GitStorageAccount API Server";

    /// <inheritdoc/>
    public int OrderWeight => 0;

    /// <inheritdoc/>
    public string Version => "1.0";

    /// <inheritdoc/>
    string IApplicationModule.Path => Path;

    private static string Path => nameof(GitStorageAccount);

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

        HexalithGitStorageEventsSerialization.RegisterPolymorphicMappers();
        HexalithGitStorageCommandsSerialization.RegisterPolymorphicMappers();
        HexalithGitStorageRequestsSerialization.RegisterPolymorphicMappers();

        // Add application module
        services.TryAddSingleton<IGitStorageModule, HexalithGitStorageApiServerModule>();

        // Add command handlers
        _ = services.AddGitStorageAccount();
        _ = services.AddGitOrganizationCommandHandlers();
        _ = services.AddGitOrganizationAggregateProviders();

        // Add projection handlers and actor factories for event processing
        _ = services.AddGitStorageAccountProjectionActorFactories();
        _ = services.AddGitOrganizationProjectionActorFactories();

        _ = services
         .AddControllers()
         .AddApplicationPart(typeof(GitStorageAccountIntegrationEventsController).Assembly);
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

        // GitStorageAccount actors
        actorRegistrations.RegisterActor<DomainAggregateActor>(GitStorageAccountDomainHelper.GitStorageAccountAggregateName.ToAggregateActorName());
        actorRegistrations.RegisterProjectionActor<GitStorageAccount>();
        actorRegistrations.RegisterProjectionActor<GitStorageAccountSummaryViewModel>();
        actorRegistrations.RegisterProjectionActor<GitStorageAccountDetailsViewModel>();
        actorRegistrations.RegisterActor<SequentialStringListActor>(IIdCollectionFactory.GetAggregateCollectionName(GitStorageAccountDomainHelper.GitStorageAccountAggregateName));

        // GitOrganization actors
        actorRegistrations.RegisterActor<DomainAggregateActor>(GitOrganizationDomainHelper.GitOrganizationAggregateName.ToAggregateActorName());
        actorRegistrations.RegisterProjectionActor<GitOrganization>();
        actorRegistrations.RegisterProjectionActor<GitOrganizationSummaryViewModel>();
        actorRegistrations.RegisterProjectionActor<GitOrganizationDetailsViewModel>();
        actorRegistrations.RegisterActor<SequentialStringListActor>(IIdCollectionFactory.GetAggregateCollectionName(GitOrganizationDomainHelper.GitOrganizationAggregateName));
    }

    /// <inheritdoc/>
    public void UseModule(object application)
    {
    }
}