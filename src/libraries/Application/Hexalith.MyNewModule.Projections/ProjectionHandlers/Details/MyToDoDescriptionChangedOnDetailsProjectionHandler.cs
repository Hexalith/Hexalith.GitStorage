// <copyright file="MyToDoDescriptionChangedOnDetailsProjectionHandler.cs" company="ITANEO">
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
/// Handles the projection update when a warehouse description is changed.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MyToDoDescriptionChangedOnDetailsProjectionHandler"/> class.
/// </remarks>
/// <param name="factory">The projection factory.</param>
public class MyToDoDescriptionChangedOnDetailsProjectionHandler(IProjectionFactory<MyToDoDetailsViewModel> factory)
    : MyToDoDetailsProjectionHandler<MyToDoDescriptionChanged>(factory)
{
    /// <inheritdoc/>
    protected override Task<MyToDoDetailsViewModel?> ApplyEventAsync([NotNull] MyToDoDescriptionChanged baseEvent, MyToDoDetailsViewModel? model, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return model == null
            ? Task.FromResult<MyToDoDetailsViewModel?>(new MyToDoDetailsViewModel(
                baseEvent.Id,
                baseEvent.Name,
                baseEvent.Comments,
                false))
            : Task.FromResult<MyToDoDetailsViewModel?>(model with { Name = baseEvent.Name, Comments = baseEvent.Comments });
    }
}