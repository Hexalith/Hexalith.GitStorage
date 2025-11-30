// <copyright file="GitStorageAccountEventsBusTopicAttribute.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.ApiServer.Controllers;

using Hexalith.Infrastructure.WebApis.Buses;
using Hexalith.GitStorage.Aggregates;

/// <summary>
/// Class CustomerEventsBusTopicAttribute. This class cannot be inherited.
/// Implements the <see cref="EventBusTopicAttribute" />.
/// </summary>
/// <seealso cref="EventBusTopicAttribute" />
[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public sealed class GitStorageAccountEventsBusTopicAttribute : EventBusTopicAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GitStorageAccountEventsBusTopicAttribute"/> class.
    /// </summary>
    public GitStorageAccountEventsBusTopicAttribute()
        : base(GitStorageAccountDomainHelper.GitStorageAccountAggregateName)
    {
    }
}