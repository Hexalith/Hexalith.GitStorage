// <copyright file="MyToDoEnabled.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Events.MyToDo;

using Hexalith.PolymorphicSerializations;

/// <summary>
/// Event raised when a mytodo is enabled.
/// </summary>
/// <param name="Id">The identifier of the mytodo.</param>
[PolymorphicSerialization]
public partial record MyToDoEnabled(string Id) : MyToDoEvent(Id);