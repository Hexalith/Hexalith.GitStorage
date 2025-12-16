// <copyright file="GitRepositoryDomainHelper.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Aggregates;

/// <summary>
/// Helper class for GitRepository domain constants.
/// </summary>
public static class GitRepositoryDomainHelper
{
    /// <summary>
    /// The name of the GitRepository aggregate.
    /// </summary>
    public const string GitRepositoryAggregateName = "GitRepository";

    /// <summary>
    /// Generates a composite identifier for a GitRepository.
    /// </summary>
    /// <param name="organizationId">The Git Organization identifier.</param>
    /// <param name="repositoryName">The repository name.</param>
    /// <returns>The composite identifier in format: {organizationId}-{repositoryName} (lowercase).</returns>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "N/A")]
    public static string GenerateId(string organizationId, string repositoryName)
        => $"{organizationId}-{repositoryName?.ToLowerInvariant()}";
}
