// <copyright file="MyToDoDescriptionChangedOnSummaryProjectionHandler.cs" company="ITANEO">
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
/// Handles the projection update when a warehouse description is changed.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MyToDoDescriptionChangedOnSummaryProjectionHandler"/> class.
/// </remarks>
/// <param name="factory">The projection factory.</param>
public class MyToDoDescriptionChangedOnSummaryProjectionHandler(IProjectionFactory<MyToDoSummaryViewModel> factory)
    : MyToDoSummaryProjectionHandler<MyToDoDescriptionChanged>(factory)
{
    /// <inheritdoc/>
    protected override Task<MyToDoSummaryViewModel?> ApplyEventAsync([NotNull] MyToDoDescriptionChanged baseEvent, MyToDoSummaryViewModel? summary, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return summary == null
            ? Task.FromResult<MyToDoSummaryViewModel?>(null)
            : Task.FromResult<MyToDoSummaryViewModel?>(summary with { Name = baseEvent.Name });
    }
}