// <copyright file="GitStorageAccountSnapshotOnSummaryProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.GitStorage.Projections.ProjectionHandlers.Summaries;

using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Projections;
using Hexalith.Domain.Events;
using Hexalith.GitStorage.Aggregates;
using Hexalith.GitStorage.Requests.GitStorageAccount;

/// <summary>
/// Handles the projection updates for warehouse snapshots on summary.
/// </summary>
/// <param name="factory">The projection factory.</param>
public class GitStorageAccountSnapshotOnSummaryProjectionHandler(IProjectionFactory<GitStorageAccountSummaryViewModel> factory)
    : IProjectionUpdateHandler<SnapshotEvent>
{
    /// <inheritdoc/>
    public async Task ApplyAsync(SnapshotEvent baseEvent, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        ArgumentNullException.ThrowIfNull(metadata);
        if (baseEvent?.AggregateName != GitStorageAccountDomainHelper.GitStorageAccountAggregateName)
        {
            return;
        }

        GitStorageAccountSummaryViewModel? currentValue = await factory
            .GetStateAsync(metadata.AggregateGlobalId, cancellationToken)
            .ConfigureAwait(false);

        GitStorageAccount warehouse = baseEvent.GetAggregate<GitStorageAccount>();
        GitStorageAccountSummaryViewModel newValue = new(warehouse.Id, warehouse.Name, warehouse.Disabled);
        if (currentValue is not null && currentValue == newValue)
        {
            return;
        }

        await factory
            .SetStateAsync(
                metadata.AggregateGlobalId,
                newValue,
                cancellationToken)
            .ConfigureAwait(false);
    }
}