// <copyright file="GetMyToDoDetailsHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.MyNewModule.Projections.RequestHandlers;

using System;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Projections;
using Hexalith.Application.Requests;
using Hexalith.MyNewModule.Requests.MyToDo;

/// <summary>
/// Handler for getting mytodo details.
/// </summary>
public class GetMyToDoDetailsHandler : RequestHandlerBase<GetMyToDoDetails>
{
    private readonly IProjectionFactory<MyToDoDetailsViewModel> _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetMyToDoDetailsHandler"/> class.
    /// </summary>
    /// <param name="factory">The projection mytodo.</param>
    /// <exception cref="ArgumentNullException">Thrown when projectionMyToDo is null.</exception>
    public GetMyToDoDetailsHandler(IProjectionFactory<MyToDoDetailsViewModel> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
    }

    /// <inheritdoc/>
    public override async Task<GetMyToDoDetails> ExecuteAsync(GetMyToDoDetails request, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(metadata);

        return request with
        {
            Result = await _factory
                .GetStateAsync(metadata.AggregateGlobalId, cancellationToken)
                .ConfigureAwait(false)
                    ?? throw new InvalidOperationException($"MyToDo type {metadata.AggregateGlobalId} not found."),
        };
    }
}