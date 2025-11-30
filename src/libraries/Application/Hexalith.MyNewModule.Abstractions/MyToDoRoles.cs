// <copyright file="MyToDoRoles.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule;

/// <summary>
/// Defines the roles for MyToDo security within the application.
/// </summary>
public static class MyToDoRoles
{
    /// <summary>
    /// Role for users who can contribute to MyToDo.
    /// </summary>
    public const string Contributor = nameof(MyNewModule) + nameof(Contributor);

    /// <summary>
    /// Role for users who own MyToDo.
    /// </summary>
    public const string Owner = nameof(MyNewModule) + nameof(Owner);

    /// <summary>
    /// Role for users who can read MyToDo.
    /// </summary>
    public const string Reader = nameof(MyNewModule) + nameof(Reader);
}