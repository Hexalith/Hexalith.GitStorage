// <copyright file="MyToDoEvent.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Events.MyToDo;

using Hexalith.MyNewModule.Aggregates;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Base event for mytodo operations.
/// </summary>
/// <param name="Id">The identifier of the mytodo.</param>
[PolymorphicSerialization]
public abstract partial record MyToDoEvent(string Id)
{
    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    public string AggregateId => Id;

    /// <summary>
    /// Gets the aggregate name.
    /// </summary>
    public static string AggregateName => MyToDoDomainHelper.MyToDoAggregateName;
}