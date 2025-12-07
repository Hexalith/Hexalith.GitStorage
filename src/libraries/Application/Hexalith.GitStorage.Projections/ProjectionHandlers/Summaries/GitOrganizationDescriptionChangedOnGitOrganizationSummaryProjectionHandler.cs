// <copyright file="GitOrganizationDescriptionChangedOnGitOrganizationSummaryProjectionHandler.cs" company="ITANEO">
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
/// Handles the projection update when a GitOrganization description is changed.
/// </summary>
/// <param name="factory">The projection factory.</param>
public class GitOrganizationDescriptionChangedOnGitOrganizationSummaryProjectionHandler(IProjectionFactory<GitOrganizationSummaryViewModel> factory)
    : GitOrganizationSummaryProjectionHandler<GitOrganizationDescriptionChanged>(factory)
{
    /// <inheritdoc/>
    protected override Task<GitOrganizationSummaryViewModel?> ApplyEventAsync([NotNull] GitOrganizationDescriptionChanged baseEvent, GitOrganizationSummaryViewModel? summary, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        if (summary == null)
        {
            return Task.FromResult<GitOrganizationSummaryViewModel?>(null);
        }

        // Summary view model doesn't include description, so return unchanged
        return Task.FromResult<GitOrganizationSummaryViewModel?>(summary);
    }
}
