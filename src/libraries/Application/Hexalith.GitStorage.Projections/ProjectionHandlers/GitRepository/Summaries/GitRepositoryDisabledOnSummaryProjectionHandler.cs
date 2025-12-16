// <copyright file="GitRepositoryDisabledOnSummaryProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Projections.ProjectionHandlers.GitRepository.Summaries;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Projections;
using Hexalith.GitStorage.Events.GitRepository;
using Hexalith.GitStorage.Requests.GitRepository;

/// <summary>
/// Handles the projection update when a GitRepository is disabled.
/// </summary>
/// <param name="factory">The projection factory.</param>
public class GitRepositoryDisabledOnSummaryProjectionHandler(IProjectionFactory<GitRepositorySummaryViewModel> factory)
    : GitRepositorySummaryProjectionHandler<GitRepositoryDisabled>(factory)
{
    /// <inheritdoc/>
    protected override Task<GitRepositorySummaryViewModel?> ApplyEventAsync([NotNull] GitRepositoryDisabled baseEvent, GitRepositorySummaryViewModel? summary, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return summary == null
            ? Task.FromResult<GitRepositorySummaryViewModel?>(null)
            : Task.FromResult<GitRepositorySummaryViewModel?>(summary with { Disabled = true });
    }
}
