// <copyright file="GitOrganizationEnabledOnGitOrganizationSummaryProjectionHandler.cs" company="ITANEO">
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
/// Handles the projection update when a GitOrganization is enabled.
/// </summary>
/// <param name="factory">The projection factory.</param>
public class GitOrganizationEnabledOnGitOrganizationSummaryProjectionHandler(IProjectionFactory<GitOrganizationSummaryViewModel> factory)
    : GitOrganizationSummaryProjectionHandler<GitOrganizationEnabled>(factory)
{
    /// <inheritdoc/>
    protected override Task<GitOrganizationSummaryViewModel?> ApplyEventAsync([NotNull] GitOrganizationEnabled baseEvent, GitOrganizationSummaryViewModel? summary, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        if (summary == null)
        {
            return Task.FromResult<GitOrganizationSummaryViewModel?>(null);
        }

        return Task.FromResult<GitOrganizationSummaryViewModel?>(summary with { Disabled = false });
    }
}
