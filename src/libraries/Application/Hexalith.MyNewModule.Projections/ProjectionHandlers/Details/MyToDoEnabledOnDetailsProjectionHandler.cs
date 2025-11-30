// <copyright file="MyToDoEnabledOnDetailsProjectionHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Projections.ProjectionHandlers.Details;

using System.Diagnostics.CodeAnalysis;

using Hexalith.Application.Projections;
using Hexalith.MyNewModule.Events.MyToDo;
using Hexalith.MyNewModule.Requests.MyToDo;

/// <summary>
/// Handles the projection update when a warehouse is enabled.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="MyToDoEnabledOnDetailsProjectionHandler"/> class.
/// </remarks>
/// <param name="factory">The projection factory.</param>
public class MyToDoEnabledOnDetailsProjectionHandler(IProjectionFactory<MyToDoDetailsViewModel> factory)
    : MyToDoDetailsProjectionHandler<MyToDoEnabled>(factory)
{
    /// <inheritdoc/>
    protected override Task<MyToDoDetailsViewModel?> ApplyEventAsync([NotNull] MyToDoEnabled baseEvent, MyToDoDetailsViewModel? model, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(baseEvent);
        return model?.Disabled == true
            ? Task.FromResult<MyToDoDetailsViewModel?>(model with { Disabled = false })
            : Task.FromResult<MyToDoDetailsViewModel?>(null);
    }
}