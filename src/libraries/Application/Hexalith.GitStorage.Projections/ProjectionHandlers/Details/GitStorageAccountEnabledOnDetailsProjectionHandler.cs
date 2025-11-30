// <copyright file="GitStorageAccountEnabledOnDetailsProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.GitStorage.Projections.ProjectionHandlers.Details;

using System.Diagnostics.CodeAnalysis;

using Hexalith.Application.Projections;
using Hexalith.GitStorage.Events.GitStorageAccount;
using Hexalith.GitStorage.Requests.GitStorageAccount;

/// <summary>
/// Handles the projection update when a warehouse is enabled.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="GitStorageAccountEnabledOnDetailsProjectionHandler"/> class.
/// </remarks>
/// <param name="factory">The projection factory.</param>
public class GitStorageAccountEnabledOnDetailsProjectionHandler(IProjectionFactory<GitStorageAccountDetailsViewModel> factory)
    : GitStorageAccountDetailsProjectionHandler<GitStorageAccountEnabled>(factory)
{
    /// <inheritdoc/>
    protected override Task<GitStorageAccountDetailsViewModel?> ApplyEventAsync([NotNull] GitStorageAccountEnabled baseEvent, GitStorageAccountDetailsViewModel? model, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return model?.Disabled == true
            ? Task.FromResult<GitStorageAccountDetailsViewModel?>(model with { Disabled = false })
            : Task.FromResult<GitStorageAccountDetailsViewModel?>(null);
    }
}