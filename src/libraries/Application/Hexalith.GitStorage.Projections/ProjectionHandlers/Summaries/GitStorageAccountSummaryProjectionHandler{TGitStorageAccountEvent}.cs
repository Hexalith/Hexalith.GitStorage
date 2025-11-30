// <copyright file="GitStorageAccountSummaryProjectionHandler{TGitStorageAccountEvent}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.GitStorage.Projections.ProjectionHandlers.Summaries;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Projections;
using Hexalith.GitStorage.Events.GitStorageAccount;
using Hexalith.GitStorage.Requests.GitStorageAccount;

/// <summary>
/// Abstract base class for handling updates to GitStorageAccount projections based on events.
/// </summary>
/// <typeparam name="TGitStorageAccountEvent">The type of the warehouse event.</typeparam>
/// <param name="factory">The actor projection factory.</param>
public abstract class GitStorageAccountSummaryProjectionHandler<TGitStorageAccountEvent>(IProjectionFactory<GitStorageAccountSummaryViewModel> factory)
    : KeyValueProjectionUpdateEventHandlerBase<TGitStorageAccountEvent, GitStorageAccountSummaryViewModel>(factory)
    where TGitStorageAccountEvent : GitStorageAccountEvent
{
    /// <inheritdoc/>
    public override async Task ApplyAsync([NotNull] TGitStorageAccountEvent baseEvent, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        ArgumentNullException.ThrowIfNull(metadata);

        GitStorageAccountSummaryViewModel? currentValue = await GetProjectionAsync(metadata.AggregateGlobalId, cancellationToken)
            .ConfigureAwait(false);

        GitStorageAccountSummaryViewModel? newValue = await ApplyEventAsync(
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
    /// Applies the event to the warehouse summary view model.
    /// </summary>
    /// <param name="baseEvent">The warehouse event.</param>
    /// <param name="summary">The existing warehouse summary view model, if any.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated warehouse summary view model.</returns>
    protected abstract Task<GitStorageAccountSummaryViewModel?> ApplyEventAsync(TGitStorageAccountEvent baseEvent, GitStorageAccountSummaryViewModel? summary, CancellationToken cancellationToken);
}