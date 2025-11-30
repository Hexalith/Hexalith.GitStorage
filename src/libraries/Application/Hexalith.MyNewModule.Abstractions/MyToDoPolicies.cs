// <copyright file="MyToDoPolicies.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule;

/// <summary>
/// Defines the policies for MyToDo security within the application.
/// </summary>
public static class MyToDoPolicies
{
    /// <summary>
    /// Policy for users who can contribute to MyToDo.
    /// </summary>
    public const string Contributor = MyToDoRoles.Contributor;

    /// <summary>
    /// Policy for users who own MyToDo.
    /// </summary>
    public const string Owner = MyToDoRoles.Owner;

    /// <summary>
    /// Policy for users who can read MyToDo.
    /// </summary>
    public const string Reader = MyToDoRoles.Reader;
}