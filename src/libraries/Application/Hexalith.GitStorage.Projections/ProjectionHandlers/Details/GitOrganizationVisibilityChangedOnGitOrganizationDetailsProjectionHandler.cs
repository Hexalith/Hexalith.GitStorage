// <copyright file="GitOrganizationVisibilityChangedOnGitOrganizationDetailsProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.GitStorage.Projections.ProjectionHandlers.Details;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Projections;
using Hexalith.GitStorage.Events.GitOrganization;
using Hexalith.GitStorage.Requests.GitOrganization;

/// <summary>
/// Handles the projection update when a GitOrganization visibility is changed.
/// </summary>
/// <param name="factory">The projection factory.</param>
public class GitOrganizationVisibilityChangedOnGitOrganizationDetailsProjectionHandler(IProjectionFactory<GitOrganizationDetailsViewModel> factory)
    : GitOrganizationDetailsProjectionHandler<GitOrganizationVisibilityChanged>(factory)
{
    /// <inheritdoc/>
    protected override Task<GitOrganizationDetailsViewModel?> ApplyEventAsync([NotNull] GitOrganizationVisibilityChanged baseEvent, GitOrganizationDetailsViewModel? model, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return model is null
            ? Task.FromResult<GitOrganizationDetailsViewModel?>(null)
            : Task.FromResult<GitOrganizationDetailsViewModel?>(model with { Visibility = baseEvent.Visibility });
    }
}