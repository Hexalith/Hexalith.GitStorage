// <copyright file="HexalithMyNewModuleWebAppModule.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.WebApp.Modules;

using System.Collections.Generic;
using System.Reflection;

using Hexalith.Application.Modules.Modules;
using Hexalith.MyNewModule;
using Hexalith.MyNewModule.Commands.Extensions;
using Hexalith.MyNewModule.Events.Extensions;
using Hexalith.MyNewModule.Helpers;
using Hexalith.MyNewModule.Projections.Helpers;
using Hexalith.MyNewModule.Requests.Extensions;
using Hexalith.MyNewModule.UI.Pages.Modules;

using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

/// <summary>
/// The MyToDo construction site client module.
/// </summary>
public class HexalithMyNewModuleWebAppModule : IWebAppApplicationModule, IMyToDoModule
{
    /// <inheritdoc/>
    public IDictionary<string, AuthorizationPolicy> AuthorizationPolicies => MyToDoModulePolicies.AuthorizationPolicies;

    /// <inheritdoc/>
    public IEnumerable<string> Dependencies => [];

    /// <inheritdoc/>
    public string Description => "MyToDo client module";

    /// <inheritdoc/>
    public string Id => "Hexalith.MyNewModule.Client";

    /// <inheritdoc/>
    public string Name => "MyToDo client";

    /// <inheritdoc/>
    public int OrderWeight => 0;

    /// <inheritdoc/>
    public string Path => nameof(MyNewModule);

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
        HexalithMyNewModuleEventsSerialization.RegisterPolymorphicMappers();
        HexalithMyNewModuleCommandsSerialization.RegisterPolymorphicMappers();
        HexalithMyNewModuleRequestsSerialization.RegisterPolymorphicMappers();

        // Add application module
        services.TryAddSingleton<IMyToDoModule, HexalithMyNewModuleWebAppModule>();

        _ = services
            .AddMyToDoQueryServices()
            .AddTransient(_ => MyNewModuleMenu.Menu);
    }

    /// <inheritdoc/>
    public void UseModule(object application)
    {
    }
}