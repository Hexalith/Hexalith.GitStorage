// <copyright file="GitRepositoryRequest.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Requests.GitRepository;

using System.Runtime.Serialization;

using Hexalith.GitStorage.Aggregates;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Represents a base class for GitRepository requests.
/// </summary>
/// <param name="Id">The aggregate ID of the GitRepository request.</param>
[PolymorphicSerialization]
public abstract partial record GitRepositoryRequest([property: DataMember(Order = 1)] string Id)
{
    /// <summary>
    /// Gets the aggregate ID of the GitRepository request.
    /// </summary>
    public string AggregateId => Id;

    /// <summary>
    /// Gets the aggregate name of the GitRepository request.
    /// </summary>
    public static string AggregateName => GitRepositoryDomainHelper.GitRepositoryAggregateName;
}
