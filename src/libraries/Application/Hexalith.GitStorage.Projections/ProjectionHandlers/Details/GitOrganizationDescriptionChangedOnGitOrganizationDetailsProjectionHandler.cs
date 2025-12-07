// <copyright file="GitOrganizationDescriptionChangedOnGitOrganizationDetailsProjectionHandler.cs" company="ITANEO">
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
/// Handles the projection update when a GitOrganization description is changed.
/// </summary>
/// <param name="factory">The projection factory.</param>
public class GitOrganizationDescriptionChangedOnGitOrganizationDetailsProjectionHandler(IProjectionFactory<GitOrganizationDetailsViewModel> factory)
    : GitOrganizationDetailsProjectionHandler<GitOrganizationDescriptionChanged>(factory)
{
    /// <inheritdoc/>
    protected override Task<GitOrganizationDetailsViewModel?> ApplyEventAsync([NotNull] GitOrganizationDescriptionChanged baseEvent, GitOrganizationDetailsViewModel? model, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        if (model == null)
        {
            return Task.FromResult<GitOrganizationDetailsViewModel?>(null);
        }

        return Task.FromResult<GitOrganizationDetailsViewModel?>(model with
        {
            Description = baseEvent.Description,
        });
    }
}
