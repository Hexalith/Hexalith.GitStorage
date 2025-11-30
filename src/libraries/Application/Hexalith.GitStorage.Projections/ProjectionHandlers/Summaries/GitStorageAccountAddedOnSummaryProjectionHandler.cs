// <copyright file="GitStorageAccountAddedOnSummaryProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.GitStorage.Projections.ProjectionHandlers.Summaries;

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
public class GitStorageAccountAddedOnSummaryProjectionHandler(IProjectionFactory<GitStorageAccountSummaryViewModel> factory)
    : GitStorageAccountSummaryProjectionHandler<GitStorageAccountAdded>(factory)
{
    /// <inheritdoc/>
    protected override Task<GitStorageAccountSummaryViewModel?> ApplyEventAsync([NotNull] GitStorageAccountAdded baseEvent, GitStorageAccountSummaryViewModel? summary, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return Task.FromResult<GitStorageAccountSummaryViewModel?>(new GitStorageAccountSummaryViewModel(baseEvent.Id, baseEvent.Name, false));
    }
}