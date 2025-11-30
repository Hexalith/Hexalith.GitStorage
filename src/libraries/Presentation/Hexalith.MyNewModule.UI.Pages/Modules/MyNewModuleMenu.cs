// <copyright file="MyNewModuleMenu.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.UI.Pages.Modules;

using Hexalith.MyNewModule;

using Hexalith.UI.Components;
using Hexalith.UI.Components.Icons;

using Labels = Localizations.MyNewModuleMenu;

/// <summary>
/// Represents the MyToDo menu.
/// </summary>
public static class MyNewModuleMenu
{
    /// <summary>
    /// Gets the menu information.
    /// </summary>
    public static MenuItemInformation Menu => new(
                    Labels.MyNewModuleMenuItem,
                    string.Empty,
                    new IconInformation("Apps", 20, IconStyle.Regular, IconSource.Fluent, IconLibraryName),
                    true,
                    10,
                    MyToDoRoles.Reader,
                    [
                        new MenuItemInformation(
                            Labels.MyToDoMenuItem,
                            "MyToDo/MyToDo",
                            new IconInformation("AppsAddIn", 20, IconStyle.Regular, IconSource.Fluent, IconLibraryName),
                            false,
                            30,
                            MyToDoRoles.Reader,
                            []),
                    ]);

    private static string IconLibraryName
        => typeof(MyNewModuleMenu).Assembly?.FullName
            ?? throw new InvalidOperationException("Menu Assembly not found");
}