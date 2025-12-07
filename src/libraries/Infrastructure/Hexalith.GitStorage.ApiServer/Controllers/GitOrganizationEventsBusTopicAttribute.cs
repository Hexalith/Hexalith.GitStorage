// <copyright file="GitOrganizationEventsBusTopicAttribute.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.ApiServer.Controllers;

using Hexalith.GitStorage.Aggregates;
using Hexalith.Infrastructure.WebApis.Buses;

/// <summary>
/// Attribute to specify the event bus topic for GitOrganization events.
/// Implements the <see cref="EventBusTopicAttribute" />.
/// </summary>
/// <seealso cref="EventBusTopicAttribute" />
[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public sealed class GitOrganizationEventsBusTopicAttribute : EventBusTopicAttribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GitOrganizationEventsBusTopicAttribute"/> class.
    /// </summary>
    public GitOrganizationEventsBusTopicAttribute()
        : base(GitOrganizationDomainHelper.GitOrganizationAggregateName)
    {
    }
}
