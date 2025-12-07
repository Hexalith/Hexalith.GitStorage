// <copyright file="GitOrganizationSyncedOnGitOrganizationSummaryProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Projections.ProjectionHandlers.Summaries;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Projections;
using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.GitStorage.Events.GitOrganization;
using Hexalith.GitStorage.Requests.GitOrganization;

/// <summary>
/// Handles the projection update when a GitOrganization is synced.
/// </summary>
/// <param name="factory">The projection factory.</param>
public class GitOrganizationSyncedOnGitOrganizationSummaryProjectionHandler(IProjectionFactory<GitOrganizationSummaryViewModel> factory)
    : GitOrganizationSummaryProjectionHandler<GitOrganizationSynced>(factory)
{
    /// <inheritdoc/>
    protected override Task<GitOrganizationSummaryViewModel?> ApplyEventAsync([NotNull] GitOrganizationSynced baseEvent, GitOrganizationSummaryViewModel? summary, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return Task.FromResult<GitOrganizationSummaryViewModel?>(new GitOrganizationSummaryViewModel(
            baseEvent.Id,
            baseEvent.Name,
            baseEvent.GitStorageAccountId,
            baseEvent.Visibility,
            GitOrganizationSyncStatus.Synced,
            summary?.Disabled ?? false));
    }
}