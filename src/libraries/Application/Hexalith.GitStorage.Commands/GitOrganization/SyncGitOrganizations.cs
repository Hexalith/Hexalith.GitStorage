// <copyright file="SyncGitOrganizations.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Commands.GitOrganization;

using System.Runtime.Serialization;

using Hexalith.GitStorage.Aggregates;
using Hexalith.PolymorphicSerializations;

/// <summary>
/// Command to trigger synchronization of organizations from a GitStorageAccount.
/// This is a bulk operation command where AggregateId is the GitStorageAccountId being synced.
/// The handler iterates remote orgs and emits individual GitOrganizationSynced/GitOrganizationMarkedNotFound events per organization.
/// </summary>
/// <param name="GitStorageAccountId">The Git Storage Account ID to sync organizations from.</param>
[PolymorphicSerialization]
public partial record SyncGitOrganizations(
    [property: DataMember(Order = 1)] string GitStorageAccountId)
{
    /// <summary>
    /// Gets the aggregate identifier (the GitStorageAccount being synced).
    /// </summary>
    public string AggregateId => GitStorageAccountId;

    /// <summary>
    /// Gets the aggregate name (GitStorageAccount as this command operates on accounts).
    /// </summary>
    public static string AggregateName => GitStorageAccountDomainHelper.GitStorageAccountAggregateName;
}
