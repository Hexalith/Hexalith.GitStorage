// <copyright file="GitOrganizationVisibilityChangedOnGitOrganizationSummaryProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Projections.ProjectionHandlers.Summaries;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Projections;
using Hexalith.GitStorage.Events.GitOrganization;
using Hexalith.GitStorage.Requests.GitOrganization;

/// <summary>
/// Handles the projection update when a GitOrganization visibility is changed.
/// </summary>
/// <param name="factory">The projection factory.</param>
public class GitOrganizationVisibilityChangedOnGitOrganizationSummaryProjectionHandler(IProjectionFactory<GitOrganizationSummaryViewModel> factory)
    : GitOrganizationSummaryProjectionHandler<GitOrganizationVisibilityChanged>(factory)
{
    /// <inheritdoc/>
    protected override Task<GitOrganizationSummaryViewModel?> ApplyEventAsync([NotNull] GitOrganizationVisibilityChanged baseEvent, GitOrganizationSummaryViewModel? summary, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return summary is null
            ? Task.FromResult<GitOrganizationSummaryViewModel?>(null)
            : Task.FromResult<GitOrganizationSummaryViewModel?>(summary with { Visibility = baseEvent.Visibility });
    }
}