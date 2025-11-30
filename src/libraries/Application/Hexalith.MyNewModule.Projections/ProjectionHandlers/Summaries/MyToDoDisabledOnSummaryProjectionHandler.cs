// <copyright file="MyToDoDisabledOnSummaryProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Projections.ProjectionHandlers.Summaries;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Projections;
using Hexalith.MyNewModule.Events.MyToDo;
using Hexalith.MyNewModule.Requests.MyToDo;

/// <summary>
/// Handles the projection update when a warehouse is disabled.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MyToDoDisabledOnSummaryProjectionHandler"/> class.
/// </remarks>
/// <param name="factory">The projection factory.</param>
public class MyToDoDisabledOnSummaryProjectionHandler(IProjectionFactory<MyToDoSummaryViewModel> factory)
    : MyToDoSummaryProjectionHandler<MyToDoDisabled>(factory)
{
    /// <summary>
    /// Applies the warehouse disabled event to the summary view model.
    /// </summary>
    /// <param name="baseEvent">The warehouse disabled event.</param>
    /// <param name="summary">The current summary view model.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>The updated summary view model.</returns>
    protected override Task<MyToDoSummaryViewModel?> ApplyEventAsync([NotNull] MyToDoDisabled baseEvent, MyToDoSummaryViewModel? summary, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return summary == null
            ? Task.FromResult<MyToDoSummaryViewModel?>(null)
            : Task.FromResult<MyToDoSummaryViewModel?>(summary with { Disabled = true });
    }
}