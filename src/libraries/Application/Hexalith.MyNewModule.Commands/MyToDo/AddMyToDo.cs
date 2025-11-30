// <copyright file="AddMyToDo.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Commands.MyToDo;

using System.Runtime.Serialization;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Command raised when a new mytodo is added.
/// </summary>
/// <param name="Id">The identifier of the mytodo.</param>
/// <param name="Name">The name of the mytodo.</param>
/// <param name="Comments">Optional comments about the mytodo.</param>
[PolymorphicSerialization]
public partial record AddMyToDo(
    string Id,
    [property: DataMember(Order = 2)] string Name,
    [property: DataMember(Order = 3)] string? Comments)
    : MyToDoCommand(Id);