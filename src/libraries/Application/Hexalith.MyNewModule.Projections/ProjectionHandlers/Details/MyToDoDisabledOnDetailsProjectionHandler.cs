// <copyright file="MyToDoDisabledOnDetailsProjectionHandler.cs" company="ITANEO">
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
/// Handles the projection update when a warehouse is disabled.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MyToDoDisabledOnDetailsProjectionHandler"/> class.
/// </remarks>
/// <param name="factory">The projection factory.</param>
public class MyToDoDisabledOnDetailsProjectionHandler(IProjectionFactory<MyToDoDetailsViewModel> factory)
    : MyToDoDetailsProjectionHandler<MyToDoDisabled>(factory)
{
    /// <inheritdoc/>
    protected override Task<MyToDoDetailsViewModel?> ApplyEventAsync([NotNull] MyToDoDisabled baseEvent, MyToDoDetailsViewModel? model, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return model?.Disabled == false
            ? Task.FromResult<MyToDoDetailsViewModel?>(model with { Disabled = true })
            : Task.FromResult<MyToDoDetailsViewModel?>(null);
    }
}