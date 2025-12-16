// <copyright file="GitRepositorySummaryProjectionHandler{TGitRepositoryEvent}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Projections.ProjectionHandlers.GitRepository.Summaries;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Projections;
using Hexalith.GitStorage.Events.GitRepository;
using Hexalith.GitStorage.Requests.GitRepository;

/// <summary>
/// Abstract base class for handling updates to GitRepository summary projections based on events.
/// </summary>
/// <typeparam name="TGitRepositoryEvent">The type of the GitRepository event.</typeparam>
/// <param name="factory">The projection factory.</param>
public abstract class GitRepositorySummaryProjectionHandler<TGitRepositoryEvent>(IProjectionFactory<GitRepositorySummaryViewModel> factory)
    : KeyValueProjectionUpdateEventHandlerBase<TGitRepositoryEvent, GitRepositorySummaryViewModel>(factory)
    where TGitRepositoryEvent : GitRepositoryEvent
{
    /// <inheritdoc/>
    public override async Task ApplyAsync([NotNull] TGitRepositoryEvent baseEvent, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        ArgumentNullException.ThrowIfNull(metadata);

        GitRepositorySummaryViewModel? currentValue = await GetProjectionAsync(metadata.AggregateGlobalId, cancellationToken)
            .ConfigureAwait(false);

        GitRepositorySummaryViewModel? newValue = await ApplyEventAsync(
                baseEvent,
                currentValue,
                cancellationToken)
            .ConfigureAwait(false);
        if (newValue == null)
        {
            return;
        }

        await SaveProjectionAsync(metadata.AggregateGlobalId, newValue, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Applies the event to the GitRepository summary view model.
    /// </summary>
    /// <param name="baseEvent">The GitRepository event.</param>
    /// <param name="summary">The existing GitRepository summary view model, if any.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated GitRepository summary view model.</returns>
    protected abstract Task<GitRepositorySummaryViewModel?> ApplyEventAsync(TGitRepositoryEvent baseEvent, GitRepositorySummaryViewModel? summary, CancellationToken cancellationToken);
}
