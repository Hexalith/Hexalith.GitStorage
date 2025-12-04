// <copyright file="HexalithGitStorageAccountWebServerModule.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.WebServer.Modules;

using System.Collections.Generic;
using System.Reflection;

using Dapr.Actors.Runtime;

using Hexalith.Application.Modules.Modules;
using Hexalith.Extensions.Configuration;
using Hexalith.GitStorage.CommandHandlers;
using Hexalith.GitStorage.Commands.Extensions;
using Hexalith.GitStorage.EventHandlers;
using Hexalith.GitStorage.Events.Extensions;
using Hexalith.GitStorage.Helpers;
using Hexalith.GitStorage.Projections.Helpers;
using Hexalith.GitStorage.Requests.Extensions;
using Hexalith.GitStorage.Servers.Helpers;
using Hexalith.GitStorage.UI.Pages.Modules;
using Hexalith.GitStorage.WebServer.Controllers;
using Hexalith.Infrastructure.CosmosDb.Configurations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// The GitStorage construction site client module.
/// </summary>
public sealed class HexalithGitStorageAccountWebServerModule : IWebServerApplicationModule, IGitStorageAccountModule
{
    /// <inheritdoc/>
    public IDictionary<string, AuthorizationPolicy> AuthorizationPolicies => GitStorageAccountModulePolicies.AuthorizationPolicies;

    /// <inheritdoc/>
    public IEnumerable<string> Dependencies => [];

    /// <inheritdoc/>
    public string Description => "GitStorage server module";

    /// <inheritdoc/>
    public string Id => "GitStorage.Server";

    /// <inheritdoc/>
    public string Name => "Hexalith GitStorage server";

    /// <inheritdoc/>
    public int OrderWeight => 0;

    /// <inheritdoc/>
    public IEnumerable<Assembly> PresentationAssemblies => [
        GetType().Assembly,
        typeof(UI.Components._Imports).Assembly,
        typeof(UI.Pages._Imports).Assembly,
    ];

    /// <inheritdoc/>
    public string Version => "1.0";

    /// <inheritdoc/>
    string IApplicationModule.Path => Path;

    private static string Path => nameof(GitStorage);

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

        _ = services
            .AddGitStorageAccountCommandHandlers()
            .AddGitStorageAccountEventValidators()
            .AddGitStorageAccountProjectionActorFactories()
            .AddGitStorageAccountRequestHandlers()
            .AddGitStorageAccountProjections()
            .AddGitOrganizationProjectionActorFactories();

        HexalithGitStorageEventsSerialization.RegisterPolymorphicMappers();
        HexalithGitStorageCommandsSerialization.RegisterPolymorphicMappers();
        HexalithGitStorageRequestsSerialization.RegisterPolymorphicMappers();

        // Add application module
        services.TryAddSingleton<IGitStorageAccountModule, HexalithGitStorageAccountWebServerModule>();

        _ = services.AddTransient(_ => GitStorageMenu.Menu);
        _ = services.AddControllers().AddApplicationPart(typeof(GitStorageController).Assembly);
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
        if (actorCollection is not ActorRegistrationCollection)
        {
            throw new ArgumentException($"{nameof(RegisterActors)} parameter must be an {nameof(ActorRegistrationCollection)}. Actual type : {actorCollection.GetType().Name}.", nameof(actorCollection));
        }
    }

    /// <inheritdoc/>
    public void UseModule(object application)
    {
    }

    /// <inheritdoc/>
    public void UseSecurity(object application)
    {
    }
}