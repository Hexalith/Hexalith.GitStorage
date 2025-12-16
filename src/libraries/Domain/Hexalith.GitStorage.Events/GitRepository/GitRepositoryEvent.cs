// <copyright file="GitRepositoryEvent.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Events.GitRepository;

using Hexalith.GitStorage.Aggregates;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Base event for GitRepository operations.
/// </summary>
/// <param name="Id">The identifier of the GitRepository (composite key: {OrganizationId}-{RepositoryName}).</param>
[PolymorphicSerialization]
public abstract partial record GitRepositoryEvent(string Id)
{
    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    public string AggregateId => Id;

    /// <summary>
    /// Gets the aggregate name.
    /// </summary>
    public static string AggregateName => GitRepositoryDomainHelper.GitRepositoryAggregateName;
}
