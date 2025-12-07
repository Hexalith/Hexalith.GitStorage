// <copyright file="GitOrganizationDomainHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Aggregates;

/// <summary>
/// Helper class for GitOrganization domain constants.
/// </summary>
public static class GitOrganizationDomainHelper
{
    /// <summary>
    /// The name of the GitOrganization aggregate.
    /// </summary>
    public const string GitOrganizationAggregateName = "GitOrganization";

    /// <summary>
    /// Generates a composite identifier for a GitOrganization.
    /// </summary>
    /// <param name="gitStorageAccountId">The Git Storage Account identifier.</param>
    /// <param name="organizationName">The organization name.</param>
    /// <returns>The composite identifier in format: {gitStorageAccountId}-{organizationName} (lowercase).</returns>
    public static string GenerateId(string gitStorageAccountId, string organizationName)
        => $"{gitStorageAccountId}-{organizationName.ToLowerInvariant()}";
}
