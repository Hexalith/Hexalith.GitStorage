// <copyright file="GitStorageAccountDisabledOnDetailsProjectionHandler.cs" company="ITANEO">
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
/// Handles the projection update when a warehouse is disabled.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="GitStorageAccountDisabledOnDetailsProjectionHandler"/> class.
/// </remarks>
/// <param name="factory">The projection factory.</param>
public class GitStorageAccountDisabledOnDetailsProjectionHandler(IProjectionFactory<GitStorageAccountDetailsViewModel> factory)
    : GitStorageAccountDetailsProjectionHandler<GitStorageAccountDisabled>(factory)
{
    /// <inheritdoc/>
    protected override Task<GitStorageAccountDetailsViewModel?> ApplyEventAsync([NotNull] GitStorageAccountDisabled baseEvent, GitStorageAccountDetailsViewModel? model, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return model?.Disabled == false
            ? Task.FromResult<GitStorageAccountDetailsViewModel?>(model with { Disabled = true })
            : Task.FromResult<GitStorageAccountDetailsViewModel?>(null);
    }
}