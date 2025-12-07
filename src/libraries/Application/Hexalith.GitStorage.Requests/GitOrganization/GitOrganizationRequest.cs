// <copyright file="GitOrganizationRequest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Requests.GitOrganization;

using System.Runtime.Serialization;

using Hexalith.GitStorage.Aggregates;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents a base class for GitOrganization requests.
/// </summary>
/// <param name="Id">The aggregate ID of the GitOrganization request.</param>
[PolymorphicSerialization]
public abstract partial record GitOrganizationRequest([property: DataMember(Order = 1)] string Id)
{
    /// <summary>
    /// Gets the aggregate ID of the GitOrganization request.
    /// </summary>
    public string AggregateId => Id;

    /// <summary>
    /// Gets the aggregate name of the GitOrganization request.
    /// </summary>
    public static string AggregateName => GitOrganizationDomainHelper.GitOrganizationAggregateName;
}
