// <copyright file="HexalithMyNewModuleWebAppApplication.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace HexalithApp.WebApp;

using System;
using System.Collections.Generic;

using Hexalith.Application.Modules.Applications;
using Hexalith.MyNewModule;
using Hexalith.MyNewModule.WebApp.Modules;
using Hexalith.Security.WebApp;
using Hexalith.UI.WebApp;

/// <summary>
/// Represents a client application.
/// </summary>
public class HexalithMyNewModuleWebAppApplication : HexalithWebAppApplication
{
    /// <inheritdoc/>
    public override string Id => $"{HexalithMyNewModuleInformation.Id}.{ApplicationType}";

    /// <inheritdoc/>
    public override string Name => $"{HexalithMyNewModuleInformation.Name} {ApplicationType}";

    /// <inheritdoc/>
    public override string ShortName => HexalithMyNewModuleInformation.ShortName;

    /// <inheritdoc/>
    public override IEnumerable<Type> WebAppModules
        => [
            typeof(HexalithUIComponentsWebAppModule),
            typeof(HexalithSecurityWebAppModule),
            typeof(HexalithMyNewModuleWebAppModule)];
}