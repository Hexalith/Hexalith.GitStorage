// <copyright file="GitRepositoryDefaultBranchChangedOnDetailsProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Projections.ProjectionHandlers.GitRepository.Details;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Projections;
using Hexalith.GitStorage.Events.GitRepository;
using Hexalith.GitStorage.Requests.GitRepository;

/// <summary>
/// Handles the projection update when a GitRepository default branch is changed.
/// </summary>
/// <param name="factory">The projection factory.</param>
public class GitRepositoryDefaultBranchChangedOnDetailsProjectionHandler(IProjectionFactory<GitRepositoryDetailsViewModel> factory)
    : GitRepositoryDetailsProjectionHandler<GitRepositoryDefaultBranchChanged>(factory)
{
    /// <inheritdoc/>
    protected override Task<GitRepositoryDetailsViewModel?> ApplyEventAsync([NotNull] GitRepositoryDefaultBranchChanged baseEvent, GitRepositoryDetailsViewModel? model, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return model == null
            ? Task.FromResult<GitRepositoryDetailsViewModel?>(null)
            : Task.FromResult<GitRepositoryDetailsViewModel?>(model with { DefaultBranch = baseEvent.DefaultBranch });
    }
}
