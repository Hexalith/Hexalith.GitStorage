// <copyright file="GitStorageAccountAddedOnDetailsProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.GitStorage.Projections.ProjectionHandlers.Details;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Projections;
using Hexalith.GitStorage.Events.GitStorageAccount;
using Hexalith.GitStorage.Requests.GitStorageAccount;

/// <summary>
/// Handles the projection update when a warehouse is added.
/// </summary>
/// <param name="factory">The projection factory.</param>
public class GitStorageAccountAddedOnDetailsProjectionHandler(IProjectionFactory<GitStorageAccountDetailsViewModel> factory)
    : GitStorageAccountDetailsProjectionHandler<GitStorageAccountAdded>(factory)
{
    /// <inheritdoc/>
    protected override Task<GitStorageAccountDetailsViewModel?> ApplyEventAsync([NotNull] GitStorageAccountAdded baseEvent, GitStorageAccountDetailsViewModel? model, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return Task.FromResult<GitStorageAccountDetailsViewModel?>(new GitStorageAccountDetailsViewModel(
            baseEvent.Id,
            baseEvent.Name,
            baseEvent.Comments,
            false,
            baseEvent.ServerUrl,
            baseEvent.AccessToken,
            baseEvent.ProviderType));
    }
}