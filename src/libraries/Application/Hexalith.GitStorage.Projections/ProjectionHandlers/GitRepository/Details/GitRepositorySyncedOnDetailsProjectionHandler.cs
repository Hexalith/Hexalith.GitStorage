// <copyright file="GitRepositorySyncedOnDetailsProjectionHandler.cs" company="ITANEO">
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
/// Handles the projection update when a GitRepository is synced.
/// </summary>
/// <param name="factory">The projection factory.</param>
public class GitRepositorySyncedOnDetailsProjectionHandler(IProjectionFactory<GitRepositoryDetailsViewModel> factory)
    : GitRepositoryDetailsProjectionHandler<GitRepositorySynced>(factory)
{
    /// <inheritdoc/>
    protected override Task<GitRepositoryDetailsViewModel?> ApplyEventAsync([NotNull] GitRepositorySynced baseEvent, GitRepositoryDetailsViewModel? model, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return Task.FromResult<GitRepositoryDetailsViewModel?>(new GitRepositoryDetailsViewModel(
            baseEvent.Id,
            baseEvent.Name,
            baseEvent.Description,
            baseEvent.Url,
            baseEvent.DefaultBranch,
            baseEvent.OrganizationId,
            model?.OrganizationName ?? string.Empty,
            baseEvent.Visibility,
            GitRepositoryOrigin.Synced,
            baseEvent.RemoteId,
            GitRepositorySyncStatus.Synced,
            baseEvent.SyncedAt,
            model?.Disabled ?? false));
    }
}
