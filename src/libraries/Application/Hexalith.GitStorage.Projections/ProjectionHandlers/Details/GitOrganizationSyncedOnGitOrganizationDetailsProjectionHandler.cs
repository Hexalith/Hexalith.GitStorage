// <copyright file="GitOrganizationSyncedOnGitOrganizationDetailsProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Projections.ProjectionHandlers.Details;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Projections;
using Hexalith.GitStorage.Aggregates.Enums;
using Hexalith.GitStorage.Events.GitOrganization;
using Hexalith.GitStorage.Requests.GitOrganization;

/// <summary>
/// Handles the projection update when a GitOrganization is synced.
/// </summary>
/// <param name="factory">The projection factory.</param>
public class GitOrganizationSyncedOnGitOrganizationDetailsProjectionHandler(IProjectionFactory<GitOrganizationDetailsViewModel> factory)
    : GitOrganizationDetailsProjectionHandler<GitOrganizationSynced>(factory)
{
    /// <inheritdoc/>
    protected override Task<GitOrganizationDetailsViewModel?> ApplyEventAsync([NotNull] GitOrganizationSynced baseEvent, GitOrganizationDetailsViewModel? model, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return Task.FromResult<GitOrganizationDetailsViewModel?>(new GitOrganizationDetailsViewModel(
            baseEvent.Id,
            baseEvent.Name,
            baseEvent.Description,
            baseEvent.GitStorageAccountId,
            model?.GitStorageAccountName ?? string.Empty,
            baseEvent.Visibility,
            model?.Origin ?? GitOrganizationOrigin.Synced,
            baseEvent.RemoteId,
            GitOrganizationSyncStatus.Synced,
            baseEvent.SyncedAt,
            model?.Disabled ?? false));
    }
}