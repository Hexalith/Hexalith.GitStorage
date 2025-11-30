// <copyright file="MyToDoAddedOnDetailsProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Projections.ProjectionHandlers.Details;

using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Projections;
using Hexalith.MyNewModule.Events.MyToDo;
using Hexalith.MyNewModule.Requests.MyToDo;

/// <summary>
/// Handles the projection update when a warehouse is added.
/// </summary>
/// <param name="factory">The projection factory.</param>
public class MyToDoAddedOnDetailsProjectionHandler(IProjectionFactory<MyToDoDetailsViewModel> factory)
    : MyToDoDetailsProjectionHandler<MyToDoAdded>(factory)
{
    /// <inheritdoc/>
    protected override Task<MyToDoDetailsViewModel?> ApplyEventAsync([NotNull] MyToDoAdded baseEvent, MyToDoDetailsViewModel? model, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return Task.FromResult<MyToDoDetailsViewModel?>(new MyToDoDetailsViewModel(
            baseEvent.Id,
            baseEvent.Name,
            baseEvent.Comments,
            false));
    }
}