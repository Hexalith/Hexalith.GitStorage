// <copyright file="GitOrganizationDetailsProjectionHandler{TGitOrganizationEvent}.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Projections.ProjectionHandlers.Details;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Projections;
using Hexalith.GitStorage.Events.GitOrganization;
using Hexalith.GitStorage.Requests.GitOrganization;

/// <summary>
/// Abstract base class for handling updates to GitOrganization details projections based on events.
/// </summary>
/// <typeparam name="TGitOrganizationEvent">The type of the GitOrganization event.</typeparam>
/// <param name="factory">The projection factory.</param>
public abstract class GitOrganizationDetailsProjectionHandler<TGitOrganizationEvent>(IProjectionFactory<GitOrganizationDetailsViewModel> factory)
    : KeyValueProjectionUpdateEventHandlerBase<TGitOrganizationEvent, GitOrganizationDetailsViewModel>(factory)
    where TGitOrganizationEvent : GitOrganizationEvent
{
    /// <inheritdoc/>
    public override async Task ApplyAsync([NotNull] TGitOrganizationEvent baseEvent, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        ArgumentNullException.ThrowIfNull(metadata);

        GitOrganizationDetailsViewModel? currentValue = await GetProjectionAsync(metadata.AggregateGlobalId, cancellationToken)
            .ConfigureAwait(false);

        GitOrganizationDetailsViewModel? newValue = await ApplyEventAsync(
                baseEvent,
                currentValue,
                cancellationToken)
            .ConfigureAwait(false);
        if (newValue == null || newValue == currentValue)
        {
            return;
        }

        await SaveProjectionAsync(metadata.AggregateGlobalId, newValue, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Applies the event to the GitOrganization details view model.
    /// </summary>
    /// <param name="baseEvent">The GitOrganization event.</param>
    /// <param name="model">The current GitOrganization details view model.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated GitOrganization details view model.</returns>
    protected abstract Task<GitOrganizationDetailsViewModel?> ApplyEventAsync(TGitOrganizationEvent baseEvent, GitOrganizationDetailsViewModel? model, CancellationToken cancellationToken);
}
