// <copyright file="GetMyToDoIdDescription.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Requests.MyToDo;

using System.Runtime.Serialization;

using Hexalith.Domains.ValueObjects;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents a request to get the identifier and description of a warehouse by its ID.
/// </summary>
/// <param name="Id">The ID of the warehouse.</param>
/// <param name="Result">The result containing the identifier and description.</param>
[PolymorphicSerialization]
public partial record GetMyToDoIdDescription(string Id, [property: DataMember(Order = 2)] IdDescription? Result = null)
    : MyToDoRequest(Id);