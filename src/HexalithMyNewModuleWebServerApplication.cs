// <copyright file="HexalithMyNewModuleWebServerApplication.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace HexalithApp.WebServer;

using System;
using System.Collections.Generic;

using Hexalith.Application.Modules.Applications;
using Hexalith.MyNewModule;
using Hexalith.MyNewModule.WebServer.Modules;
using Hexalith.Security.WebServer;
using Hexalith.UI.WebServer;

using HexalithApp.WebApp;

/// <summary>
/// Represents a server application.
/// </summary>
public class HexalithMyNewModuleWebServerApplication : HexalithWebServerApplication
{
    /// <inheritdoc/>
    public override string Id => $"{HexalithMyNewModuleInformation.Id}.{ApplicationType}";

    /// <inheritdoc/>
    public override string Name => $"{HexalithMyNewModuleInformation.Name} {ApplicationType}";

    /// <inheritdoc/>
    public override string ShortName => HexalithMyNewModuleInformation.ShortName;

    /// <inheritdoc/>
    public override Type WebAppApplicationType => typeof(HexalithMyNewModuleWebAppApplication);

    /// <inheritdoc/>
    public override IEnumerable<Type> WebServerModules => [
        typeof(HexalithUIComponentsWebServerModule),
        typeof(HexalithSecurityWebServerModule),
        typeof(HexalithMyToDoWebServerModule)];
}