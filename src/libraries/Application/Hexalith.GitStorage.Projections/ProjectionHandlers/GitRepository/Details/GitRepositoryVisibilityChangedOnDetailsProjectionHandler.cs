// <copyright file="GitRepositoryVisibilityChangedOnDetailsProjectionHandler.cs" company="ITANEO">
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
/// Handles the projection update when a GitRepository visibility is changed.
/// </summary>
/// <param name="factory">The projection factory.</param>
public class GitRepositoryVisibilityChangedOnDetailsProjectionHandler(IProjectionFactory<GitRepositoryDetailsViewModel> factory)
    : GitRepositoryDetailsProjectionHandler<GitRepositoryVisibilityChanged>(factory)
{
    /// <inheritdoc/>
    protected override Task<GitRepositoryDetailsViewModel?> ApplyEventAsync([NotNull] GitRepositoryVisibilityChanged baseEvent, GitRepositoryDetailsViewModel? model, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return model == null
            ? Task.FromResult<GitRepositoryDetailsViewModel?>(null)
            : Task.FromResult<GitRepositoryDetailsViewModel?>(model with { Visibility = baseEvent.Visibility });
    }
}
