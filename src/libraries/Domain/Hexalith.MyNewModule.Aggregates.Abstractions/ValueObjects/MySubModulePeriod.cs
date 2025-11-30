// <copyright file="MySubModulePeriod.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.Aggregates.ValueObjects;

using System.Runtime.Serialization;

/// <summary>
/// Represents a MyToDo period with a start and end date.
/// </summary>
/// <param name="StartDate">The start date of the MyToDo period.</param>
/// <param name="EndDate">The end date of the MyToDo period.</param>
[DataContract]
public record MySubModulePeriod(
    [property: DataMember(Order = 1)]
    DateOnly StartDate,
    [property: DataMember(Order = 2)]
    DateOnly EndDate);