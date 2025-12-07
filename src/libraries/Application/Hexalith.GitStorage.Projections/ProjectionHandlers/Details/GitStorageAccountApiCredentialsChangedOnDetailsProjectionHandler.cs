// <copyright file="GitStorageAccountApiCredentialsChangedOnDetailsProjectionHandler.cs" company="ITANEO">
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
/// Handles the projection update when API credentials are changed for a GitStorageAccount.
/// </summary>
/// <param name="factory">The projection factory.</param>
public class GitStorageAccountApiCredentialsChangedOnDetailsProjectionHandler(IProjectionFactory<GitStorageAccountDetailsViewModel> factory)
    : GitStorageAccountDetailsProjectionHandler<GitStorageAccountApiCredentialsChanged>(factory)
{
    /// <inheritdoc/>
    protected override Task<GitStorageAccountDetailsViewModel?> ApplyEventAsync(
        [NotNull] GitStorageAccountApiCredentialsChanged baseEvent,
        GitStorageAccountDetailsViewModel? model,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return model == null
            ? Task.FromResult<GitStorageAccountDetailsViewModel?>(new GitStorageAccountDetailsViewModel(
                baseEvent.Id,
                string.Empty,
                null,
                false,
                baseEvent.ServerUrl,
                baseEvent.AccessToken,
                baseEvent.ProviderType))
            : Task.FromResult<GitStorageAccountDetailsViewModel?>(model with
            {
                ServerUrl = baseEvent.ServerUrl,
                AccessToken = baseEvent.AccessToken,
                ProviderType = baseEvent.ProviderType,
            });
    }
}
