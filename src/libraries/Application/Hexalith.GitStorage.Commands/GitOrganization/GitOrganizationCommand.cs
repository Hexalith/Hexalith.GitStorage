// <copyright file="GitOrganizationCommand.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Commands.GitOrganization;

using Hexalith.GitStorage.Aggregates;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Base command for GitOrganization operations.
/// </summary>
/// <param name="Id">The identifier of the GitOrganization (composite key: {GitStorageAccountId}-{OrganizationName}).</param>
[PolymorphicSerialization]
public abstract partial record GitOrganizationCommand(string Id)
{
    /// <summary>
    /// Gets the aggregate identifier.
    /// </summary>
    public string AggregateId => Id;

    /// <summary>
    /// Gets the aggregate name.
    /// </summary>
    public static string AggregateName => GitOrganizationDomainHelper.GitOrganizationAggregateName;
}
