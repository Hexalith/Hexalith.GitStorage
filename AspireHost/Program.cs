// <copyright file="Program.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
using Hexalith.NetAspire.Hosting.Helpers;

using Microsoft.Extensions.Configuration;

HexalithDistributedApplication app = new(args);

app.Builder.AddForwardedHeaders();

app.Builder.Configuration.AddUserSecrets<Program>();

if (app.IsProjectEnabled<Projects.HexalithApp_WebServer>())
{
    _ = app
        .AddProject<Projects.HexalithApp_WebServer>("GitStorageweb")
        .WithEnvironmentFromConfiguration("APP_API_TOKEN")
        .WithEnvironmentFromConfiguration("Hexalith__IdentityStores__Microsoft__Id", false)
        .WithEnvironmentFromConfiguration("Hexalith__IdentityStores__Microsoft__Secret", false)
        .WithEnvironmentFromConfiguration("Hexalith__IdentityStores__Microsoft__Tenant", false)
        .WithEnvironmentFromConfiguration("Hexalith__IdentityStores__Google__Id", false)
        .WithEnvironmentFromConfiguration("Hexalith__IdentityStores__Google__Secret", false)
        .WithEnvironmentFromConfiguration("Hexalith__IdentityStores__Github__Id", false)
        .WithEnvironmentFromConfiguration("Hexalith__IdentityStores__Github__Secret", false)
        .WithEnvironmentFromConfiguration("Hexalith__IdentityStores__Facebook__Id", false)
        .WithEnvironmentFromConfiguration("Hexalith__IdentityStores__Facebook__Secret", false)
        .WithEnvironmentFromConfiguration("Hexalith__IdentityStores__X__Id", false)
        .WithEnvironmentFromConfiguration("Hexalith__IdentityStores__X__Secret", false)
        .WithEnvironmentFromConfiguration("EmailServer__ApplicationSecret")
        .WithEnvironmentFromConfiguration("EmailServer__FromEmail")
        .WithEnvironmentFromConfiguration("EmailServer__FromName");
}

if (app.IsProjectEnabled<Projects.HexalithApp_ApiServer>())
{
    _ = app
        .AddProject<Projects.HexalithApp_ApiServer>("GitStorageapi")
        .WithEnvironmentFromConfiguration("Hexalith__IdentityStores__Microsoft__Id", false)
        .WithEnvironmentFromConfiguration("Hexalith__IdentityStores__Microsoft__Secret", false)
        .WithEnvironmentFromConfiguration("Hexalith__IdentityStores__Microsoft__TenantId", false)
        .WithEnvironmentFromConfiguration("EmailServer__ApplicationSecret")
        .WithEnvironmentFromConfiguration("EmailServer__FromEmail")
        .WithEnvironmentFromConfiguration("EmailServer__FromName");
}

await app
    .Builder
    .Build()
    .RunAsync()
    .ConfigureAwait(false);