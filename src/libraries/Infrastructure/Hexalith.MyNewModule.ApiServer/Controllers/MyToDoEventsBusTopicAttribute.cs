// <copyright file="MyToDoEventsBusTopicAttribute.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.MyNewModule.ApiServer.Controllers;

using Hexalith.Infrastructure.WebApis.Buses;
using Hexalith.MyNewModule.Aggregates;

/// <summary>
/// Class CustomerEventsBusTopicAttribute. This class cannot be inherited.
/// Implements the <see cref="EventBusTopicAttribute" />.
/// </summary>
/// <seealso cref="EventBusTopicAttribute" />
[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public sealed class MyToDoEventsBusTopicAttribute : EventBusTopicAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MyToDoEventsBusTopicAttribute"/> class.
    /// </summary>
    public MyToDoEventsBusTopicAttribute()
        : base(MyToDoDomainHelper.MyToDoAggregateName)
    {
    }
}