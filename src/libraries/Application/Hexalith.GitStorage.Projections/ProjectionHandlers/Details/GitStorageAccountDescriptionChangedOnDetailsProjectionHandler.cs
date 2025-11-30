// <copyright file="GitStorageAccountDescriptionChangedOnDetailsProjectionHandler.cs" company="ITANEO">
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
/// Handles the projection update when a warehouse description is changed.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="GitStorageAccountDescriptionChangedOnDetailsProjectionHandler"/> class.
/// </remarks>
/// <param name="factory">The projection factory.</param>
public class GitStorageAccountDescriptionChangedOnDetailsProjectionHandler(IProjectionFactory<GitStorageAccountDetailsViewModel> factory)
    : GitStorageAccountDetailsProjectionHandler<GitStorageAccountDescriptionChanged>(factory)
{
    /// <inheritdoc/>
    protected override Task<GitStorageAccountDetailsViewModel?> ApplyEventAsync([NotNull] GitStorageAccountDescriptionChanged baseEvent, GitStorageAccountDetailsViewModel? model, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return model == null
            ? Task.FromResult<GitStorageAccountDetailsViewModel?>(new GitStorageAccountDetailsViewModel(
                baseEvent.Id,
                baseEvent.Name,
                baseEvent.Comments,
                false))
            : Task.FromResult<GitStorageAccountDetailsViewModel?>(model with { Name = baseEvent.Name, Comments = baseEvent.Comments });
    }
}