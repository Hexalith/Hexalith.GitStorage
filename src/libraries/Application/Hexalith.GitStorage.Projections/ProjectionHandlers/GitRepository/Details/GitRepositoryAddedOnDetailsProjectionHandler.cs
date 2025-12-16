// <copyright file="GitRepositoryAddedOnDetailsProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Projections.ProjectionHandlers.GitRepository.Details;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Projections;
using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.GitStorage.Events.GitRepository;
using Hexalith.GitStorage.Requests.GitRepository;

/// <summary>
/// Handles the projection update when a GitRepository is added.
/// </summary>
/// <param name="factory">The projection factory.</param>
public class GitRepositoryAddedOnDetailsProjectionHandler(IProjectionFactory<GitRepositoryDetailsViewModel> factory)
    : GitRepositoryDetailsProjectionHandler<GitRepositoryAdded>(factory)
{
    /// <inheritdoc/>
    protected override Task<GitRepositoryDetailsViewModel?> ApplyEventAsync([NotNull] GitRepositoryAdded baseEvent, GitRepositoryDetailsViewModel? model, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return Task.FromResult<GitRepositoryDetailsViewModel?>(new GitRepositoryDetailsViewModel(
            baseEvent.Id,
            baseEvent.Name,
            baseEvent.Description,
            baseEvent.Url,
            baseEvent.DefaultBranch,
            baseEvent.OrganizationId,
            string.Empty,
            baseEvent.Visibility,
            GitRepositoryOrigin.CreatedViaApplication,
            baseEvent.RemoteId,
            GitRepositorySyncStatus.Synced,
            null,
            false));
    }
}
