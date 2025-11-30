// <copyright file="GitStorageAccountDescriptionChangedOnSummaryProjectionHandler.cs" company="ITANEO">
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
/// Handles the projection update when a warehouse description is changed.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="GitStorageAccountDescriptionChangedOnSummaryProjectionHandler"/> class.
/// </remarks>
/// <param name="factory">The projection factory.</param>
public class GitStorageAccountDescriptionChangedOnSummaryProjectionHandler(IProjectionFactory<GitStorageAccountSummaryViewModel> factory)
    : GitStorageAccountSummaryProjectionHandler<GitStorageAccountDescriptionChanged>(factory)
{
    /// <inheritdoc/>
    protected override Task<GitStorageAccountSummaryViewModel?> ApplyEventAsync([NotNull] GitStorageAccountDescriptionChanged baseEvent, GitStorageAccountSummaryViewModel? summary, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return summary == null
            ? Task.FromResult<GitStorageAccountSummaryViewModel?>(null)
            : Task.FromResult<GitStorageAccountSummaryViewModel?>(summary with { Name = baseEvent.Name });
    }
}