// <copyright file="GitRepositoryCommand.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Commands.GitRepository;

using Hexalith.GitStorage.Aggregates;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Base command for GitRepository operations.
/// </summary>
/// <param name="Id">The identifier of the GitRepository (composite key: {OrganizationId}-{RepositoryName}).</param>
[PolymorphicSerialization]
public abstract partial record GitRepositoryCommand(string Id)
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
