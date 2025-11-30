// <copyright file="MyNewModuleQuickStartData.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Projections.Services;

using Hexalith.MyNewModule.Commands.MyToDo;

/// <summary>
/// Represents the details of a mytodo.
/// </summary>
public static class MyNewModuleQuickStartData
{
    /// <summary>
    /// Gets the collection of mytodo details.
    /// </summary>
    public static IEnumerable<AddMyToDo> Data => [M1, M2, M3, M4];

    /// <summary>
    /// Gets the M1 demo data.
    /// </summary>
    public static AddMyToDo M1 => new("M1", "Module 1", "this is module 1");

    /// <summary>
    /// Gets the M2 demo data.
    /// </summary>
    public static AddMyToDo M2 => new("M2", "Module 2", "this is module 2");

    /// <summary>
    /// Gets the M3 demo data.
    /// </summary>
    public static AddMyToDo M3 => new("M3", "Module 3", "this is module 3");

    /// <summary>
    /// Gets the M4 demo data.
    /// </summary>
    public static AddMyToDo M4 => new("M4", "Module 4", "this is module 4");
}