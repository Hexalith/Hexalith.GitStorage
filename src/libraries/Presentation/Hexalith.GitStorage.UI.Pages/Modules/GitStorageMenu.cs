// <copyright file="GitStorageMenu.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.UI.Pages.Modules;

using Hexalith.GitStorage;

using Hexalith.UI.Components;
using Hexalith.UI.Components.Icons;

using Labels = Localizations.GitStorageMenu;

/// <summary>
/// Represents the GitStorageAccount menu.
/// </summary>
public static class GitStorageMenu
{
    /// <summary>
    /// Gets the menu information.
    /// </summary>
    public static MenuItemInformation Menu => new(
                    Labels.GitStorageMenuItem,
                    string.Empty,
                    new IconInformation("Apps", 20, IconStyle.Regular, IconSource.Fluent, IconLibraryName),
                    true,
                    10,
                    GitStorageRoles.Reader,
                    [
                        new MenuItemInformation(
                            Labels.GitStorageAccountMenuItem,
                            "GitStorageAccount/GitStorageAccount",
                            new IconInformation("AppsAddIn", 20, IconStyle.Regular, IconSource.Fluent, IconLibraryName),
                            false,
                            30,
                            GitStorageRoles.Reader,
                            []),
                        new MenuItemInformation(
                            Labels.GitOrganizationMenuItem,
                            "GitOrganization/GitOrganization",
                            new IconInformation("Organization", 20, IconStyle.Regular, IconSource.Fluent, IconLibraryName),
                            false,
                            40,
                            GitStorageRoles.Reader,
                            []),
                    ]);

    private static string IconLibraryName
        => typeof(GitStorageMenu).Assembly?.FullName
            ?? throw new InvalidOperationException("Menu Assembly not found");
}