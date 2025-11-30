// <copyright file="GitStorageAccountEnabledOnSummaryProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.GitStorage.Projections.ProjectionHandlers.Summaries;

using System.Diagnostics.CodeAnalysis;

using Hexalith.Application.Projections;
using Hexalith.GitStorage.Events.GitStorageAccount;
using Hexalith.GitStorage.Requests.GitStorageAccount;

/// <summary>
/// Handles the projection update when a warehouse is enabled.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="GitStorageAccountEnabledOnSummaryProjectionHandler"/> class.
/// </remarks>
/// <param name="factory">The projection factory.</param>
public class GitStorageAccountEnabledOnSummaryProjectionHandler(IProjectionFactory<GitStorageAccountSummaryViewModel> factory)
    : GitStorageAccountSummaryProjectionHandler<GitStorageAccountEnabled>(factory)
{
    /// <summary>
    /// Applies the event to the summary projection.
    /// </summary>
    /// <param name="baseEvent">The event to apply.</param>
    /// <param name="summary">The current summary projection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated summary projection.</returns>
    protected override Task<GitStorageAccountSummaryViewModel?> ApplyEventAsync([NotNull] GitStorageAccountEnabled baseEvent, GitStorageAccountSummaryViewModel? summary, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return summary == null
            ? Task.FromResult<GitStorageAccountSummaryViewModel?>(null)
            : Task.FromResult<GitStorageAccountSummaryViewModel?>(summary with { Disabled = false });
    }
}