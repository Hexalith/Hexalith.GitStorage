// <copyright file="HexalithGitStorageWebServerApplication.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace HexalithApp.WebServer;

using System;
using System.Collections.Generic;

using Hexalith.Application.Modules.Applications;
using Hexalith.GitStorage;
using Hexalith.GitStorage.WebServer.Modules;
using Hexalith.Security.WebServer;
using Hexalith.UI.WebServer;

using HexalithApp.WebApp;

/// <summary>
/// Represents a server application.
/// </summary>
public class HexalithGitStorageWebServerApplication : HexalithWebServerApplication
{
    /// <inheritdoc/>
    public override string Id => $"{HexalithGitStorageInformation.Id}.{ApplicationType}";

    /// <inheritdoc/>
    public override string Name => $"{HexalithGitStorageInformation.Name} {ApplicationType}";

    /// <inheritdoc/>
    public override string ShortName => HexalithGitStorageInformation.ShortName;

    /// <inheritdoc/>
    public override Type WebAppApplicationType => typeof(HexalithGitStorageWebAppApplication);

    /// <inheritdoc/>
    public override IEnumerable<Type> WebServerModules => [
        typeof(HexalithUIComponentsWebServerModule),
        typeof(HexalithSecurityWebServerModule),
        typeof(HexalithGitStorageAccountWebServerModule)];
}