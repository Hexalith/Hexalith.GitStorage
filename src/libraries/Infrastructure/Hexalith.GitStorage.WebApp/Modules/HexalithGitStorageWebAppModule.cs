// <copyright file="HexalithGitStorageWebAppModule.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.WebApp.Modules;

using System.Collections.Generic;
using System.Reflection;

using Hexalith.Application.Modules.Modules;
using Hexalith.GitStorage;
using Hexalith.GitStorage.Commands.Extensions;
using Hexalith.GitStorage.Events.Extensions;
using Hexalith.GitStorage.Helpers;
using Hexalith.GitStorage.Projections.Helpers;
using Hexalith.GitStorage.Requests.Extensions;
using Hexalith.GitStorage.UI.Pages.Modules;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// The GitStorageAccount construction site client module.
/// </summary>
public class HexalithGitStorageWebAppModule : IWebAppApplicationModule, IGitStorageModule
{
    /// <inheritdoc/>
    public IDictionary<string, AuthorizationPolicy> AuthorizationPolicies => GitStorageModulePolicies.AuthorizationPolicies;

    /// <inheritdoc/>
    public IEnumerable<string> Dependencies => [];

    /// <inheritdoc/>
    public string Description => "GitStorageAccount client module";

    /// <inheritdoc/>
    public string Id => "Hexalith.GitStorage.Client";

    /// <inheritdoc/>
    public string Name => "GitStorageAccount client";

    /// <inheritdoc/>
    public int OrderWeight => 0;

    /// <inheritdoc/>
    public string Path => nameof(GitStorage);

    /// <inheritdoc/>
    public IEnumerable<Assembly> PresentationAssemblies => [
        GetType().Assembly,
        typeof(UI.Components._Imports).Assembly,
        typeof(UI.Pages._Imports).Assembly,
    ];

    /// <inheritdoc/>
    public string Version => "1.0";

    /// <summary>
    /// Adds services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public static void AddServices(IServiceCollection services)
    {
        HexalithGitStorageEventsSerialization.RegisterPolymorphicMappers();
        HexalithGitStorageCommandsSerialization.RegisterPolymorphicMappers();
        HexalithGitStorageRequestsSerialization.RegisterPolymorphicMappers();

        // Add application module
        services.TryAddSingleton<IGitStorageModule, HexalithGitStorageWebAppModule>();

        _ = services
            .AddGitStorageAccountQueryServices()
            .AddTransient(_ => GitStorageMenu.Menu);
    }

    /// <inheritdoc/>
    public void UseModule(object application)
    {
    }
}