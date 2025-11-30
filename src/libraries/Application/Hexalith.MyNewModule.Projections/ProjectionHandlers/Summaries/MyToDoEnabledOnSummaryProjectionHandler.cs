// <copyright file="MyToDoEnabledOnSummaryProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Projections.ProjectionHandlers.Summaries;

using System.Diagnostics.CodeAnalysis;

using Hexalith.Application.Projections;
using Hexalith.MyNewModule.Events.MyToDo;
using Hexalith.MyNewModule.Requests.MyToDo;

/// <summary>
/// Handles the projection update when a warehouse is enabled.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MyToDoEnabledOnSummaryProjectionHandler"/> class.
/// </remarks>
/// <param name="factory">The projection factory.</param>
public class MyToDoEnabledOnSummaryProjectionHandler(IProjectionFactory<MyToDoSummaryViewModel> factory)
    : MyToDoSummaryProjectionHandler<MyToDoEnabled>(factory)
{
    /// <summary>
    /// Applies the event to the summary projection.
    /// </summary>
    /// <param name="baseEvent">The event to apply.</param>
    /// <param name="summary">The current summary projection.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated summary projection.</returns>
    protected override Task<MyToDoSummaryViewModel?> ApplyEventAsync([NotNull] MyToDoEnabled baseEvent, MyToDoSummaryViewModel? summary, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return summary == null
            ? Task.FromResult<MyToDoSummaryViewModel?>(null)
            : Task.FromResult<MyToDoSummaryViewModel?>(summary with { Disabled = false });
    }
}