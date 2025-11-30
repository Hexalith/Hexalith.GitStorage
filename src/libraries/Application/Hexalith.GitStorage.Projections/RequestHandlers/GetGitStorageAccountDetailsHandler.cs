// <copyright file="GetGitStorageAccountDetailsHandler.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
namespace Hexalith.GitStorage.Projections.RequestHandlers;

using System;
using System.Threading;
using System.Threading.Tasks;

using Hexalith.Application.Metadatas;
using Hexalith.Application.Projections;
using Hexalith.Application.Requests;
using Hexalith.GitStorage.Requests.GitStorageAccount;

/// <summary>
/// Handler for getting GitStorageAccount details.
/// </summary>
public class GetGitStorageAccountDetailsHandler : RequestHandlerBase<GetGitStorageAccountDetails>
{
    private readonly IProjectionFactory<GitStorageAccountDetailsViewModel> _factory;

    /// <summary>
    /// Initializes a new instance of the <see cref="GetGitStorageAccountDetailsHandler"/> class.
    /// </summary>
    /// <param name="factory">The projection GitStorageAccount.</param>
    /// <exception cref="ArgumentNullException">Thrown when projectionGitStorageAccount is null.</exception>
    public GetGitStorageAccountDetailsHandler(IProjectionFactory<GitStorageAccountDetailsViewModel> factory)
    {
        ArgumentNullException.ThrowIfNull(factory);
        _factory = factory;
    }

    /// <inheritdoc/>
    public override async Task<GetGitStorageAccountDetails> ExecuteAsync(GetGitStorageAccountDetails request, Metadata metadata, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(metadata);

        return request with
        {
            Result = await _factory
                .GetStateAsync(metadata.AggregateGlobalId, cancellationToken)
                .ConfigureAwait(false)
                    ?? throw new InvalidOperationException($"GitStorageAccount type {metadata.AggregateGlobalId} not found."),
        };
    }
}