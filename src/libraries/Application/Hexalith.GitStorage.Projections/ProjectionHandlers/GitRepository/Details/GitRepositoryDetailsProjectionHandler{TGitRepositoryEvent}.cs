// <copyright file="GitRepositoryDetailsProjectionHandler{TGitRepositoryEvent}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Projections.ProjectionHandlers.GitRepository.Details;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Projections;
using Hexalith.GitStorage.Events.GitRepository;
using Hexalith.GitStorage.Requests.GitRepository;

/// <summary>
/// Abstract base class for handling updates to GitRepository details projections based on events.
/// </summary>
/// <typeparam name="TGitRepositoryEvent">The type of the GitRepository event.</typeparam>
/// <param name="factory">The projection factory.</param>
public abstract class GitRepositoryDetailsProjectionHandler<TGitRepositoryEvent>(IProjectionFactory<GitRepositoryDetailsViewModel> factory)
    : KeyValueProjectionUpdateEventHandlerBase<TGitRepositoryEvent, GitRepositoryDetailsViewModel>(factory)
    where TGitRepositoryEvent : GitRepositoryEvent
{
    /// <inheritdoc/>
    public override async Task ApplyAsync([NotNull] TGitRepositoryEvent baseEvent, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        ArgumentNullException.ThrowIfNull(metadata);

        GitRepositoryDetailsViewModel? currentValue = await GetProjectionAsync(metadata.AggregateGlobalId, cancellationToken)
            .ConfigureAwait(false);

        GitRepositoryDetailsViewModel? newValue = await ApplyEventAsync(
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
    /// Applies the event to the GitRepository details view model.
    /// </summary>
    /// <param name="baseEvent">The GitRepository event.</param>
    /// <param name="model">The existing GitRepository details view model, if any.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated GitRepository details view model.</returns>
    protected abstract Task<GitRepositoryDetailsViewModel?> ApplyEventAsync(TGitRepositoryEvent baseEvent, GitRepositoryDetailsViewModel? model, CancellationToken cancellationToken);
}
